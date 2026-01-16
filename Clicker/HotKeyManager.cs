using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZapretMouseMacro
{
    public class HotKeyManager
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        private const int WM_HOTKEY = 0x0312;

        private readonly Dictionary<int, Action> hotKeyActions = new Dictionary<int, Action>();

        public void RegisterHotKey(int id, ConsoleKey key, Action action)
        {
            if (!RegisterHotKey(IntPtr.Zero, id, 0, (uint)key))
                throw new Exception($"Не удалось зарегистрировать хоткей {key}");

            hotKeyActions[id] = action;
        }

        public void UnregisterAll()
        {
            foreach (var id in hotKeyActions.Keys)
                UnregisterHotKey(IntPtr.Zero, id);
        }

        public void RunMessageLoop()
        {
            while (GetMessage(out MSG msg, IntPtr.Zero, 0, 0))
            {
                if (msg.message == WM_HOTKEY && hotKeyActions.TryGetValue((int)msg.wParam, out var action))
                {
                    action.Invoke();
                }
            }
        }
    }
}
