﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//1 is used
namespace Lab1TimeSeries
{
    class Program
    {

        public class Matrix
        {

            Tuple<double[][], int[]> LUPDecomposition(double[][] A)
            {
                int n = A.Length - 1;
                /*
                * pi represents the permutation matrix.  We implement it as an array
                * whose value indicates which column the 1 would appear.  We use it to avoid 
                * dividing by zero or small numbers.
                * */
                int[] pi = new int[n + 1];
                double p = 0;
                int kp = 0;
                int pik = 0;
                int pikp = 0;
                double aki = 0;
                double akpi = 0;

                //Initialize the permutation matrix, will be the identity matrix
                for (int j = 0; j <= n; j++)
                {
                    pi[j] = j;
                }

                for (int k = 0; k <= n; k++)
                {
                    /*
                    * In finding the permutation matrix p that avoids dividing by zero
                    * we take a slightly different approach.  For numerical stability
                    * We find the element with the largest 
                    * absolute value of those in the current first column (column k).  If all elements in
                    * the current first column are zero then the matrix is singluar and throw an
                    * error.
                    * */
                    p = 0;
                    for (int i = k; i <= n; i++)
                    {
                        if (Math.Abs(A[i][k]) > p)
                        {
                            p = Math.Abs(A[i][k]);
                            kp = i;
                        }
                    }
                    if (p == 0)
                    {
                        throw new Exception("singular matrix");
                    }
                    /*
                    * These lines update the pivot array (which represents the pivot matrix)
                    * by exchanging pi[k] and pi[kp].
                    * */
                    pik = pi[k];
                    pikp = pi[kp];
                    pi[k] = pikp;
                    pi[kp] = pik;

                    /*
                    * Exchange rows k and kpi as determined by the pivot
                    * */
                    for (int i = 0; i <= n; i++)
                    {
                        aki = A[k][i];
                        akpi = A[kp][i];
                        A[k][i] = akpi;
                        A[kp][i] = aki;
                    }

                    /*
                        * Compute the Schur complement
                        * */
                    for (int i = k + 1; i <= n; i++)
                    {
                        A[i][k] = A[i][k] / A[k][k];
                        for (int j = k + 1; j <= n; j++)
                        {
                            A[i][j] = A[i][j] - (A[i][k] * A[k][j]);
                        }
                    }
                }
                return Tuple.Create(A, pi);
            }
            double[] LUPSolve(double[][] LU, int[] pi, double[] b)
            {
                int n = LU.Length - 1;
                double[] x = new double[n + 1];
                double[] y = new double[n + 1];
                double suml = 0;
                double sumu = 0;
                double lij = 0;

                /*
                * Solve for y using formward substitution
                * */
                for (int i = 0; i <= n; i++)
                {
                    suml = 0;
                    for (int j = 0; j <= i - 1; j++)
                    {
                        /*
                        * Since we've taken L and U as a singular matrix as an input
                        * the value for L at index i and j will be 1 when i equals j, not LU[i][j], since
                        * the diagonal values are all 1 for L.
                        * */
                        if (i == j)
                        {
                            lij = 1;
                        }
                        else
                        {
                            lij = LU[i][j];
                        }
                        suml = suml + (lij * y[j]);
                    }
                    y[i] = b[pi[i]] - suml;
                }
                //Solve for x by using back substitution
                for (int i = n; i >= 0; i--)
                {
                    sumu = 0;
                    for (int j = i + 1; j <= n; j++)
                    {
                        sumu = sumu + (LU[i][j] * x[j]);
                    }
                    x[i] = (y[i] - sumu) / LU[i][i];
                }
                return x;
            }
            double[][] InvertMatrix(double[][] A)
            {
                int n = A.Length;
                //e will represent each column in the identity matrix
                double[] e;
                //x will hold the inverse matrix to be returned
                double[][] x = new double[n][];
                for (int i = 0; i < n; i++)
                {
                    x[i] = new double[A[i].Length];
                }
                /*
                * solve will contain the vector solution for the LUP decomposition as we solve
                * for each vector of x.  We will combine the solutions into the double[][] array x.
                * */
                double[] solve;

                //Get the LU matrix and P matrix (as an array)
                Tuple<double[][], int[]> results = LUPDecomposition(A);

                double[][] LU = results.Item1;
                int[] P = results.Item2;

                /*
                * Solve AX = e for each column ei of the identity matrix using LUP decomposition
                * */
                for (int i = 0; i < n; i++)
                {
                    e = new double[A[i].Length];
                    e[i] = 1;
                    solve = LUPSolve(LU, P, e);
                    for (int j = 0; j < solve.Length; j++)
                    {
                        x[j][i] = solve[j];
                    }
                }
                return x;
            }
            public Matrix ToInverse()
            {
                return new Matrix(InvertMatrix(this.ToArray()));
            }
            //Every list is a column of matrix;
            List<List<double>> m;
            public Matrix()
            {
                //All right
                m = new List<List<double>>();
            }
            public Matrix(List<List<double>> inp)
            {
                m = inp;
            }
            public double[][] ToArray()
            {
                double[][] temp = new double[Rows()][];
                for (int i = 0; i < Rows(); i++)
                    temp[i] = m[i].ToArray();
                return temp;
            }
            public Matrix ToTranspose()
            {
                var temp = new Matrix(Nulificator(Columns(),Rows()));

                for (int i = 0; i < Rows(); i++)
                {
                    for (int j = 0; j < Columns(); j++)
                    {
                        temp.m[j][i] = m[i][j];

                    }
                }

                return temp;
            }
            public Matrix(double[,] inp)
            {
                int c = inp.GetLength(0); //Column Length
                int r = inp.GetLength(1); //Row Length
                m = Nulificator(c, r);

                for (int i = 0; i < c; i++)
                {
                    for (int j = 0; j < r; j++)
                    {
                        m[i][j] = inp[i, j];

                    }
                }
            }
            public Matrix(double[][] inp)
            {
                int c = inp.GetLength(0); //Column Length
                int r = inp[0].GetLength(0); //Row Length
                m = Nulificator(c, r);

                for (int i = 0; i < c; i++)
                {
                    for (int j = 0; j < r; j++)
                    {
                        m[i][j] = inp[i][j];

                    }
                }
            }
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Rows(); i++)
                {
                    for (int j = 0; j < Columns(); j++)
                    {
                        sb.Append(m[i][j].ToString() + "\t");
                    }
                    sb.Append("\n");
                }
                return sb.ToString();
            }
            int Columns() { return m[0].Count; }
            int Rows() { return m.Count; }
            public static List<List<double>> Nulificator(int Rows, int Columns)
            {
                List<List<double>> temp = new List<List<double>>();
                for (int i = 0; i < Rows; i++)
                {
                    temp.Add(new List<double>(new double[Columns]));
                }


                return temp;
            }
            public static Matrix operator *(Matrix m1, Matrix m2)
            {
                //int n = m1.Columns(), m = m1.Rows(), r = m2.Rows();
                Matrix temp = new Matrix();

                int a1 = m1.Columns(); int b0 = m2.Rows();
                int a0 = m1.Rows(); int b1 = m2.Columns();

                if (a1 == b0)
                {
                    temp = new Matrix(Nulificator(a0, b1));
                    int c0 = temp.Rows();
                    int c1 = temp.Columns();
                    ///c = new int[a.GetLength(0), b.GetLength(1)];
                    for (int i = 0; i < c0; i++)
                    {
                        for (int j = 0; j < c1; j++)
                        {
                            temp.m[i][j] = 0;
                            for (int k = 0; k < a1; k++) // OR k<b.GetLength(0)
                                temp.m[i][j] += m1.m[i][k] * m2.m[k][j];
                        }
                    }
                }
                else Console.WriteLine("Error #1 Wrong matrix dimensions!");
                return temp;
                //Value = c1.Value + c2.Value
            }
        }

        static void Main(string[] args)
        {
            Matrix a = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 7, 8, 9 } });
            Matrix b = new Matrix(new double[,] { { 1 }, { 2 }, { 3 } });
            Matrix result = (a.ToTranspose() * a).ToInverse()*a.ToTranspose();//a * b;
            Console.WriteLine(result.ToString());
            Console.ReadLine();
        }
    }
}
