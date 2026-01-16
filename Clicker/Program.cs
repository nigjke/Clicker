using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    [DllImport("user32.dll")]
    static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll")]
    static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    const int MOUSEEVENTF_LEFTUP = 0x0004;
    const int WM_HOTKEY = 0x0312;

    struct POINT { public int X; public int Y; }

    [StructLayout(LayoutKind.Sequential)]
    struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    static List<POINT> points = new List<POINT>();
    static bool isAdding = true;
    static int clickDelay = 200;

    static void Main()
    {
        try
        {
            RunProgram();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }

    static void RunProgram()
    {
        Console.WriteLine("'zapret' is ALREADY RUNNING as service, use 'service.bat' and choose 'Remove Services' first if you want to run standalone bat.");
        Console.WriteLine("Press any key to continue . . .");

        var key = Console.ReadKey(true);

        if (!char.IsDigit(key.KeyChar))
        {
            Console.WriteLine("Нажата неверная клавиша. Программа завершена.");
            Environment.Exit(0);
        }

        Console.Clear();
        Console.Write("Введите задержку между кликами (мс): ");
        if (!int.TryParse(Console.ReadLine(), out clickDelay))
        {
            clickDelay = 80; 
        }

        Console.WriteLine("F8 - добавить точку");
        Console.WriteLine("F9 - закончить добавление");
        Console.WriteLine("F10 - очистить все точки");

        RegisterHotKey(IntPtr.Zero, 1, 0, (uint)ConsoleKey.F8);
        RegisterHotKey(IntPtr.Zero, 2, 0, (uint)ConsoleKey.F9);
        RegisterHotKey(IntPtr.Zero, 3, 0, (uint)ConsoleKey.F10);

        while (GetMessage(out MSG msg, IntPtr.Zero, 0, 0))
        {
            if (msg.message != WM_HOTKEY) continue;

            if (msg.wParam == (IntPtr)1)
            {
                if (isAdding)
                    AddPoint();
                else
                    ClickCurrentCursorThenAllPoints();
            }

            if (msg.wParam == (IntPtr)2)
            {
                isAdding = false;
                Console.WriteLine("Режим добавления завершён.");
            }

            if (msg.wParam == (IntPtr)3)
            {
                points.Clear();
                Console.WriteLine("Все точки очищены.");
            }
        }

        UnregisterHotKey(IntPtr.Zero, 1);
        UnregisterHotKey(IntPtr.Zero, 2);
        UnregisterHotKey(IntPtr.Zero, 3);
    }

    static void AddPoint()
    {
        GetCursorPos(out POINT p);
        points.Add(p);
        Click();
        Console.WriteLine($"Точка добавлена: X={p.X}, Y={p.Y}");
    }

    static void ClickCurrentCursorThenAllPoints()
    {
        GetCursorPos(out POINT current);

        SetCursorPos(current.X, current.Y);
        Click();
        Thread.Sleep(clickDelay);

        foreach (var p in points)
        {
            SetCursorPos(p.X, p.Y);
            Click();
            Thread.Sleep(clickDelay);
        }
    }

    static void Click()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }
}
