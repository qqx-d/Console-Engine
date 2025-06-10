using System.Numerics;

public class Camera3D
{
    public Vector3 Position = new(0, 0, -30);
    public Vector3 Rotation = new(0, 0, 0);
    public float Fov = 60f;

    public Vector2 Project(Vector3 worldPos, int screenWidth, int screenHeight)
    {
        var view = GetViewMatrix();
        var proj = GetProjectionMatrix(screenWidth, screenHeight);

        var world = new Vector4(worldPos, 1f);
        var clip = Vector4.Transform(world, view * proj);

        if (clip.W <= 0.01f)
            return new Vector2(-9999, -9999);

        var ndc = clip / clip.W;

        return new Vector2(
            (ndc.X + 1f) * 0.5f * screenWidth,
            (1f - ndc.Y) * 0.5f * screenHeight
        );
    }

    private Matrix4x4 GetViewMatrix()
    {
        var rotation = Matrix4x4.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);

        var forward = Vector3Extensions.Transform(-Vector3.UnitZ.ToNumerics(), rotation);
        var up = Vector3Extensions.Transform(Vector3.UnitY.ToNumerics(), rotation);
        var target = Position + forward;

        return Matrix4x4.CreateLookAt(Position.ToNumerics(), target, up);
    }

    private Matrix4x4 GetProjectionMatrix(int width, int height)
    {
        var aspectRatio = width / (float)height;
        var fovRadians = Fov * MathF.PI / 180f;
        return Matrix4x4.CreatePerspectiveFieldOfView(fovRadians, aspectRatio, 0.1f, 1000f);
    }

    public Vector3 WorldToCamera(Vector3 world)
    {
        var view = GetViewMatrix();
        var world4 = new Vector4(world.ToNumerics(), 1f);
        var cam = Vector4.Transform(world4, view);
        return new Vector3(cam.X, cam.Y, cam.Z);
    }
}