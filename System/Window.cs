using System.Text;

public abstract class Window
{
    private const int CharWidthPx = 8;
    private const int CharHeightPx = 16;
    
    public int ConsoleWidth { get; private set; }
    public int ConsoleHeight { get; private set; }

    private char[,] _bufferA, _bufferB;
    private ConsoleColor[,] _colorA, _colorB;
    private char[,] _drawBuffer, _writeBuffer;
    private ConsoleColor[,] _drawColor, _writeColor;
    private float[,] _depthBuffer;

    private DateTime _lastFrameTime = DateTime.Now;
    private float _deltaTime;

    public float DeltaTime => _deltaTime;
    
    public Window(int realWidth, int realHeight)
    {
        ConsoleWidth = Math.Min(realWidth / CharWidthPx, Console.LargestWindowWidth);
        ConsoleHeight = Math.Min(realHeight / CharHeightPx, Console.LargestWindowHeight);

        Console.SetBufferSize(ConsoleWidth, ConsoleHeight);
        Console.SetWindowSize(ConsoleWidth, ConsoleHeight);

        _bufferA = new char[ConsoleWidth, ConsoleHeight];
        _bufferB = new char[ConsoleWidth, ConsoleHeight];
        _colorA = new ConsoleColor[ConsoleWidth, ConsoleHeight];
        _colorB = new ConsoleColor[ConsoleWidth, ConsoleHeight];

        _drawBuffer = _bufferA;
        _writeBuffer = _bufferB;
        _drawColor = _colorA;
        _writeColor = _colorB;

        _depthBuffer = new float[ConsoleWidth, ConsoleHeight];
    }

    public void Run()
    {
        OnLoad();

        while (true)
        {
            var now = DateTime.Now;
            _deltaTime = (float)(now - _lastFrameTime).TotalSeconds;
            _lastFrameTime = now;

            Input.Update();
            OnUpdateFrame();
            SwapBuffers();
            OnRenderFrame();

            var elapsed = (DateTime.Now - now).TotalMilliseconds;
            int sleepTime = Math.Max(0, 16 - (int)elapsed);
            Thread.Sleep(sleepTime);

            if (Input.IsKeyPressed(ConsoleKey.Escape)) break;
        }
    }

    private void SwapBuffers()
    {
        (_drawBuffer, _writeBuffer) = (_writeBuffer, _drawBuffer);
        (_drawColor, _writeColor) = (_writeColor, _drawColor);
    }

    public void Clear(char c = ' ', ConsoleColor color = ConsoleColor.Black)
    {
        for (int y = 0; y < ConsoleHeight; y++)
        for (int x = 0; x < ConsoleWidth; x++)
        {
            _writeBuffer[x, y] = c;
            _writeColor[x, y] = color;
            _depthBuffer[x, y] = float.MaxValue;
        }
    }

    public void SetPixel(int x, int y, char c, ConsoleColor color)
    {
        if (x < 0 || x >= ConsoleWidth || y < 0 || y >= ConsoleHeight) return;
        _writeBuffer[x, y] = c;
        _writeColor[x, y] = color;
    }

    public void SetPixelDepth(int x, int y, float depth, char c, ConsoleColor color)
    {
        if (x < 0 || x >= ConsoleWidth || y < 0 || y >= ConsoleHeight) return;

        if (depth < _depthBuffer[x, y])
        {
            _depthBuffer[x, y] = depth;
            _writeBuffer[x, y] = c;
            _writeColor[x, y] = color;
        }
    }

    private void OnRenderFrame()
    {
        for (int y = 0; y < ConsoleHeight; y++)
        {
            int x = 0;
            while (x < ConsoleWidth)
            {
                var color = _drawColor[x, y];
                var segment = new StringBuilder();

                while (x < ConsoleWidth && _drawColor[x, y] == color)
                {
                    segment.Append(_drawBuffer[x, y]);
                    x++;
                }

                Console.ForegroundColor = color;
                Console.SetCursorPosition(x - segment.Length, y);
                Console.Write(segment);
            }
        }

        Console.ResetColor();
        Console.SetCursorPosition(0, 0);
    }

    protected virtual void OnLoad() { }
    protected virtual void OnUpdateFrame() { }
}
