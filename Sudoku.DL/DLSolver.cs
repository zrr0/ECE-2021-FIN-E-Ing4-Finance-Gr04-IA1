using Sudoku.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DlxLib;

namespace Sudoku.DL
{
    public class DLSolver : ISudokuSolver
    {
         public void Solve(GrilleSudoku s)
        //private static void Main()
        {
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var x = 0;
            
            while (x != 1)
                
            {Console.WriteLine("oui1");
                /*
                 string[] remplir = new string[9];
                 //string b = Convert.ToString( s.GetCellule(0, 0));
                 //if (b == "0") { b = " "; }
                 for (int row = 0; row <= 8; row++)
                 {
                     string b = "";
                     for (int col = 0; col <= 8; col++)
                     {
                         
                             String a = Convert.ToString(s.GetCellule(row, col));
                             if (a == "0")
                             { a = " "; }
                                 //b = int.Parse(string.Concat(b,a));
                              b = b + a;
                         

                     }
                     remplir[row] = b;
                     //Console.WriteLine(remplir[row]);
                     Console.WriteLine(remplir[row].Count());
                 }

          
                var grid = new Grid(ImmutableList.Create(
                    remplir[0], remplir[1], remplir[2], remplir[3], remplir[4], remplir[5], remplir[6], remplir[7], remplir[8]));
               */
                var grid = new Grid(ImmutableList.Create(
                    "6 4 9 7 3",
                    "  3    6 ",
                    "       18",
                    "   18   9",
                    "     43  ",
                    "7   39   ",
                    " 7       ",
                    " 4    8  ",
                    "9 8 6 4 5"));
                Console.WriteLine("oui");
                grid.Draw();
                 var internalRows = BuildInternalRowsForGrid(grid);
                 var dlxRows = BuildDlxRows(internalRows);
                 var solutions = new Dlx()
                     .Solve(dlxRows, d => d, r => r)
                     .Where(solution => VerifySolution(internalRows, solution))
                     .ToImmutableList();

                 Console.WriteLine();

                 if (solutions.Any())
                 {
                     Console.WriteLine($"First solution (of {solutions.Count}):");
                     Console.WriteLine();
                     DrawSolution(internalRows, solutions.First());
                     Console.WriteLine();
                 }
                 else
                 {
                     Console.WriteLine("No solutions found!");
                 }

                var oui = Console.ReadLine();
                
            }
        }

        private static IEnumerable<int> Rows => Enumerable.Range(0, 9);
        private static IEnumerable<int> Cols => Enumerable.Range(0, 9);
        private static IEnumerable<Tuple<int, int>> Locations =>
            from row in Rows
            from col in Cols
            select Tuple.Create(row, col);
        private static IEnumerable<int> Digits => Enumerable.Range(1, 9);

        private static IImmutableList<Tuple<int, int, int, bool>> BuildInternalRowsForGrid(Grid grid)
        {
            var rowsByCols =
                from row in Rows
                from col in Cols
                let value = grid.ValueAt(row, col)
                select BuildInternalRowsForCell(row, col, value);

            return rowsByCols.SelectMany(cols => cols).ToImmutableList();
        }

        private static IImmutableList<Tuple<int, int, int, bool>> BuildInternalRowsForCell(int row, int col, int value)
        {
            if (value >= 1 && value <= 9)
                return ImmutableList.Create(Tuple.Create(row, col, value, true));

            return Digits.Select(v => Tuple.Create(row, col, v, false)).ToImmutableList();
        }

        private static IImmutableList<IImmutableList<int>> BuildDlxRows(
            IEnumerable<Tuple<int, int, int, bool>> internalRows)
        {
            return internalRows.Select(BuildDlxRow).ToImmutableList();
        }

        private static IImmutableList<int> BuildDlxRow(Tuple<int, int, int, bool> internalRow)
        {
            var row = internalRow.Item1;
            var col = internalRow.Item2;
            var value = internalRow.Item3;
            var box = RowColToBox(row, col);

            var posVals = Encode(row, col);
            var rowVals = Encode(row, value - 1);
            var colVals = Encode(col, value - 1);
            var boxVals = Encode(box, value - 1);

            return posVals.Concat(rowVals).Concat(colVals).Concat(boxVals).ToImmutableList();
        }

