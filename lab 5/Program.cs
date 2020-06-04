﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab_5
{
    class Program
    {
        static void Main(string[] args)
        {
            MainProcess obj = new MainProcess();
            obj.Start();
        }
    }

    class MainProcess
    {
        int n;
        public Matrix A1, A2, B2, C2, A, b1, c1, bi, Y3, y1, y2, Y3squared, Y3cubed;
        public Matrix comp2_1, comp2_2, comp4_1, comp4_2, comp4_3, comp4_4, comp5_1, comp5_2, comp6_1, comp6_2, comp7;
        public Matrix result;

        public void Start()
        {

            Console.WriteLine("Enter n: ");
            n = Convert.ToInt32(Console.ReadLine());
            InitStage();
        }

        private void InitStage()
        {
            //init stage
            //initialised by formula
            bi = new Matrix(n);
            var create_bi = Task.Factory.StartNew(() => { bi.Create_bi(); });
            C2 = new Matrix(n);
            var create_C2 = Task.Factory.StartNew(() => { C2.Create_C2(); });

            Console.WriteLine("Initialise random? y/n");
            ConsoleKey answer = Console.ReadKey(true).Key;

            if (answer == ConsoleKey.Y)
            {
                A = new Matrix(n);
                A1 = new Matrix(n);
                A2 = new Matrix(n);
                B2 = new Matrix(n);
                b1 = new Matrix(n);
                c1 = new Matrix(n);
                //creating and starting new task for each initial matrix to run in sync
                var create_A = Task.Factory.StartNew(() => { A.RandomInitMatrix(); });
                var create_A1 = Task.Factory.StartNew(() => { A1.RandomInitMatrix(); });
                var create_A2 = Task.Factory.StartNew(() => { A2.RandomInitMatrix(); });
                var create_B2 = Task.Factory.StartNew(() => { B2.RandomInitMatrix(); });
                var create_b1 = Task.Factory.StartNew(() => { b1.RandomInitVector(); });
                var create_c1 = Task.Factory.StartNew(() => { c1.RandomInitVector(); });
                Task.WaitAll();
            }
            else
            {
                Console.WriteLine("Initialise your matrices");
                A = new Matrix(n);
                A1 = new Matrix(n);
                A2 = new Matrix(n);
                B2 = new Matrix(n);
                b1 = new Matrix(n);
                c1 = new Matrix(n);
                A.HandInit("A");
                A1.HandInit("A1");
                A2.HandInit("A2");
                B2.HandInit("B2");
                b1.HandInit("b1");
                c1.HandInit("c1");
            }
            MainSync();
        }

        private void Calc_comp2_1()
        {
            comp2_1 = b1 + c1;

        }

        private void Calc_comp2_2()
        {
            comp2_2 = B2 - C2;
        }

        private void Calc_y1()
        {
            y1 = A * bi;
        }

        private void Calc_y2()
        {
            y2 = A1 * comp2_1;
        }

        private void Calc_Y3()
        {
            Y3 = A2 * comp2_2;
        }

        private void Calc_Y3squared()
        {
            Y3squared = Y3 * Y3;
        }

        private void Calc_comp4_1()
        {
            comp4_1 = y2.GetTransposed() * y1;
        }

        private void Calc_comp4_2()
        {
            comp4_2 = y2.GetTransposed() * Y3;
        }

        private void Calc_comp4_3()
        {
            comp4_3 = y1.GetTransposed() * y2;
        }

        private void Calc_comp4_4()
        {
            comp4_4 = y1 * Y3;
        }

        private void Calc_Y3cubed()
        {
            Y3cubed = Y3squared * Y3;
        }

        private void Calc_comp5_1()
        {
            comp5_1 = comp4_1 * Y3squared;
        }

        private void Calc_comp5_2()
        {
            comp5_2 = comp4_2 * comp4_4;
        }

        private void Calc_comp6_1()
        {
            comp6_1 = Y3cubed * comp4_3;
        }
        private void Calc_comp6_2()
        {
            comp6_2 = comp5_2 + Y3;
        }

        private void Calc_comp7()
        {
            comp7 = comp5_1 + comp6_1;
        }

        private void Calc_result()
        {
            result = comp7 + comp6_2;
        }

        private void MainSync()
        {
            //stage 2 process 
            var tasks2 = new List<Task>
            {
                Task.Factory.StartNew(Calc_comp2_1),
                Task.Factory.StartNew(Calc_comp2_2),
                Task.Factory.StartNew(Calc_y1)
            };

            //stage 3 process 
            Task.WaitAll(tasks2.ToArray());
            var tasks3 = new List<Task>
            {
                Task.Factory.StartNew(Calc_y2),
                Task.Factory.StartNew(Calc_Y3)
            };

            //stage 4 process Y3^2
            Task.WaitAll(tasks3.ToArray());
            var tasks4 = new List<Task>
            {
                Task.Factory.StartNew(Calc_Y3squared),
                Task.Factory.StartNew(Calc_comp4_1),
                Task.Factory.StartNew(Calc_comp4_2),
                Task.Factory.StartNew(Calc_comp4_3),
                Task.Factory.StartNew(Calc_comp4_4)
            };

            //stage 5 process
            Task.WaitAll(tasks4.ToArray());
            var tasks5 = new List<Task>
            {
                Task.Factory.StartNew(Calc_Y3cubed),
                Task.Factory.StartNew(Calc_comp5_1),
                Task.Factory.StartNew(Calc_comp5_2)
            };

            //stage 6 process
            Task.WaitAll(tasks5.ToArray());
            var tasks6 = new List<Task>
            {
                Task.Factory.StartNew(Calc_comp6_1),
                Task.Factory.StartNew(Calc_comp6_2)
            };

            //stage 7 process 
            Task.WaitAll(tasks6.ToArray());
            var task7 = Task.Factory.StartNew(Calc_comp7);

            //Last stage
            task7.Wait();
            var result = Task.Factory.StartNew(Calc_result);

            result.Wait();
            ResultStage();
        }

        private void ResultStage()
        {
            Console.WriteLine("Show matrices? y/n");
            ConsoleKey answer = Console.ReadKey(true).Key;

            //A1, A2, B2, C2, A, b1, c1, bi, Y3, y1, y2, Y3squared, Y3cubed
            if (answer == ConsoleKey.Y)
            {
                Console.WriteLine("Matrices:");
                A1.ShowMatrix("A1");
                A2.ShowMatrix("A2");
                B2.ShowMatrix("B2");
                C2.ShowMatrix("C2");
                A.ShowMatrix("A");
                b1.ShowMatrix("b1");
                c1.ShowMatrix("c1");
                bi.ShowMatrix("bi");
                Y3.ShowMatrix("Y3");
                y1.ShowMatrix("y1");
                y2.ShowMatrix("y2");
                result.ShowMatrix("result");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
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
        public double[,] matrix;

        public Matrix(int n)
        {
            if (n < 0) throw new Exception("Invalid size");
            this.n = n;
            matrix = new double[n, n];
        }

        public double this[int i, int j]
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

        public Matrix GetTransposed()
        {
            Matrix result = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result.matrix[i, j] = matrix[j, i];
                }
            }
            return result;
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

        public void RandomInitVector()
        {
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j == 0)
                    {
                        matrix[i, j] = rnd.Next(0, MaxRandValue);
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }

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
                    Console.Write(string.Format("{0:N2}   ", matrix[i, j]));
                }
                Console.WriteLine();
            }
        }

        public void Create_C2()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = 1.0 / (Math.Pow(i + 1, 3) + Math.Pow(j + 1, 2));
                }
            }
        }

        public void Create_bi()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j == 0)
                    {
                        matrix[i, j] = 7 * (i + 1);
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }

                }
            }
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.n != m2.n) return new Matrix(2);

            Matrix result = new Matrix(m1.n);

            for (int i = 0; i < m1.n; i++)
            {
                for (int j = 0; j < m1.n; j++)
                {
                    result[i, j] = m1[i, j] + m2[i, j];
                }
            }

            return result;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.n != m2.n) return new Matrix(2);

            Matrix result = new Matrix(m1.n);

            for (int i = 0; i < m1.n; i++)
            {
                for (int j = 0; j < m1.n; j++)
                {
                    result[i, j] = m1[i, j] - m2[i, j];
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.n != m2.n) return new Matrix(2);

            Matrix result = new Matrix(m1.n);

            for (int i = 0; i < m1.n; i++)
            {
                for (int j = 0; j < m1.n; j++)
                {
                    double tmp = 0;
                    for (int k = 0; k < m1.n; k++)
                    {
                        tmp += m1[i, k] * m2[k, j];
                    }
                    result[i, j] = tmp;
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix m1, double value)
        {
            Matrix result = new Matrix(m1.n);

            for (int i = 0; i < m1.n; i++)
            {
                for (int j = 0; j < m1.n; j++)
                {
                    result[i, j] = m1[i, j] * value;

                }
            }
            return result;
        }
    }
}
