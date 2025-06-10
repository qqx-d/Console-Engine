namespace Learn.System;

public static class MathX
{
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}