using System;

namespace ZapretMouseMacro
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("'zapret' is ALREADY RUNNING as service.");
            Console.WriteLine("Press any key to continue . . .");

            bool isChecked = false;
            var key = Console.ReadKey(true);
            if (!char.IsDigit(key.KeyChar) && !isChecked)
                Environment.Exit(0);

            isChecked = true;

            Console.Clear();
            Console.Write("Введите задержку между кликами (мс): ");
            int clickDelay = int.Parse(Console.ReadLine());

            var clicker = new MouseClicker(clickDelay);
            var hotKeys = new HotKeyManager();

            bool isAdding = true;

            Console.WriteLine("F8 - добавить точку / клик по всем после F9");
            Console.WriteLine("F9 - закончить добавление");
            Console.WriteLine("F10 - очистить все точки");

            hotKeys.RegisterHotKey(1, ConsoleKey.F8, () =>
            {
                if (isAdding) clicker.AddPoint();
                else clicker.ClickCurrentCursorThenAllPoints();
            });

            hotKeys.RegisterHotKey(2, ConsoleKey.F9, () =>
            {
                isAdding = false;
                Console.WriteLine("Режим добавления завершён.");
            });

            hotKeys.RegisterHotKey(3, ConsoleKey.F10, () =>
            {
                clicker.ClearPoints();
            });

            hotKeys.RunMessageLoop();
            hotKeys.UnregisterAll();
        }
    }
}
