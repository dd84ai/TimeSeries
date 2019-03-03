using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1TimeSeries
{
    // C# program to find rank of a matrix 
    class GFG
    {
        static double error = 0.0000000001;
        // function for exchanging two rows 
        // of a matrix  
        static void swap(double[,] mat,
              int row1, int row2, int col)
        {
            for (int i = 0; i < col; i++)
            {
                double temp = mat[row1, i];
                mat[row1, i] = mat[row2, i];
                mat[row2, i] = temp;
            }
        }

        // Function to display a matrix 
        static void display(int[,] mat,
                         int row, int col)
        {
            for (int i = 0; i < row; i++)
            {

                for (int j = 0; j < col; j++)

                    Console.Write(" "
                              + mat[i, j]);

                Console.Write("\n");
            }
        }

        // function for finding rank of matrix  
        public static int rankOfMatrix(double[,] mat, int R, int C)
        {

            int rank = C;

            for (int row = 0; row < rank; row++)
            {

                // Before we visit current row  
                // 'row', we make sure that  
                // mat[row][0],....mat[row][row-1] 
                // are 0. 

                // Diagonal element is not zero 
                if (mat[row, row] != 0)
                {
                    for (int col = 0; col < R; col++)
                    {
                        if (col != row)
                        {
                            // This makes all entries  
                            // of current column  
                            // as 0 except entry  
                            // 'mat[row][row]' 
                            double mult =
                               (double)mat[col, row] /
                                        mat[row, row];

                            for (int i = 0; i < rank; i++)

                                mat[col, i] -= (int)mult
                                         * mat[row, i];
                        }
                    }
                }

                // Diagonal element is already zero.  
                // Two cases arise: 
                // 1) If there is a row below it  
                // with non-zero entry, then swap  
                // this row with that row and process  
                // that row 
                // 2) If all elements in current  
                // column below mat[r][row] are 0,  
                // then remvoe this column by  
                // swapping it with last column and 
                // reducing number of columns by 1. 
                else
                {
                    bool reduce = true;

                    // Find the non-zero element  
                    // in current column  
                    for (int i = row + 1; i < R; i++)
                    {
                        // Swap the row with non-zero  
                        // element with this row. 
                        if (mat[i, row] != 0)
                        {
                            swap(mat, row, i, rank);
                            reduce = false;
                            break;
                        }
                    }

                    // If we did not find any row with  
                    // non-zero element in current  
                    // columnm, then all values in  
                    // this column are 0. 
                    if (reduce)
                    {
                        // Reduce number of columns 
                        rank--;

                        // Copy the last column here 
                        for (int i = 0; i < R; i++)
                            mat[i, row] = mat[i, rank];
                    }

                    // Process this row again 
                    row--;
                }

                // Uncomment these lines to see  
                // intermediate results display(mat, R, C); 
                // printf("\n"); 
            }

            return rank;
        }

        // Driver code 
        public static void SubMain()
        {
            double[,] mat = {{10, 20, 10},
                       {-20, -30, 10},
                       {30, 50, 0}};

            Console.Write("Rank of the matrix is : "
                              + rankOfMatrix(mat,3,3));
        }
    }
}
