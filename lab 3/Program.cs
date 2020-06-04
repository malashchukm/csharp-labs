using System;

namespace lab_3
{
    class Program
    {
        static double[,,] C;
        static double[,] A, B;
        static int m_counter = 0;
        // tweak to change max rand value
        // (1 - maxRandValue)
        static int maxValue = 10;


        static void Main(string[] args)
        {
            Console.WriteLine("Enter matrix size");
            int size = Convert.ToInt32(Console.ReadLine());

            C = new double[size, size, size + 1];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    C[i, j, 0] = 0;
                }
            }

            A = new double[size, size];
            setA(ref A, size);
            printMatrix(A, "Matrix A");

            B = new double[size, size];
            genB(ref B, size);
            printMatrix(B, "Matrix B");

            singleAssigment();
            localRecursive(0, 0, 0);

            string title = "Local recursive algorithm\nNumber of operations: " + m_counter.ToString();
            printMatrix3D(C, title);
        }

        static void setA(ref double[,] matrix, int size)
        {
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j + i < size)
                    {
                        matrix[i, j] = j + i + 1;
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }
                }
            }
        }

        static void genB(ref double[,] matrix, int size)
        {
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j + i < size && i >= j)
                    {
                        matrix[i, j] = rand.Next(1, maxValue);
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }
                }
            }
        }

        // одноразове присвоєння
        static void singleAssigment()
        {
            int counter = 0;
            int size = A.GetLength(0);
            double[,,] Z = new double[size, size, size + 1];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Z[i, j, 0] = 0;
                    for (int k = 0; k < size; k++)
                    {
                        Z[i, j, k + 1] = Z[i, j, k] + A[i, k] * B[k, j];
                        counter += 2;
                    }
                }
            }
            string title = "Single variable assigment\nNumber of operations: " + counter.ToString();
            printMatrix3D(Z, title);
        }

        // локально рекурсивний алгоритм
        static void localRecursive(int i, int j, int k)
        {
            int size = A.GetLength(0);

            if (i < size && j < size && k < size)
            {
                if (A[i, k] != 0 && B[k, j] != 0)
                {
                    C[i, j, size] += A[i, k] * B[k, j];
                    m_counter += 2;
                }
                localRecursive(i, j, k + 1);
                if (k == size - 1)
                {
                    // reset k and j because it makes new branch
                    // to run less empty runs which count the same values
                    k = 0;
                    localRecursive(i, j + 1, k);

                    if (j == size - 1)
                    {
                        j = 0;
                        localRecursive(i + 1, j, k);
                    }
                }
            }
        }

        static void printMatrix(double[,] matrix, string title)
        {
            Console.WriteLine(title + '\n');
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(string.Format("{0} ", matrix[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        static void printMatrix3D(double[,,] matrix, string title)
        {
            Console.WriteLine(title + '\n');
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(string.Format("{0} ", matrix[i, j, matrix.GetLength(2) - 1]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }
    }
}
