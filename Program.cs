using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArithmeticProgressionApp
{
    class ArithmeticProgression
    {
       
        public double A0 { get; }
        public double D { get; }
        public int Count { get; }

      
        public ArithmeticProgression(double a0, double d, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count має бути додатнім.", nameof(count));

            A0 = a0;
            D = d;
            Count = count;
        }

     
        public double Sum()
        {
            return Count / 2.0 * (2 * A0 + (Count - 1) * D);
        }

        public override string ToString()
        {
            return $"A0={A0}, D={D}, Count={Count}, Sum={Sum():F2}";
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Арифметичні прогресії ===");

            Console.Write("Введіть кількість прогресій (m): ");
            if (!int.TryParse(Console.ReadLine(), out int m) || m <= 0)
            {
                Console.WriteLine("Некоректне число. Завершення програми.");
                return;
            }

            var list = new List<ArithmeticProgression>();
            var rnd = new Random();

            Console.Write("Ви хочете вводити прогресії вручну? (y/n): ");
            string choice = Console.ReadLine()?.Trim().ToLower() ?? "n";

            for (int i = 0; i < m; i++)
            {
                double a0, d;
                int count;

                if (choice == "y")
                {
                    Console.WriteLine($"\nПрогресія #{i + 1}:");
                    a0 = ReadDouble("Введіть A0: ");
                    d = ReadDouble("Введіть D: ");
                    count = ReadInt("Введіть Count (>0): ", minValue: 1);
                }
                else
                {
                    a0 = rnd.Next(-10, 11);
                    d = rnd.Next(-5, 6);
                    count = rnd.Next(1, 11);
                }

                list.Add(new ArithmeticProgression(a0, d, count));
            }

            Console.WriteLine("\n=== Результати ===");
            for (int i = 0; i < list.Count; i++)
                Console.WriteLine($"[{i}] {list[i]}");

            // Знаходження прогресії з найбільшою сумою
            int maxIndex = 0;
            double maxSum = list[0].Sum();

            for (int i = 1; i < list.Count; i++)
            {
                double sum = list[i].Sum();
                if (sum > maxSum)
                {
                    maxSum = sum;
                    maxIndex = i;
                }
            }

            Console.WriteLine($"\nПрогресія з найбільшою сумою:");
            Console.WriteLine($"Індекс: {maxIndex}");
            Console.WriteLine(list[maxIndex]);

          
            RunManualTests();

            Console.WriteLine("\nНатисніть Enter для виходу...");
            Console.ReadLine();
        }

     

        static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    return value;
                Console.WriteLine("Невірний формат. Спробуйте ще раз (використовуйте крапку як роздільник).");
            }
        }

        static int ReadInt(string prompt, int minValue = int.MinValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= minValue)
                    return value;
                Console.WriteLine("Невірне значення. Спробуйте ще раз.");
            }
        }

        static void RunManualTests()
        {
            Console.WriteLine("\n=== Перевірка правильності формули ===");

            var test1 = new ArithmeticProgression(1, 1, 5);
            var test2 = new ArithmeticProgression(2, 3, 4); 
            var test3 = new ArithmeticProgression(10, -2, 5); 
            Console.WriteLine($"Очікується 15 → {test1.Sum()}");
            Console.WriteLine($"Очікується 26 → {test2.Sum()}");
            Console.WriteLine($"Очікується 30 → {test3.Sum()}");
        }
    }
}
