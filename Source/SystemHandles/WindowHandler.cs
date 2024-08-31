using System.Runtime.InteropServices;

namespace OpenVikings.SystemHandles
{
    internal static class WindowHandler
    {
        // Windows API constants
        private const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        private const int WS_VISIBLE = 0x10000000;
        private const int SW_SHOW = 5;

        // Windows API methods
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CreateWindowEx(
            int dwExStyle,
            string lpClassName,
            string lpWindowName,
            int dwStyle,
            int x, int y, int nWidth, int nHeight,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern ushort RegisterClass(ref WNDCLASS lpWndClass);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string? lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr DispatchMessage(ref MSG lpMsg);

        [StructLayout(LayoutKind.Sequential)]
        private struct WNDCLASS
        {
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hWnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        // Window Procedure to handle window messages
        private static IntPtr WindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            switch (uMsg)
            {
                case 0x0010: // WM_CLOSE
                    DestroyWindow(hWnd);
                    PostQuitMessage(0);
                    return IntPtr.Zero;
            }

            return DefWindowProc(hWnd, uMsg, wParam, lParam);
        }

        internal static void CreateFullScreenWindow(string windowName)
        {
            // Get the module handle
            IntPtr hInstance = GetModuleHandle(null);

            // Register the window class
            WNDCLASS wndClass = new()
            {
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(WindowProc),
                hInstance = hInstance,
                lpszClassName = windowName
            };
            RegisterClass(ref wndClass);

            // Create the window
            IntPtr hWnd = CreateWindowEx(
                0, windowName, windowName, WS_OVERLAPPEDWINDOW | WS_VISIBLE,
                0, 0, 1920, 1080, IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

            // Show the window in full screen
            ShowWindow(hWnd, SW_SHOW);

            // Main message loop
            MSG msg;
            while (GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }
    }
}