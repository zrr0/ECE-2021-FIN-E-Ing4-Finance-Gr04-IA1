using System;
using Sudoku.Core;


namespace Sudoku.Swarm
{
    public class SwarmSolver : ISudokuSolver
    {
        public  void Solve(GrilleSudoku s)
        {
            //conversion de formats en int[,]
            int[,] converted = new int[9,9];
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    converted[row,column] = s.Cellules[row * 9 + column];
                }
            }
            var sudoku = Sudoku.New(converted);
            
           var solver = new SudokuSolver();
           
            var solvedSudoku = solver.Solve(sudoku, 1,1,0);
            // Possibilité d'incrémenter le nombre d'organismes jusqu'à obtenir une solution?
            // Mais les meilleures optimisations à faire sont probablement dans le code du solver lui-même, auquel cas, il faudrait copier le code dans le projet plutôt que le référencer

            
            double numOrganisms = 20;
            double epochs = 2000;
            var max_retry = 10;
            int cnt = 0;

            do
            {
                solvedSudoku = solver.Solve(sudoku, (int)numOrganisms, (int)epochs, max_retry);
                if (cnt == 0)
                {
                    numOrganisms = 200;
                    // Console.WriteLine ($"nb orga =: {numOrganisms}");
                    max_retry = 300;
                }
                else if (cnt >= 0)
                {
                    //  Console.WriteLine($"nb orga =: {numOrganisms}");

                    numOrganisms *= 1.5;
                    if (max_retry < 300)
                    {
                        max_retry += 50;
                    }
                }
                cnt++;
            } while (solvedSudoku.Error > 0);

            //affichage du sudoku
            Console.WriteLine(solvedSudoku.ToString());
            //Reconversion depuis int[,]
            //Console.WriteLine($"nb restars=: {max_retry}");
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    s.Cellules[row * 9 + column] = solvedSudoku.CellValues[row, column];
                }
            }


        }
    }
}