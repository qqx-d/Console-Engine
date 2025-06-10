using System.Numerics;

public class Transform
{
    public Vector3 Position = Vector3.Zero;
    public Vector3 Rotation = Vector3.Zero;
    public Vector3 Scale = Vector3.One;

    public Vector3 Apply(Vector3 point)
    {
        return point.Transform(this);
    }

    public Matrix4x4 GetMatrix()
    {
        var scale = Matrix4x4.CreateScale(Scale.ToNumerics());
        var rotation = Matrix4x4.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
        var translation = Matrix4x4.CreateTranslation(Position.ToNumerics());

        // Новый порядок: scale -> rotate -> translate
        return scale * rotation * translation;
    }
}