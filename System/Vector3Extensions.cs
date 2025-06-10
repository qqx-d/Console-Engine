using System.Numerics;

public static class Vector3Extensions
{
    public static Vector3 Transform(this Vector3 v, Matrix4x4 m)
    {
        var result = Vector4.Transform(new Vector4(v.X, v.Y, v.Z, 1f), m);
        return new Vector3(result.X, result.Y, result.Z);
    }

    public static Vector3 Transform(this Vector3 v, Transform t)
    {
        return v.Transform(t.GetMatrix());
    }
    
    public static Vector3 ToCustom(this System.Numerics.Vector3 v) =>
        new Vector3(v.X, v.Y, v.Z);
}