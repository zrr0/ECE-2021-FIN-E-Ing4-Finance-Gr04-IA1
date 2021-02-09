using Sudoku.Core;

namespace Sudoku.DL
{
    public class DLSolver : ISudokuSolver
    {
       
        private static DlxSudokuSolver s = new DlxSudokuSolver();

        public void Solve(GrilleSudoku grid)
        {
            s.Solve(grid);
        }

    }
}
