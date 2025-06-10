using System.Runtime.InteropServices;

public static class CursorUtils
{
    [DllImport("user32.dll")]
    private static extern int ShowCursor(bool bShow);

    private static bool _isHidden = false;

    public static void Hide()
    {
        if (!_isHidden)
        {
            while (ShowCursor(false) >= 0);
            _isHidden = true;
        }
    }

    public static void Show()
    {
        if (_isHidden)
        {
            while (ShowCursor(true) < 0);
            _isHidden = false;
        }
    }
}