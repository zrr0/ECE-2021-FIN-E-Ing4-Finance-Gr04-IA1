using System;
using System.Threading;
using SudokuCombinatorialEvolutionSolver;
using System.Linq;

namespace Sudoku.Swarm
{
    public class Solver : Core.ISudokuSolver
    {
        public void Solve(Core.GrilleSudoku s)
        {
            int[,] converted = new int[9,9];
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    converted[row,column] = s.Cellules[row * 9 + column];
                }
            }
            var sudoku = SudokuCombinatorialEvolutionSolver.Sudoku.New(converted);

            var solver = new SudokuSolver();

            var solvedSudoku = solver.Solve(sudoku,200,5000,40);

            Console.WriteLine(solvedSudoku.ToString());

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    s.Cellules[row * 9 + column] = solvedSudoku.CellValues[row, column];
                }
            }


        }
    }
}