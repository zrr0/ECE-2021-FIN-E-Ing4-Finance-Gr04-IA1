using Sudoku.Core;
using System;

namespace Sudoku.DL
{
    class DlxSudokuSolver
    {
        public GrilleSudoku sudoku; 

        public void Solve(GrilleSudoku trav)
        {
           Dlx.MatrixList s = new Dlx.MatrixList(ConvertToMatrix(trav.CloneSudoku()));           
            s.search();
            sudoku.setSudoku(ConvertMatrixToGrid(s.convertMatrixSudoku()));
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

        public GrilleSudoku ConvertMatrixToGrid(int[][] mat)
        {
            GrilleSudoku grille = sudoku;
            for (int i = 0; i< 9; i++)
            {
                for (int j = 0; j< 9; j++)
                {
                    grille.SetCell(i, j, mat[i][j]);
                }
            }
            return grille;

        }
        



}
}
