using Sudoku.Core;
using System;
namespace Sudoku.DL

{
    class DlxSudokuSolver
    {

        public void Solve(GrilleSudoku trav)
        {
           Dlx.MatrixList s = new Dlx.MatrixList(ConvertToMatrix(trav));           
            s.search();
            trav.setSudoku(s.convertMatrixSudoku());
        }

         public int[][] ConvertToMatrix(GrilleSudoku grille)
        {
            int[][] sud = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                sud[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    sud[i][j] = grille.GetCellule(i, j);
                }
            }
            return sud;
        }
    }
}
