/* Good job! (Max time used: 0.19/3.00, max memory used: 13066240/536870912.) */
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedAlgos
{
    internal class Diet
    {
        private static void Main(string[] args)
        {
            var firstLine = Console.ReadLine().Split();
            var n = int.Parse(firstLine[0]);
            var m = int.Parse(firstLine[1]);

            var matrix = new double[n][];
            
            for (var i = 0; i < n; i++)
            {
                if (matrix[i] == null)
                    matrix[i] = new double[m];
                var input = Console.ReadLine().Split();
                for (var j = 0; j < m; j++)
                {
                    matrix[i][j] = int.Parse(input[j]);
                }
            }
            var b = new double[n];
            var bInput = Console.ReadLine().Split();
            for (var i = 0; i < n; i++)
            {
                b[i] = double.Parse(bInput[i]);
            }

            var c = new double[m];
            var cInput = Console.ReadLine().Split();
            for (var i = 0; i < m; i++)
            {
                c[i] = double.Parse(cInput[i]);
            }

            var linearEquations = new LinearEquations(matrix, b, c);
            linearEquations.PrintResult();
            Console.ReadKey();
        }
    }

    internal class LinearEquations
    {
        private readonly double[][] _a; //co-efficient matrix
        private readonly double[] _b; //output matrix for co-efficicnet
        private readonly double[] _c; //objective function matrix

        private readonly int _n;//no of input equations
        private readonly int _m; // no of variables
        private double _maxValue = double.MinValue;//.MinValue;
        private double[] _result;
        private static readonly double Infinity = Math.Pow(10, 9);
        private bool _bounded = true;

        public LinearEquations(double[][] a, double[] b, double[] c)
        {
            _n = a.Length;
            _m = c.Length;
            _a = a;
            _b = b;
            _c = c;

            var total = _n + _m + 1;
            Compute(total);
        }

        public void PrintResult()
        {
            if (_maxValue == double.MinValue)
            {
                Console.WriteLine("No solution");
                return;
            }
            if (!_bounded)
            {
                Console.WriteLine("Infinity");
                return;
            }
            Console.WriteLine("Bounded solution");
            foreach (var val in _result)
            {
                Console.Write( val.ToString("F15") + " ");
            }
        }

        private void Compute(int total)
        {
            var arr = new int[total];
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }
            GenProcessCombinations(arr, total, _m);
        }

        private void ProcessSubset(List<int> subset)
        {
            var a = new double[_m][];
            var b = new double[_m];
            UpdateMatrices(subset, a, b);
            var gaussElimination = new GaussianElimination(a, b);
            if (!gaussElimination.HasSolution)
            {
                return;
            }
            var tempResult = gaussElimination.B;
            if (!SatisfiesAllInEq(tempResult))
            {
                return;
            }
            var val = ComputeVal(tempResult);
            if (val > _maxValue)
            {
                _maxValue = val;
                _result = tempResult;
                _bounded = !subset.Contains(_n + _m);
            }
        }

        private bool SatisfiesAllInEq(double[] result)
        {
            var satisfied = true;
            //check to see if eq satisfies regular equations
            for (var i = 0; i < _n; i++)
            {
                double inEqSum = 0;
                for (var j = 0; j < _m; j++)
                {
                    inEqSum += result[j] * _a[i][j];
                }
                if (inEqSum > _b[i] + Math.Pow(10, -3))
                {
                    //not satisfied
                    satisfied = false;
                    break;
                }
            }
            //check to see if it satisfies constraints
            for (var i = 0; i < _m; i++)
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

        private double ComputeVal(IReadOnlyList<double> result)
        {
            return result.Select((t, i) => t * _c[i]).Sum();
        }

        private void UpdateMatrices(IEnumerable<int> set, IList<double[]> a, IList<double> b)
        {
            var index = 0;
            foreach (var val in set)
            {
                if (val < _n)
                {
                    a[index] = _a[val];
                    b[index] = _b[val];
                }
                else if (val < _n + _m)
                {
                    var diff = val - _n;
                    a[index] = new double[_m];
                    a[index][diff] = -1;
                    b[index] = 0;
                }
                else
                {
                    a[index] = new double[_m];
                    a[index] = Enumerable.Repeat(1d, _m).ToArray();
                    b[index] = Infinity;
                }
                index += 1;
            }
        }

        private void CombinationUtil(IReadOnlyList<int> arr, int n, int r, int index, IList<int> data, int i)
        {
            if (index == r)
            {
                var set = new List<int>();
                for (var j = 0; j < r; j++)
                    set.Add(data[j]);
                ProcessSubset(set);
                return;
            }

            // When no more elements are there to put in data[]
            if (i >= n)
                return;
            
            data[index] = arr[i];
            CombinationUtil(arr, n, r, index + 1, data, i + 1);
            CombinationUtil(arr, n, r, index, data, i + 1);
        }

        private void GenProcessCombinations(int[] arr, int n, int r)
        {
            var data = new int[r];
            CombinationUtil(arr, n, r, 0, data, 0);
        }
    }

    internal class GaussianElimination
    {
        private double[][] _a; //refers to co-efficient matrix
        public double[] B; //refers to output matrix
        public bool HasSolution = true;


        public GaussianElimination(IReadOnlyList<double[]> a, IReadOnlyList<double> b)
        {
            if (a == null || b == null)
            {
                HasSolution = false;
                return;
            }
            if (a.Count == 0 || b.Count == 0)
            {
                HasSolution = false;
                return;
            }
            CopyMatrix(a);
            CopyMatrix(b);
            RowReduce();
        }

        private void RowReduce()
        {
            var rowLength = _a[0].Length;
            for (var row = 0; row < rowLength; row++)
            {
                var rowPivot = GetRowPivot(_a, row);
                if (rowPivot == -1)
                {
                    HasSolution = false;
                    return;
                }
                if (rowPivot != row)
                {
                    //swap rows
                    SwapRowsInA(row, rowPivot);
                    SwapIndexInb(row, rowPivot);
                }
                //pivot element is located in col <row> for current row < row>
                //rescale to make pivot as 1
                if (_a[row][row] != 1)
                {
                    //rescale entire row
                    RescalePivot(row);
                }
                //make col zero
                for (var r = 0; r < rowLength; r++)
                {
                    if (row == r)
                    {
                        continue;
                    }
                    MakeColZero(row, r);

                }
            }
        }

        private void MakeColZero(int currentRow, int row)
        {
            var scaleFactor = _a[row][currentRow];
            for (var col = 0; col < _a[0].Length; col++)
            {
                _a[row][col] = _a[row][col] - scaleFactor * _a[currentRow][col];
            }
            B[row] = B[row] - scaleFactor * B[currentRow];
        }

        private void RescalePivot(int row)
        {
            var scaleFactor = _a[row][row];
            for (var col = 0; col < _a[0].Length; col++)
            {
                _a[row][col] = _a[row][col] / scaleFactor;
            }
            B[row] = B[row] / scaleFactor;
        }

        /**
         * Swap rows i and j of A
         */
        private void SwapRowsInA(int i, int j)
        {
            var temp = _a[i];
            _a[i] = _a[j];
            _a[j] = temp;
        }

        /**
         * Swap values in ith and jth index of b
         */
        private void SwapIndexInb(int i, int j)
        {
            var temp = B[i];
            B[i] = B[j];
            B[j] = temp;
        }

        private static int GetRowPivot(IReadOnlyList<double[]> matrix, int row)
        {
            //select first non zero entry in left most column
            for (var r = row; r < matrix.Count; r++)
            {
                if (matrix[r][row] != 0)
                {
                    return r;
                }
            }
            return -1;
        }

        private void CopyMatrix(IReadOnlyList<double[]> matrix)
        {
            _a = new double[matrix.Count][];
            for (var i = 0; i < matrix.Count; i++)
            {
                if(_a[i] == null)
                    _a[i] = new double[matrix[0].Length];
                for (var j = 0; j < matrix[0].Length; j++)
                {
                    _a[i][j] = matrix[i][j];
                }
            }
        }

        private void CopyMatrix(IReadOnlyList<double> matrix)
        {
            B = new double[matrix.Count];
            for (var i = 0; i < matrix.Count; i++)
            {
                B[i] = matrix[i];
            }
        }
    }
}
