public class Camera2D
{
    public Vector2 Position;
    public float Zoom = 1f;
    public float Rotation; // в радианах

    public Vector2 Project(Vector2 world, int screenWidth, int screenHeight)
    {
        var relative = world - Position;
        
        var sin = MathF.Sin(Rotation);
        var cos = MathF.Cos(Rotation);
        var rotated = new Vector2(
            relative.X * cos - relative.Y * sin,
            relative.X * sin + relative.Y * cos
        );
        
        var scaled = rotated * Zoom;
        
        var screenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);
        return screenCenter + scaled;
    }
}