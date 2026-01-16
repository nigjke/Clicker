using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace ZapretMouseMacro
{
    public class MouseClicker
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;

        private readonly List<Point> points = new List<Point>();
        private readonly int clickDelay;

        public MouseClicker(int clickDelayMs)
        {
            clickDelay = clickDelayMs;
        }

        public void AddPoint()
        {
            GetCursorPos(out POINT p);
            points.Add(new Point(p.X, p.Y));
            Click();
            Console.WriteLine($"Точка добавлена: X={p.X}, Y={p.Y}");
        }

        public void ClickCurrentCursorThenAllPoints()
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

            Console.WriteLine("Клики выполнены: текущая позиция + все точки.");
        }

        public void ClearPoints()
        {
            points.Clear();
            Console.WriteLine("Все точки очищены.");
        }

        private void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
