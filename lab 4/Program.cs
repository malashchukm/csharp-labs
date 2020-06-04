using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab_4
{
    class Program
    {
        static int size;
        static Matrix matrix_to_check;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter size of the matrix");
            size = Convert.ToInt32(Console.ReadLine());

            matrix_to_check = new Matrix(size);

            Console.WriteLine("Use random values? y/n");
            ConsoleKey response = Console.ReadKey(true).Key;

            if (response == ConsoleKey.Y)
            {
                matrix_to_check.RandomInitMatrix();
            }
            else
            {
                matrix_to_check.HandInit("matrix");
            }
            
            matrix_to_check.ShowMatrix("Matrix");

            CheckSaddlePoints();
        }

        // return list of tuples with indices of max values in each column
        public static List<Tuple<int, int>> GetMaxInColumnList()
        {
            List<Task> tasks = new List<Task>();
            var positions = new List<Tuple<int, int>>();
            for (int i = 0; i < matrix_to_check.GetLength(1); i++)
            {
                // creating new thread for each iteration of for
                int index = i;
                var task = Task.Factory.StartNew(() =>
                {
                    // receiving indices of max element in column
                    positions.Add(FindColumnMax(index));
                });
                tasks.Add(task);

            }

            Task.WaitAll(tasks.ToArray());
            return positions;
        }

        // return tuple with (row, column) indices of max element in column 
        public static Tuple<int, int> FindColumnMax(int i)
        {
            // Initialize max to 0 at begining 
            // of finding max element of each column
            int maxm = matrix_to_check[0, i];
            int max_j = 0;
            for (int j = 1; j < matrix_to_check.GetLength(0); j++)
            {

                if (matrix_to_check[j, i] > maxm)
                {
                    maxm = matrix_to_check[j, i];
                    max_j = j;
                }
            }
            return new Tuple<int, int>(max_j, i);
        }

        // return list of tuples with indices of min values in each row
        public static List<Tuple<int, int>> GetMinInRowList()
        {
            List<Task> tasks = new List<Task>();
            var positions = new List<Tuple<int, int>>();
            for (int i = 0; i < matrix_to_check.GetLength(1); i++)
            {

                int index = i;
                var task = Task.Factory.StartNew(() =>
                {
                    positions.Add(FindRowMin(index));
                });
                tasks.Add(task);

            }
            Task.WaitAll(tasks.ToArray());
            return positions;
        }

        // return tuple with (row, column) indices of min element in row 
        public static Tuple<int, int> FindRowMin(int i)
        {
            // Initialize max to 0 at begining 
            // of finding max element of each column
            int minm = matrix_to_check[0, i];
            int min_j = 0;
            for (int j = 1; j < matrix_to_check.GetLength(0); j++)
            {

                if (matrix_to_check[i, j] < minm)
                {
                    minm = matrix_to_check[j, i];
                    min_j = j;
                }
            }
            return new Tuple<int, int>(min_j, i);
        }

        // prints all values of indices list
        public static void PrintIndecesList(List<Tuple<int, int>> n_indices) 
        {
            foreach (Tuple<int, int> indeces in n_indices)
            {
                Console.WriteLine(indeces.Item1.ToString() + " " + indeces.Item2.ToString());
            }
        }

        // preforms all gets and prints saddle points
        public static void CheckSaddlePoints() {
            var max_indices = GetMaxInColumnList();

            var min_indices = GetMinInRowList();

            Console.WriteLine("All saddle points");

            foreach (Tuple<int, int> indices in min_indices) {
                // checks all intersections between min and max lists
                // and prints them out
                if (max_indices.Contains(indices)) {
                    Console.WriteLine(string.Format("({0}, {1})", indices.Item1.ToString(), indices.Item2.ToString()));
                }
            }
        }
    }
    
    public class Matrix
    {
        public static int MaxRandValue = 10;
        protected int n;
        public int Size
        {
            get { return n; }
        }
        public int[,] matrix;

        public Matrix(int n)
        {
            if (n < 0) throw new Exception("Invalid size");
            this.n = n;
            matrix = new int[n, n];
        }

        public int this[int i, int j]
        {
            get
            {
                return matrix[i, j];
            }
            protected set
            {
                matrix[i, j] = value;
            }
        }


        public void HandInit(string message)
        {
            Console.WriteLine("Enter {0}", message);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.WriteLine("Enter {0}[{1}, {2}]:", message, i.ToString(), j.ToString());
                    matrix[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
        }

        public void RandomInitMatrix()
        {
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = rnd.Next(1, MaxRandValue);
                }
            }
        }

        public void ShowMatrix(string message)
        {
            Console.WriteLine("Matrix: {0}", message);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(string.Format("{0}   ", matrix[i, j].ToString()));
                }
                Console.WriteLine();
            }
        }

        public int GetLength(int i) {
            return matrix.GetLength(i);
        }
    }
}
