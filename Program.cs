using System;

class ArithmeticProgression
{
    // Перший член прогресії
    public double A0 { get; }
    // Різниця прогресії
    public double D { get; }
    // Кількість членів
    public int Count { get; }

    // Конструктор з перевіркою правильності
    public ArithmeticProgression(double a0, double d, int count)
    {
        if (count <= 0)
            throw new ArgumentException("Кількість елементів повинна бути більшою за 0.");

        A0 = a0;
        D = d;
        Count = count;
    }

    // Властивість для обчислення суми
    public double Sum => Count * (2 * A0 + (Count - 1) * D) / 2.0;

    // Перевизначення ToString для зручного виводу
    public override string ToString()
    {
        return $"a₀ = {A0}, d = {D}, n = {Count}, сума = {Sum:F2}";
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

        ArithmeticProgression[] progressions = new ArithmeticProgression[n];
        Random random = new Random();

        Console.Write("Ви хочете вводити дані вручну? (y/n): ");
        string? choice = Console.ReadLine();

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"\nПрогресія #{i + 1}:");

            double a0, d;
            int count;

            if (choice?.ToLower() == "y")
            {
                Console.Write("  Введіть перший член (a₀): ");
                a0 = double.Parse(Console.ReadLine()!);

                Console.Write("  Введіть різницю (d): ");
                d = double.Parse(Console.ReadLine()!);

                Console.Write("  Введіть кількість членів (n): ");
                count = int.Parse(Console.ReadLine()!);
            }
            else
            {
                // Генеруємо випадкові значення
                a0 = random.Next(-10, 11);     // від -10 до 10
                d = random.Next(-5, 6);        // від -5 до 5
                count = random.Next(1, 11);    // від 1 до 10
                Console.WriteLine($"  Згенеровано: a₀={a0}, d={d}, n={count}");
            }

            progressions[i] = new ArithmeticProgression(a0, d, count);
        }

        // Знаходимо прогресію з найбільшою сумою
        int maxIndex = 0;
        double maxSum = progressions[0].Sum;

        for (int i = 1; i < n; i++)
        {
            if (progressions[i].Sum > maxSum)
            {
                maxSum = progressions[i].Sum;
                maxIndex = i;
            }
        }

        Console.WriteLine("\n=== Результати ===");
        for (int i = 0; i < n; i++)
            Console.WriteLine($"Прогресія #{i + 1}: {progressions[i]}");

        Console.WriteLine($"\nНайбільша сума у прогресії #{maxIndex + 1}: {progressions[maxIndex]}");
    }
}