        private static int RowColToBox(int row, int col)
        {
            return row - (row % 3) + (col / 3);
        }

        private static IEnumerable<int> Encode(int major, int minor)
        {
            var result = new int[81];
            result[major * 9 + minor] = 1;
            return result.ToImmutableList();
        }

        private static bool VerifySolution(
            IReadOnlyList<Tuple<int, int, int, bool>> internalRows,
            Solution solution)
        {
            var solutionInternalRows = solution.RowIndexes
                .Select(rowIndex => internalRows[rowIndex])
                .ToImmutableList();

            var locationsGroupedByRow = Locations.GroupBy(t => t.Item1);
            var locationsGroupedByCol = Locations.GroupBy(t => t.Item2);
            var locationsGroupedByBox = Locations.GroupBy(t => RowColToBox(t.Item1, t.Item2));

            return
                CheckGroupsOfLocations(solutionInternalRows, locationsGroupedByRow, "row") &&
                CheckGroupsOfLocations(solutionInternalRows, locationsGroupedByCol, "col") &&
                CheckGroupsOfLocations(solutionInternalRows, locationsGroupedByBox, "box");
        }

        private static bool CheckGroupsOfLocations(
            IEnumerable<Tuple<int, int, int, bool>> solutionInternalRows,
            IEnumerable<IGrouping<int, Tuple<int, int>>> groupedLocations,
            string tag)
        {
            return groupedLocations.All(grouping =>
                CheckLocations(solutionInternalRows, grouping, grouping.Key, tag));
        }

        private static bool CheckLocations(
            IEnumerable<Tuple<int, int, int, bool>> solutionInternalRows,
            IEnumerable<Tuple<int, int>> locations,
            int key,
            string tag)
        {
            var digits = locations.SelectMany(location =>
                solutionInternalRows
                    .Where(solutionInternalRow =>
                        solutionInternalRow.Item1 == location.Item1 &&
                        solutionInternalRow.Item2 == location.Item2)
                    .Select(t => t.Item3));
            return CheckDigits(digits, key, tag);
        }

        private static bool CheckDigits(
            IEnumerable<int> digits,
            int key,
            string tag)
        {
            var actual = digits.OrderBy(v => v);
            if (actual.SequenceEqual(Digits)) return true;
            var values = string.Concat(actual.Select(n => Convert.ToString(n)));
            Console.WriteLine($"{tag} {key}: {values} !!!");
            return false;
        }

        private static Grid SolutionToGrid(
            IReadOnlyList<Tuple<int, int, int, bool>> internalRows,
            Solution solution)
        {
            var rowStrings = solution.RowIndexes
                .Select(rowIndex => internalRows[rowIndex])
                .OrderBy(t => t.Item1)
                .ThenBy(t => t.Item2)
                .GroupBy(t => t.Item1, t => t.Item3)
                .Select(value => string.Concat(value))
                .ToImmutableList();
            return new Grid(rowStrings);
        }

        private static void DrawSolution(
            IReadOnlyList<Tuple<int, int, int, bool>> internalRows,
            Solution solution)
        {
            SolutionToGrid(internalRows, solution).Draw();
        }
    
    /*
    private void DrawSolution(object internalRows, Solution solution)
    {
        throw new NotImplementedException();
    }

    private bool VerifySolution(object internalRows, Solution solution)
    {
        throw new NotImplementedException();
    }

    private object BuildDlxRows(object internalRows)
    {
        throw new NotImplementedException();
    }

    private object BuildInternalRowsForGrid(Grid grid)
    {
        throw new NotImplementedException();
    }

    private class Dlx
    {
        public Dlx()
        {
        }
        internal IEnumerable<Solution> Solve(object dlxRows, Func<object, object> p1, Func<object, object> p2)
        {
            throw new NotImplementedException();
        }

           internal IEnumerable<Solution> Solve(ImmutableList<ImmutableList<int>>, ImmutableList<int>, int>(ImmutableList<ImmutableList<int>>data, Func<ImmutableList<ImmutableList<int>>,
                    IEnumerable<ImmutableList<int>>>iterateRows, Func<ImmutableList<int>, IEnumerable<int>>iterateCols)
           {
               throw new NotImplementedException();
           }
    }
*/
    }
}
