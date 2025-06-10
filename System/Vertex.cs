public struct Vertex
{
    public Vector3 Position;
    public char Symbol;
    public ConsoleColor Color;

    public Vertex(float x, float y, float z, char symbol, ConsoleColor color)
    {
        Position = new Vector3(x, y, z);
        Symbol = symbol;
        Color = color;
    }
}