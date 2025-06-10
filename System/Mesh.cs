using System.Numerics;
using Learn.System;

public class Mesh
{
    public Vertex[] Vertices;
    public int[] Indices;
    public Transform Transform;

    private static readonly Vector3 LightDirection = Vector3.Normalize(new Vector3(-0.5f, -0.5f, -0.5f));
    private static readonly string ShadeChars = ".:-=+*#%@";

    public Mesh(Vertex[] vertices, int[] indices)
    {
        Vertices = vertices;
        Indices = indices;
        Transform = new Transform();
    }

    public void Draw(Window window, Camera3D camera)
    {
        for (int i = 0; i < Indices.Length; i += 3)
        {
            var v0 = Vertices[Indices[i]];
            var v1 = Vertices[Indices[i + 1]];
            var v2 = Vertices[Indices[i + 2]];

            var wp0 = Transform.Apply(v0.Position);
            var wp1 = Transform.Apply(v1.Position);
            var wp2 = Transform.Apply(v2.Position);

            // Нормаль грани
            var edge1 = wp2.ToNumerics() - wp0.ToNumerics();
            var edge2 = wp1.ToNumerics() - wp0.ToNumerics();
            var normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

            var center = (wp0.ToNumerics() + wp1.ToNumerics() + wp2.ToNumerics()) / 3f;
            var toCamera = Vector3.Normalize(camera.Position.ToNumerics() - center);

            if (Vector3.Dot(normal, toCamera) <= 0f)
                continue;

            // Освещённость на вершинах
            float l0 = MathF.Max(0f, Vector3.Dot(normal, LightDirection));
            float l1 = MathF.Max(0f, Vector3.Dot(normal, LightDirection));
            float l2 = MathF.Max(0f, Vector3.Dot(normal, LightDirection));

            var s0 = camera.Project(wp0, window.ConsoleWidth, window.ConsoleHeight);
            var s1 = camera.Project(wp1, window.ConsoleWidth, window.ConsoleHeight);
            var s2 = camera.Project(wp2, window.ConsoleWidth, window.ConsoleHeight);

            if (s0.X < -1000 || s1.X < -1000 || s2.X < -1000)
                continue;

            var cv0 = camera.WorldToCamera(wp0);
            var cv1 = camera.WorldToCamera(wp1);
            var cv2 = camera.WorldToCamera(wp2);

            float avgDepth = -(cv0.Z + cv1.Z + cv2.Z) / 3f;

            FillTriangle(window, s0, s1, s2, avgDepth, l0, l1, l2, v0.Color);
        }
    }

    
    private void FillTriangle(Window window, Vector2 p0, Vector2 p1, Vector2 p2, float depth, float l0, float l1, float l2, ConsoleColor color)
{
    if (p1.Y < p0.Y) { (p0, p1) = (p1, p0); (l0, l1) = (l1, l0); }
    if (p2.Y < p0.Y) { (p0, p2) = (p2, p0); (l0, l2) = (l2, l0); }
    if (p2.Y < p1.Y) { (p1, p2) = (p2, p1); (l1, l2) = (l2, l1); }

    void DrawScanline(float y, float xStart, float xEnd, float lightStart, float lightEnd, float zStart, float zEnd)
    {
        if (y < 0 || y >= window.ConsoleHeight) return;

        int xs = (int)MathF.Round(MathF.Min(xStart, xEnd));
        int xe = (int)MathF.Round(MathF.Max(xStart, xEnd));
        int yy = (int)MathF.Round(y);

        for (int x = xs; x <= xe; x++)
        {
            float t = xe == xs ? 0f : (x - xs) / (float)(xe - xs);
            float light = MathX.Lerp(lightStart, lightEnd, t);
            float z = MathX.Lerp(zStart, zEnd, t);

            int index = (int)(light * (ShadeChars.Length - 1));
            index = Math.Clamp(index, 0, ShadeChars.Length - 1);
            char shadeChar = ShadeChars[index];

            window.SetPixelDepth(x, yy, z, shadeChar, color);
        }
    }

    float EdgeX(Vector2 a, Vector2 b, float y)
    {
        if (b.Y - a.Y == 0) return a.X;
        return a.X + (y - a.Y) * (b.X - a.X) / (b.Y - a.Y);
    }

    float EdgeVal(float va, float vb, float ay, float by, float y)
    {
        if (by - ay == 0) return va;
        return va + (y - ay) * (vb - va) / (by - ay);
    }

    // Переводы глубин в отдельные переменные для интерполяции
    float z0 = depth - 0.001f; // Примерная поправка
    float z1 = depth;
    float z2 = depth + 0.001f;

    for (float y = p0.Y; y <= p2.Y; y++)
    {
        if (y < 0 || y >= window.ConsoleHeight) continue;

        bool upper = y < p1.Y;

        Vector2 a = upper ? p0 : p1;
        Vector2 b = upper ? p1 : p2;
        float la = upper ? l0 : l1;
        float lb = upper ? l1 : l2;
        float za = upper ? z0 : z1;
        float zb = upper ? z1 : z2;

        float xA = EdgeX(p0, p2, y);
        float xB = EdgeX(a, b, y);
        float lA = EdgeVal(l0, l2, p0.Y, p2.Y, y);
        float lB = EdgeVal(la, lb, a.Y, b.Y, y);
        float zA = EdgeVal(z0, z2, p0.Y, p2.Y, y);
        float zB = EdgeVal(za, zb, a.Y, b.Y, y);

        DrawScanline(y, xA, xB, lA, lB, zA, zB);
    }
}

}
