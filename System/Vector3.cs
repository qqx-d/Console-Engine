using System;

public struct Vector3
{
    public float X, Y, Z;

    public static Vector3 One = new Vector3(1, 1, 1);
    public static Vector3 Zero = new Vector3(0, 0, 0);

    public static Vector3 UnitX => new Vector3(1f, 0f, 0f);
    public static Vector3 UnitY => new Vector3(0f, 1f, 0f);
    public static Vector3 UnitZ => new Vector3(0f, 0f, 1f);

    public Vector3(float x, float y, float z) =>
        (X, Y, Z) = (x, y, z);

    public static Vector3 operator +(Vector3 a, Vector3 b) =>
        new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3 operator -(Vector3 a, Vector3 b) =>
        new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector3 operator *(Vector3 v, float scalar) =>
        new(v.X * scalar, v.Y * scalar, v.Z * scalar);

    public static Vector3 operator /(Vector3 v, float scalar) =>
        new(v.X / scalar, v.Y / scalar, v.Z / scalar);

    public static Vector3 operator -(Vector3 v) =>
        new(-v.X, -v.Y, -v.Z);
    
    public static float Dot(Vector3 a, Vector3 b) =>
        a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static Vector3 Cross(Vector3 a, Vector3 b) =>
        new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );

    public static Vector3 Normalize(Vector3 v)
    {
        float length = MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        if (length == 0f) return Vector3.Zero;
        return v / length;
    }

    public System.Numerics.Vector3 ToNumerics() => new(X, Y, Z);

    public override string ToString() =>
        $"({X:0.##}, {Y:0.##}, {Z:0.##})";

    public static implicit operator System.Numerics.Vector3(Vector3 v) => new(v.X, v.Y, v.Z);

    public static implicit operator Vector3(System.Numerics.Vector3 v) => new(v.X, v.Y, v.Z);
}