using Sudoku.Core;

namespace Sudoku.DL
{
    public class DLSolver : ISudokuSolver
    {   
        
        private DlxSudokuSolver s = new DlxSudokuSolver();

        public void Solve(GrilleSudoku grid)
        {
            s.sudoku = grid; // affecte 
            s.Solve(grid);
            grid = s.sudoku;           
        }

    }
}
