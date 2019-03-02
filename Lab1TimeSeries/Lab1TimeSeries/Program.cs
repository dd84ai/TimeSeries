using System;
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
            Matrix result = a * b;
            Console.WriteLine(result.ToString());
            Console.ReadLine();
        }
    }
}
