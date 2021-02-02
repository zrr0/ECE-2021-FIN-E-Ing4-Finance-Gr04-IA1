using Sudoku.Core;


namespace Sudoku.DL
{
    class DlxSudokuSolver
    {
        public GrilleSudoku sudoku; 

        public void Solve()
        {
            Dlx.MatrixList s = new Dlx.MatrixList(sudoku.getSudoku(null));
            s.search();
            sudoku.setSudoku(s.convertMatrixSudoku());
        }
    }
}
