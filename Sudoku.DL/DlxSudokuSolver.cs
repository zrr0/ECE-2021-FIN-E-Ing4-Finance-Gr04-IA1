using Sudoku.Core;


namespace Sudoku.DL
{
    class DlxSudokuSolver
    {
        public Sudoku sudoku; //= new Sudoku();

        public void Solve()
        {
            Dlx.MatrixList s = new Dlx.MatrixList(sudoku.getSudoku(null));
            s.search();
            sudoku.setSudoku(s.convertMatrixSudoku());
        }
    }
}
