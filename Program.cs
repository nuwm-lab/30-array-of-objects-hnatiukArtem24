using System;

namespace ArithmeticProgressionApp
{
    class ArithmeticProgression
    {
        // Властивості
        public double A0 { get; }  // перший член
        public double D { get; }   // різниця
        public int N { get; }      // кількість членів

        // Конструктор з валідацією
        public ArithmeticProgression(double a0, double d, int n)
        {
            if (n <= 0)
                throw new ArgumentException("Кількість елементів (n) повинна бути більшою за 0.");

            A0 = a0;
            D = d;
            N = n;
        }

        // Властивість для обчислення суми
        public double Sum => N / 2.0 * (2 * A0 + (N - 1) * D);

        public override string ToString()
        {
            return $"a₀ = {A0}, d = {D}, n = {N}, сума = {Sum:F2}";
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Write("Введіть кількість прогресій n: ");

            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Некоректне значення n.");
                return;
            }

            var progressions = new ArithmeticProgression[n];
            var random = new Random();

            Console.Write("Бажаєте вводити дані вручну? (y/n): ");
            string? choice = Console.ReadLine()?.Trim().ToLower();

            for (int i = 0; i < n; i++)
            {
                progressions[i] = CreateProgression(i + 1, choice, random);
            }

            int maxIndex = FindIndexOfMaxSum(progressions);

            Console.WriteLine("\n=== Усі прогресії ===");
            for (int i = 0; i < n; i++)
                Console.WriteLine($"#{i + 1}: {progressions[i]}");

            Console.WriteLine($"\nПрогресія з найбільшою сумою — #{maxIndex + 1}");
            Console.WriteLine(progressions[maxIndex]);
        }

        // Метод для створення прогресії
        static ArithmeticProgression CreateProgression(int index, string? mode, Random random)
        {
            Console.WriteLine($"\nПрогресія #{index}:");
            double a0, d;
            int n;

            if (mode == "y")
            {
                a0 = ReadDouble("  Введіть перший член (a₀): ");
                d = ReadDouble("  Введіть різницю (d): ");
                n = ReadInt("  Введіть кількість членів (n): ");
            }
            else
            {
                a0 = random.Next(-10, 11);
                d = random.Next(-5, 6);
                n = random.Next(2, 11);
                Console.WriteLine($"  Згенеровано: a₀={a0}, d={d}, n={n}");
            }

            return new ArithmeticProgression(a0, d, n);
        }

        // Метод для знаходження індексу прогресії з максимальною сумою
        static int FindIndexOfMaxSum(ArithmeticProgression[] progressions)
        {
            if (progressions == null || progressions.Length == 0)
                throw new ArgumentException("Масив прогресій порожній або не ініціалізований.");

            int maxIndex = 0;
            double maxSum = progressions[0].Sum;

            for (int i = 1; i < progressions.Length; i++)
            {
                double sum = progressions[i].Sum;
                if (sum > maxSum)
                {
                    maxSum = sum;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        // Допоміжні методи для зчитування з консолі
        static double ReadDouble(string message)
        {
            double value;
            Console.Write(message);
            while (!double.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("  Помилка! Введіть число ще раз: ");
            }
            return value;
        }

        static int ReadInt(string message)
        {
            int value;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out value) || value <= 0)
            {
                Console.Write("  Помилка! Введіть додатне ціле число: ");
            }
            return value;
        }
    }
}

