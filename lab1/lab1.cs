using System.Diagnostics;

class lab1
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

        // Измерение времени решения
        Stopwatch stopwatch = Stopwatch.StartNew();

        double[] x = Gaussian(A, b);

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


    static double[] Gaussian(double[,] A, double[] b)
    {
        // Расширенная матрица [A | b]
        double[,] matrix = new double[N, N + 1];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                matrix[i, j] = A[i, j];
            }

            matrix[i, N] = b[i];
        }


        // Прямой ход метода Гаусса
        for (int i = 0; i < N; i++)
        {
            // Поиск строки с максимальным ведущим элементом
            int maxRow = i;

            for (int j = i + 1; j < N; j++)
            {
                if (Math.Abs(matrix[j, i]) >
                    Math.Abs(matrix[maxRow, i]))
                {
                    maxRow = j;
                }
            }

            // Перестановка строк
            for (int k = 0; k <= N; k++)
            {
                double temp = matrix[i, k];
                matrix[i, k] = matrix[maxRow, k];
                matrix[maxRow, k] = temp;
            }

            // Проверка на вырожденность
            if (Math.Abs(matrix[i, i]) < 1e-12)
            {
                throw new Exception(
                    "Система не имеет единственного решения"
                );
            }

            // Обнуление элементов ниже главного элемента
            for (int j = i + 1; j < N; j++)
            {
                double factor = matrix[j, i] / matrix[i, i];

                for (int k = i; k <= N; k++)
                {
                    matrix[j, k] -= factor * matrix[i, k];
                }
            }
        }


        // Обратный ход
        double[] x = new double[N];

        for (int i = N - 1; i >= 0; i--)
        {
            x[i] = matrix[i, N];

            for (int j = i + 1; j < N; j++)
            {
                x[i] -= matrix[i, j] * x[j];
            }

            x[i] /= matrix[i, i];
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