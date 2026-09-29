using System;

class Lab3
{
    const int N = 4;

    static void Main()
    {
        double[,] C = new double[N, N];
        double[] b = new double[N];

        // Ввод матрицы C
        Console.WriteLine("Введите матрицу C:");

        for (int i = 0; i < N; i++)
        {
            string[] values = Console.ReadLine().Split();

            for (int j = 0; j < N; j++)
            {
                C[i, j] = double.Parse(values[j]);
            }
         }

        // Ввод вектора b
        Console.WriteLine("Введите вектор b:");

        string[] vector = Console.ReadLine().Split();

        for (int i = 0; i < N; i++)
        {
            b[i] = double.Parse(vector[i]);
        }

        // Единичная матрица
        double[,] A = new double[N, N];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (i == j)
                    A[i, j] = 1 - C[i, j];
                else
                    A[i, j] = -C[i, j];
            }
        }

        // y = b
        double[] y = b;

        Console.WriteLine("\nМатрица A:");
        PrintMatrix(A);

        Console.WriteLine("\nВектор y:");
        PrintVector(y);

        // Норма матрицы A
        double normA = 0;

        for (int i = 0; i < N; i++)
        {
            double rowSum = 0;

            for (int j = 0; j < N; j++)
            {
                rowSum += Math.Abs(A[i, j]);
            }

            if (rowSum > normA)
                normA = rowSum;
        }

        Console.WriteLine($"\nНорма матрицы A: {normA:F6}");

        if (normA >= 1)
        {
            Console.WriteLine(
                "Предупреждение: норма A >= 1, " +
                "сходимость метода Якоби не гарантируется."
            );
        }

        // Начальное приближение
        double[] x = new double[N];

        // Точность
        double eps = 0.001;

        int iteration = 0;
        double error = 0;

        // Метод Якоби
        while (iteration < 10000)
        {
            iteration++;

            double[] xNew = new double[N];

            for (int i = 0; i < N; i++)
            {
                xNew[i] = y[i];

                for (int j = 0; j < N; j++)
                {
                    xNew[i] += A[i, j] * x[j];
                }
            }

            // Максимальная разница между итерациями
            error = 0;

            for (int i = 0; i < N; i++)
            {
                double difference = Math.Abs(xNew[i] - x[i]);

                if (difference > error)
                    error = difference;
            }

            x = xNew;

            if (error < eps)
                break;
        }

        Console.WriteLine("\nРешение:");
        PrintVector(x);

        Console.WriteLine($"\nКоличество итераций: {iteration}");
        Console.WriteLine($"Погрешность: {error:F6}");

        // Проверка Cx = b
        double[] check = new double[N];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                check[i] += C[i, j] * x[j];
            }
        }

        Console.WriteLine("\nПроверка Cx:");
        PrintVector(check);

        Console.WriteLine("\nИсходный b:");
        PrintVector(b);

        bool correct = true;

        for (int i = 0; i < N; i++)
        {
            if (Math.Abs(check[i] - b[i]) > eps)
            {
                correct = false;
                break;
            }
        }

        Console.WriteLine(
            "\nРезультат: " +
            (correct ? "решение верное" : "решение требует уточнения")
        );
    }

    static void PrintMatrix(double[,] matrix)
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write($"{matrix[i, j],10:F4}");
            }

            Console.WriteLine();
        }
    }

    static void PrintVector(double[] vector)
    {
        for (int i = 0; i < N; i++)
        {
            Console.Write($"{vector[i],10:F4}");
        }

        Console.WriteLine();
    }
}