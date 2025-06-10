using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class Input
{
    private static readonly HashSet<ConsoleKey> CurrentKeys = [];
    private static HashSet<ConsoleKey> _previousKeys = [];

    public static float MouseDeltaX { get; private set; }
    public static float MouseDeltaY { get; private set; }

    [DllImport("user32.dll")] private static extern short GetAsyncKeyState(int vKey);
    [DllImport("user32.dll")] private static extern bool GetCursorPos(out POINT lpPoint);
    [DllImport("user32.dll")] private static extern bool SetCursorPos(int X, int Y);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X; public int Y; }

    private static int _centerX;
    private static int _centerY;
    private static bool _mouseInitialized;

    public static void InitializeMouse(int pixelScreenWidth, int pixelScreenHeight)
    {
        _centerX = pixelScreenWidth / 2;
        _centerY = pixelScreenHeight / 2;
        _mouseInitialized = true;
        SetCursorPos(_centerX, _centerY);
    }

    public static void Update()
    {
        // Update keyboard state
        _previousKeys = [..CurrentKeys];
        CurrentKeys.Clear();

        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(intercept: true).Key;
            CurrentKeys.Add(key);
        }

        // Update mouse state
        if (_mouseInitialized && GetCursorPos(out var pos))
        {
            MouseDeltaX = pos.X - _centerX;
            MouseDeltaY = pos.Y - _centerY;

            SetCursorPos(_centerX, _centerY);
        }
    }

    public static bool IsKeyDown(ConsoleKey key) => CurrentKeys.Contains(key);
    public static bool IsKeyPressed(ConsoleKey key) => CurrentKeys.Contains(key) && !_previousKeys.Contains(key);
    public static bool IsKeyReleased(ConsoleKey key) => !CurrentKeys.Contains(key) && _previousKeys.Contains(key);
}