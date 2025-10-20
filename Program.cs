using System;

namespace TetrahedronApp
{
    struct Point3D
    {
        public double X, Y, Z;
        public Point3D(double x, double y, double z) { X = x; Y = y; Z = z; }
        public override string ToString() => $"({X}, {Y}, {Z})";
    }

    class Tetrahedron
    {
        private static int _nextId = 1;
        private int _id;
        public Point3D P1 { get; }
        public Point3D P2 { get; }
        public Point3D P3 { get; }
        public Point3D P4 { get; }

        public Tetrahedron(Point3D p1, Point3D p2, Point3D p3, Point3D p4)
        {
            P1 = p1; P2 = p2; P3 = p3; P4 = p4;
            _id = _nextId++;
            Console.WriteLine($"Tetrahedron #{_id} created.");
        }

        // Фіналізатор (деструктор у C#)
        ~Tetrahedron()
        {
            // У фіналізаторі не рекомендується робити складну логіку,
            // але для демонстрації виведемо повідомлення.
            Console.WriteLine($"Finalizer called for Tetrahedron #{_id}");
        }

        // Обчислення об'єму: |det(a,b,c)| / 6
        public double Volume()
        {
            var a = Sub(P2, P1);
            var b = Sub(P3, P1);
            var c = Sub(P4, P1);
            double det = Determinant(a, b, c);
            return Math.Abs(det) / 6.0;
        }

        private static Point3D Sub(Point3D u, Point3D v) =>
            new Point3D(u.X - v.X, u.Y - v.Y, u.Z - v.Z);

        private static double Determinant(Point3D a, Point3D b, Point3D c)
        {
            // Розгорнутий обчислювач детермінанта 3x3
            return a.X * (b.Y * c.Z - b.Z * c.Y)
                 - a.Y * (b.X * c.Z - b.Z * c.X)
                 + a.Z * (b.X * c.Y - b.Y * c.X);
        }

        public override string ToString()
        {
            return $"Tetrahedron #{_id}: {P1}, {P2}, {P3}, {P4}";
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Програма: масив тетраедрів — знайти тетраедр з найбільшим об'ємом.");
            Console.Write("Введіть кількість тетраедрів (наприклад, 3): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Невірне число. Вихід.");
                return;
            }

            Tetrahedron[] arr = new Tetrahedron[n];

            Console.WriteLine("Ви хочете вводити координати вручну? (y/n)");
            string choice = Console.ReadLine()?.Trim().ToLower();

            for (int i = 0; i < n; i++)
            {
                if (choice == "y")
                {
                    Console.WriteLine($"Тетраедр {i + 1}: введіть 4 вершини (x y z) по черзі:");
                    var p1 = ReadPoint(i + 1, 1);
                    var p2 = ReadPoint(i + 1, 2);
                    var p3 = ReadPoint(i + 1, 3);
                    var p4 = ReadPoint(i + 1, 4);
                    arr[i] = new Tetrahedron(p1, p2, p3, p4);
                }
                else
                {
                    // Автозаповнення для прикладу — випадкові координати
                    var rnd = new Random(i + Environment.TickCount);
                    Point3D r1 = new Point3D(rnd.NextDouble() * 5, rnd.NextDouble() * 5, rnd.NextDouble() * 5);
                    Point3D r2 = new Point3D(rnd.NextDouble() * 5, rnd.NextDouble() * 5, rnd.NextDouble() * 5);
                    Point3D r3 = new Point3D(rnd.NextDouble() * 5, rnd.NextDouble() * 5, rnd.NextDouble() * 5);
                    Point3D r4 = new Point3D(rnd.NextDouble() * 5, rnd.NextDouble() * 5, rnd.NextDouble() * 5);
                    arr[i] = new Tetrahedron(r1, r2, r3, r4);
                }
            }

            // Знаходимо тетраедр з найбільшим об'ємом
            double maxVol = double.NegativeInfinity;
            int maxIdx = -1;
            for (int i = 0; i < n; i++)
            {
                double vol = arr[i].Volume();
                Console.WriteLine($"[{i}] {arr[i].ToString()}  Volume = {vol:F6}");
                if (vol > maxVol)
                {
                    maxVol = vol;
                    maxIdx = i;
                }
            }

            Console.WriteLine();
            if (maxIdx >= 0)
            {
                Console.WriteLine($"Тетраедр з найбільшим об'ємом: індекс {maxIdx}, об'єм = {maxVol:F6}");
                Console.WriteLine(arr[maxIdx].ToString());
            }
            else
            {
                Console.WriteLine("Не знайдено жодного тетраедра.");
            }

            // --- Демонстрація Garbage Collection ---
            Console.WriteLine();
            Console.WriteLine("Демонстрація Garbage Collection і виклику фіналізаторів.");
            Console.WriteLine("Видаляємо посилання на масив та створимо декілька тимчасових об'єктів.");
            // видаляємо посилання на масив
            arr = null;

            // Створимо тимчасові об'єкти, потім видалимо посилання
            for (int i = 0; i < 5; i++)
            {
                var tmp = new Tetrahedron(
                    new Point3D(i, i + 1, i + 2),
                    new Point3D(i + 1, i + 2, i + 3),
                    new Point3D(i + 2, i + 3, i + 4),
                    new Point3D(i + 3, i + 4, i + 5)
                );
                // Видаляємо посилання — об'єкт стане доступним для GC
                tmp = null;
            }

            Console.WriteLine("Викликаємо GC.Collect() і чекаємо завершення фіналізаторів...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("GC завершив роботу. Якщо фіналізатори були викликані, ви побачите відповідні повідомлення вище.");
            Console.WriteLine("Кінець програми. Натисніть Enter для виходу.");
            Console.ReadLine();
        }

        static Point3D ReadPoint(int tetraIndex, int vertexNumber)
        {
            while (true)
            {
                Console.Write($"T{tetraIndex} V{vertexNumber} (x y z): ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3
                    && double.TryParse(parts[0], out double x)
                    && double.TryParse(parts[1], out double y)
                    && double.TryParse(parts[2], out double z))
                {
                    return new Point3D(x, y, z);
                }
                Console.WriteLine("Невірний ввід. Спробуйте ще раз.");
            }
        }
    }
}
