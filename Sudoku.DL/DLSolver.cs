using Sudoku.Core;

namespace Sudoku.DL
{
    public class DLSolver : ISudokuSolver
    {   
        Sudoku Sud;
        private DlxSudokuSolver s = new DlxSudokuSolver();

        public void Solve(GrilleSudoku grid)
        {
            Sud = new Sudoku(grid);
            s.sudoku = Sud;
            s.Solve();
            Sud = s.sudoku;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid.SetCell(i, j, Sud.getCaseSudoku(i,j));
                }
            }
        }
    }
}
