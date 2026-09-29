using System;
using System.Diagnostics;

class Lab2
{
    const int N = 4;

    static Random random = new Random();

    static void Main()
    {
        // Создание матрицы A
        double[,] A = new double[N, N];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                A[i, j] = random.Next(-10, 11);
            }
        }

        // Создание вектора b
        double[] b = new double[N];

        for (int i = 0; i < N; i++)
        {
            b[i] = random.Next(-10, 11);
        }

        Console.WriteLine("Матрица A:");
        PrintMatrix(A);

        Console.WriteLine("\nВектор b:");
        PrintVector(b);

        // Измерение времени LU-разложения и решения
        Stopwatch stopwatch = Stopwatch.StartNew();

        double[] x = LUSolve(A, b);

        stopwatch.Stop();

        Console.WriteLine("\nРешение x:");
        PrintVector(x);

        // Проверка решения
        double[] check = CheckSolution(A, x);

        Console.WriteLine("\nПроверка A * x:");
        PrintVector(check);

        Console.WriteLine("\nИсходный вектор b:");
        PrintVector(b);

        bool correct = true;

        for (int i = 0; i < N; i++)
        {
            if (Math.Abs(check[i] - b[i]) > 1e-9)
            {
                correct = false;
                break;
            }
        }

        Console.WriteLine(
            "\nРезультат проверки: " +
            (correct ? "Решение верное" : "Решение неверное")
        );

        Console.WriteLine(
            $"Время работы: {stopwatch.Elapsed.TotalSeconds:F10} секунд"
        );
    }


    static double[] LUSolve(double[,] A, double[] b)
    {
        double[,] LU = new double[N, N];

        // Копирование A
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                LU[i, j] = A[i, j];
            }
        }

        // Массив перестановок
        int[] P = new int[N];

        for (int i = 0; i < N; i++)
        {
            P[i] = i;
        }

        // LU-разложение: PA = LU
        for (int k = 0; k < N; k++)
        {
            // Поиск главного элемента
            int maxRow = k;

            for (int i = k + 1; i < N; i++)
            {
                if (Math.Abs(LU[i, k]) >
                    Math.Abs(LU[maxRow, k]))
                {
                    maxRow = i;
                }
            }

            if (Math.Abs(LU[maxRow, k]) < 1e-12)
            {
                throw new Exception("Матрица вырожденная");
            }

            // Перестановка строк
            for (int j = 0; j < N; j++)
            {
                double temp = LU[k, j];
                LU[k, j] = LU[maxRow, j];
                LU[maxRow, j] = temp;
            }

            // Перестановка индексов
            int tempP = P[k];
            P[k] = P[maxRow];
            P[maxRow] = tempP;

            // Вычисление L и U
            for (int i = k + 1; i < N; i++)
            {
                LU[i, k] /= LU[k, k];

                for (int j = k + 1; j < N; j++)
                {
                    LU[i, j] -= LU[i, k] * LU[k, j];
                }
            }
        }

        // Решение Ly = Pb
        double[] y = new double[N];

        for (int i = 0; i < N; i++)
        {
            y[i] = b[P[i]];

            for (int j = 0; j < i; j++)
            {
                y[i] -= LU[i, j] * y[j];
            }
        }

        // Решение Ux = y
        double[] x = new double[N];

        for (int i = N - 1; i >= 0; i--)
        {
            x[i] = y[i];

            for (int j = i + 1; j < N; j++)
            {
                x[i] -= LU[i, j] * x[j];
            }

            x[i] /= LU[i, i];
        }

        return x;
    }


    static double[] CheckSolution(double[,] A, double[] x)
    {
        double[] result = new double[N];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                result[i] += A[i, j] * x[j];
            }
        }

        return result;
    }


    static void PrintMatrix(double[,] A)
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write($"{A[i, j],8:F3}");
            }

            Console.WriteLine();
        }
    }


    static void PrintVector(double[] vector)
    {
        for (int i = 0; i < N; i++)
        {
            Console.Write($"{vector[i],8:F3}");
        }

        Console.WriteLine();
    }
}