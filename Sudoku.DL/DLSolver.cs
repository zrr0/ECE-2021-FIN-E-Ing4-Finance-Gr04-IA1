using Sudoku.Core;
using System;


namespace Sudoku.DL
{
    public class DLSolver : ISudokuSolver
    {   
        Sudoku Sud;
        private DlxSudokuSolver s = new DlxSudokuSolver();

        public void Solve(GrilleSudoku grid)
        {
            GrilleSudoku GrilleSudokuInitial = grid;
            //Sud = new Sudoku(grid); //creation d'un nouveau sudoku à partir du sudoku que l'on recoit
            s.sudoku = grid; // affecte 
            s.Solve();
            grid = s.sudoku;

            /*
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid.SetCell(i, j, Sud.getCaseSudoku(i,j));
                }
            }*/
        }

    }
}
