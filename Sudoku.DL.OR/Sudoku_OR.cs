using Sudoku.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.ConstraintSolver;

namespace Sudoku.DL.OR
{
    public class Sudoku_OR : ISudokuSolver
    {

        IEnumerable<int> cellIndices = Enumerable.Range(0, 9);

        public void Solve(GrilleSudoku s)
        {
            
            //Création d'un solver Or-tools

            Solver solver = new Solver("Sudoku");
            
            //Création de la grille de variables

            IntVar[,] grid = solver.MakeIntVarMatrix(9, 9, 1, 9, "grid");
            IntVar[] grid_flat = grid.Flatten();

            
            
            
            //Masque de résolution
            foreach (int i in cellIndices)
            {
                foreach (int j in cellIndices)
                {
                    if (s.GetCellule(i, j) > 0)
                    {
                        solver.Add(grid[i, j] == s.GetCellule(i, j));
                    }
                }
            }

            //Un chiffre ne figure qu'une seule fois par ligne/colonne/cellule
            foreach (int i in cellIndices)
            {
                // Lignes
                solver.Add((from j in cellIndices
                    select grid[i, j]).ToArray().AllDifferent());

                // Colonnes





             
            }

            //Cellules
           









            //Début de la résolution
            DecisionBuilder db = solver.MakePhase(grid_flat,
                Solver.INT_VAR_SIMPLE,
                Solver.INT_VALUE_SIMPLE);
            solver.NewSearch(db);

            //Mise à jour du sudoku










        }
    }
}
