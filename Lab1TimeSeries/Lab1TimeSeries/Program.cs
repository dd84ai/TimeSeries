using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                m = new List<List<double>>();
            }
            public Matrix(List<List<double>> inp)
            {
                m = inp;
            }
            public Matrix(double[,] inp)
            {
                int c = inp.GetLength(0); //Column Length
                int r = inp.Length / inp.GetLength(0); //Row Length
                m = Nulificator(c, r);

                for (int i = 0; i < c; i++)
                {
                    for (int j = 0; j < r; j++)
                    {
                        m[j][i] = inp[i, j];

                    }
                }
            }
            public override string ToString()
            {
                return "";
            }
            int Columns() { return m.Count; }
            int Rows() { return m[0].Count; }
            public static List<List<double>> Nulificator(int Rows, int Columns)
            {
                List<List<double>> temp = new List<List<double>>();
                for (int i = 0; i < Columns; i++)
                {
                    temp.Add(new List<double>(new double[Rows]));
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
                            temp.m[j][i] = 0;
                            for (int k = 0; k < a1; k++) // OR k<b.GetLength(0)
                                temp.m[j][i] += m1.m[k][i] * m2.m[j][k];
                        }
                    }
                }
                return temp;
                //Value = c1.Value + c2.Value
            }
        }

        static void Main(string[] args)
        {
            Matrix a = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 7, 8, 9 } });
            Matrix b = new Matrix(new double[,] { { 1 }, { 2 }, { 3 } });
            Matrix result = a * b;


        }
    }
}
