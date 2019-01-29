using System;
using System.Collections.Generic;

namespace AdvancedAlgos
{
    class Diet
    {
        private static void Main(string[] args)
        {
            var firstLine = Console.ReadLine().Split();




            Console.ReadKey();
        }
    }

    class LinearEquationsSolver
    {
        double[][] A; //co-efficient matrix
        double[] b; //output matrix for co-efficicnet
        double[] c; //objective function matrix

        int n;//no of input equations
        int m; // no of variables
        double maxValue = Double.MinValue;//.MinValue;
        double[] result = null;
        static double INFINITY = Math.Pow(10, 9);
        private bool bounded = true;

        LinearEquationsSolver(double[][] A, double[] b, double[] c)
        {
            n = A.Length;
            m = c.Length;
            this.A = A;
            this.b = b;
            this.c = c;
            //total no of inequalities = n +m + 1(for infinity)
            int total = n + m + 1;
            compute(total);
        }

        public void print()
        {
            if (maxValue == Double.MinValue)
            {
                Console.WriteLine("No solution");
                return;
            }
            if (!bounded)
            {
                Console.WriteLine("Infinity");
                return;
            }
            Console.WriteLine("Bounded solution");
            for (int i = 0; i < result.Length; i++)
            {
                //System.out.print(String.format("%.15f", result[i]) + " ");
            }
        }

        private void compute(int total)
        {
            int[] arr = new int[total];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }
            genProcessCombinations(arr, total, m);
        }

        /**
         * Process subset of size
         * @param subset
         */
        private void processSubset(List<int> subset)
        {
            double[][] A = new double[m][];
            double[] b = new double[m];
            updateMatrices(subset, A, b);
            GaussianElimination gElim = new GaussianElimination(A, b);
            if (!gElim.hasSolution)
            {
                return;
            }
            double[] temp_result = gElim.b;
            if (!satisfiesAllInEq(temp_result))
            {
                return;
            }
            double val = computeVal(temp_result);
            if (val > maxValue)
            {
                maxValue = val;
                result = temp_result;
                if (subset.Contains(n + m))
                {
                    bounded = false;
                }
                else
                {
                    bounded = true;
                }
            }
        }

        /**
         * Verify result satisfies all the equations
         * @param result
         * @return
         */
        private bool satisfiesAllInEq(double[] result)
        {
            bool satisfied = true;
            //check to see if eq satisfies regular equations
            for (int i = 0; i < n; i++)
            {
                double inEqSum = 0;
                for (int j = 0; j < m; j++)
                {
                    inEqSum += result[j] * this.A[i][j];
                }
                if (inEqSum > b[i] + Math.Pow(10, -3))
                {
                    //not satisfied
                    satisfied = false;
                    break;
                }
            }
            //check to see if it satisfies constraints
            for (int i = 0; i < m; i++)
            {
                if (result[i] * -1 > Math.Pow(10, -3))
                {
                    //not satisfied
                    satisfied = false;
                    break;
                }

            }
            return satisfied;

        }

        private double computeVal(double[] result)
        {
            double val = 0;
            for (int i = 0; i < result.Length; i++)
            {
                val += result[i] * c[i];
            }
            return val;
        }

        /**
         * Update matrices
         */
        private void updateMatrices(List<int> set, double[][] A, double[] b)
        {
            int index = 0;
            foreach (int val in set)
            {
                if (val < n)
                {
                    A[index] = this.A[val];
                    b[index] = this.b[val];
                }
                else if (val < n + m)
                {
                    int diff = val - n;
                    A[index] = new double[m];
                    A[index][diff] = -1;
                    b[index] = 0;
                }
                else
                {
                    A[index] = new double[m];
                    //TODO:  Arrays.fill(A[index], 1);
                    b[index] = INFINITY;
                }
                index += 1;
            }
        }


