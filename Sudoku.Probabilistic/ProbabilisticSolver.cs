using Sudoku.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Collections;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Math;
using Microsoft.ML.Probabilistic.Models;

namespace Sudoku.Probabilistic
{

    public class ProbabilisticSolver : ISudokuSolver
    {
       
        public void Solve(GrilleSudoku s)
        {
            var model = new NaiveSudokuModel();
            model.SolveSudoku(s);

        }

    }

    /// <summary>
    /// Ce premier modèle est très faible: d'une part, il ne résout que quelques Sudokus faciles, d'autre part, le modèle est recompilé à chaque fois, ce qui prend beaucoup de temps
    /// </summary>
    public class NaiveSudokuModel
    {

        private static List<int> CellDomain = Enumerable.Range(1, 9).ToList();
        private static List<int> CellIndices = Enumerable.Range(0, 81).ToList();


        public virtual void SolveSudoku(GrilleSudoku s)
        {

            var algo = new ExpectationPropagation();
            var engine = new InferenceEngine(algo);

            //Implémentation naïve: une variable aléatoire entière par cellule
            var cells = new List<Variable<int>>(CellIndices.Count);

            foreach (var cellIndex in GrilleSudoku.IndicesCellules)
            {
                //On initialise le vecteur de probabilités de façon uniforme pour les chiffres de 1 à 9
                var baseProbas = Enumerable.Repeat(1.0, CellDomain.Count).ToList();
                //Création et ajout de la variable aléatoire
                var cell = Variable.Discrete(baseProbas.ToArray());
                cells.Add(cell);

            }

            //Ajout des contraintes de Sudoku (all diff pour tous les voisinages)
            foreach (var cellIndex in GrilleSudoku.IndicesCellules)
            {
                foreach (var neighbourCellIndex in GrilleSudoku.VoisinagesParCellule[cellIndex])
                {
                    if (neighbourCellIndex > cellIndex)
                    {
                        Variable.ConstrainFalse(cells[cellIndex] == cells[neighbourCellIndex]);
                    }
                }
            }

            //On affecte les valeurs fournies par le masque à résoudre comme variables observées
            foreach (var cellIndex in GrilleSudoku.IndicesCellules)
            {
                if (s.Cellules[cellIndex] > 0)
                {
                    cells[cellIndex].ObservedValue = s.Cellules[cellIndex] - 1;
                }
            }
            
            foreach (var cellIndex in GrilleSudoku.IndicesCellules)
            {
                if (s.Cellules[cellIndex]==0)
                {
                    var result = (Discrete)engine.Infer(cells[cellIndex]);
                    s.Cellules[cellIndex] = result.Point + 1;
                }
            }

        }



    }

}
