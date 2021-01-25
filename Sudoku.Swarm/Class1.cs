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
            Console.WriteLine(s.ToString());

            var sudoku = SudokuCombinatorialEvolutionSolver.Sudoku.Difficult;

            var solver = new SudokuSolver();

            var solvedSudoku = solver.Solve(sudoku,200,5000,40);

            Console.WriteLine(solvedSudoku.ToString());

            //return (solvedSudoku.ToString());
        }
    }
}