        /**
          arr[]  ---> Input Array
        data[] ---> Temporary array to store current combination
        start & end ---> Staring and Ending indexes in arr[]
        index  ---> Current index in data[]
        r ---> Size of a combination to be printed
         **/
        void combinationUtil(int[] arr, int n, int r, int index,
                                    int[] data, int i)
        {
            // Current combination is ready to be printed, print it
            if (index == r)
            {
                List<int> set = new List<int>();
                for (int j = 0; j < r; j++)
                    set.Add(data[j]);
                processSubset(set);
                return;
            }

            // When no more elements are there to put in data[]
            if (i >= n)
                return;

            // current is included, put next at next location
            data[index] = arr[i];
            combinationUtil(arr, n, r, index + 1, data, i + 1);

            // current is excluded, replace it with next (Note that
            // i+1 is passed, but index is not changed)
            combinationUtil(arr, n, r, index, data, i + 1);
        }

        /**
         *     The main function that prints all combinations of size r
         *      in arr[] of size n. This function mainly uses combinationUtil()
         */
        void genProcessCombinations(int[] arr, int n, int r)
        {
            // A temporary array to store all combination one by one
            int[] data = new int[r];
            //Set<Set<int>> result = new HashSet<Set<int>>();
            // Print all combination using temprary array 'data[]'
            combinationUtil(arr, n, r, 0, data, 0);
        }
    }

    class GaussianElimination
    {
        private double[][] A; //refers to co-efficient matrix
        public double[] b; //refers to output matrix
        public bool hasSolution = true;


        public GaussianElimination(double[][] A, double[] b)
        {
            if (A == null || b == null)
            {
                hasSolution = false;
                return;
            }
            if (A.Length == 0 || b.Length == 0)
            {
                hasSolution = false;
                return;
            }
            copyMatrix(A);
            copyMatrix(b);
            rowReduce();
        }

        private void rowReduce()
        {
            int rowLength = A.Length;
            for (int row = 0; row < rowLength; row++)
            {
                int rowPivot = getRowPivot(A, row);
                if (rowPivot == -1)
                {
                    hasSolution = false;
                    return;
                }
                if (rowPivot != row)
                {
                    //swap rows
                    swapRowsInA(row, rowPivot);
                    swapIndexInb(row, rowPivot);
                }
                //pivot element is located in col <row> for current row < row>
                //rescale to make pivot as 1
                if (A[row][row] != 1)
                {
                    //rescale entire row
                    rescalePivot(row);
                }
                //make col zero
                for (int r = 0; r < rowLength; r++)
                {
                    if (row == r)
                    {
                        continue;
                    }
                    makeColZero(row, r);

                }
            }
        }

        /**
         * Print result of b
         */
        public void printResult()
        {
            if (hasSolution)
            {
                for (int row = 0; row < b.Length; row++)
                {
                    Console.WriteLine(b[row] + " ");
                }
            }
        }

        /**
         * Make col entries as zero for the pivot row of A and update b
         * @param current_row pi
         * @param row
         */
        private void makeColZero(int current_row, int row)
        {
            double scale_factor = A[row][current_row];
            for (int col = 0; col < A[0].Length; col++)
            {
                A[row][col] = A[row][col] - scale_factor * A[current_row][col];
            }
            b[row] = b[row] - scale_factor * b[current_row];
        }

        private void rescalePivot(int row)
        {
            double scale_factor = A[row][row];
            for (int col = 0; col < A[0].Length; col++)
            {
                A[row][col] = A[row][col] / scale_factor;
            }
            b[row] = b[row] / scale_factor;
        }

        /**
         * Swap rows i and j of A
         */
        private void swapRowsInA(int i, int j)
        {
            double[] temp = A[i];
            A[i] = A[j];
            A[j] = temp;
        }

        /**
         * Swap values in ith and jth index of b
         */
        private void swapIndexInb(int i, int j)
        {
            double temp = b[i];
            b[i] = b[j];
            b[j] = temp;
        }

        private int getRowPivot(double[][] matrix, int row)
        {
            //select first non zero entry in left most column
            for (int r = row; r < matrix.Length; r++)
            {
                if (matrix[r][row] != 0)
                {
                    return r;
                }
            }
            return -1;
        }

        /**
         * Copy input A to have a local copy < avoid modifying client</>
         * @param matrix
         */
        private void copyMatrix(double[][] matrix)
        {
            A = new double[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    this.A[i][j] = matrix[i][j];
                }
            }
        }

        /**
         * Copy input B to have a local copy < avoid modifying client</>
         * @param matrix
         */
        private void copyMatrix(double[] matrix)
        {
            b = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                b[i] = matrix[i];
            }

        }

    }

}
