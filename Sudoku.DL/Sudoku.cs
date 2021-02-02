using Sudoku.Core;
using System;
using System.Collections.Generic;


namespace Sudoku.DL
{
    class Sudoku
    {
        private int[][] initialSudoku;  //Sudoku original et vide (ne peut être modifié)
        private int[][] workingSudoku;  //Sudoku sur lequel vous allez travailler 


        /*--------------------Constructeur--------------------*/
        public Sudoku(GrilleSudoku grid)  //Constructeur
        {
            initialSudoku = new int[9][];
            workingSudoku = new int[9][];
            string phrase = "";
            for (int i = 0; i < 9; i++)
            {
                initialSudoku[i] = new int[9];
                workingSudoku[i] = new int[9];
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    phrase = phrase + grid.GetCellule(i, j);
                }
            }

            initialSudoku = stringToSudoku(phrase);
            workingSudoku = stringToSudoku(phrase);
        }

        /*--------------------Getter & Setter--------------------*/

        public int[][] getSudoku(int[][] sudoku)  //récupèrele sudoku de "travail"
        {
            sudoku = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                sudoku[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    sudoku[i][j] = workingSudoku[i][j];
                }
            }
            return sudoku;
        }
        public int getCaseSudoku(int line, int column)  //récupère une case du sudoku de "travail"
        {
            return workingSudoku[line][column];
        }

        public bool setSudoku(int[][] sudoku)  //Attribue un nouveau sudoku de "travail" 
        {
            if (!checkSudoku(sudoku, "setSudoku"))  //Renvoie false si ce n'est pas autorisé
                return false;
            if (!checkAllCase(sudoku, "setSudoku"))  //Renvoie false si ce n'est pas autorisé
                return false;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    workingSudoku[i][j] = sudoku[i][j];
                }
            }
            return true;
        }


        public bool setCaseSudoku(int line, int column, int value)  //Attribue une nouvelle case au sudoku de "travail"
        {
            if (!checkCase(line, column, value, "setCaseSudoku"))  //Renvoie false si ce n'est pas autorisé
                return false;

            workingSudoku[line][column] = value;

            return true;
        }
 

        /*--------------------Outils--------------------*/
        public int[][] stringToSudoku(String stringSudoku)  //Transforme un String en sudoku (tableau de int[9][9])
        {
            if (stringSudoku.Length != 81)
            {
                Console.WriteLine("        !!! ERROR !!! : Ce String a une taille non conforme pour étre un sudoku");
                return stringToSudoku(stringSudoku);
            }
            int[][] sudoku;
            sudoku = new int[9][];

            for (int i = 0; i < 9; i++)
            {
                sudoku[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    char car = stringSudoku[i * 9 + j];
                    if (car == '.')
                        sudoku[i][j] = 0;
                    else
                    {
                        int number = car - 48; // - 48 pour la conversion de la table ascii
                        if (number < 0 || number > 9)
                        {
                            Console.WriteLine("        !!! ERROR !!! : Caractère non valide pour un sudoku");
                            return stringToSudoku(stringSudoku);
                        }
                        else
                            sudoku[i][j] = number;
                    }
                }
            }
            return sudoku;
        }

        public bool checkSudoku(int[][] sudoku, String log)  //Vérifie la validité d'un sudoku (taille 9x9)
        {
            if (sudoku.Length != 9)
            {
                Console.WriteLine("        !!! WARNING !!! : Nombre de lignes incorrect (" + sudoku.Length + ", au lieu de 9) lors de la commande " + log);
                return false;
            }
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[i].Length != 9)
                {
                    Console.WriteLine("        !!! WARNING !!! : Nombre de colonnes incorrect à la ligne " + i + " (" + sudoku.Length + ", au lieu de 9) lors de la commande " + log);
                    return false;
                }
            }
            return true;
        }

        public bool checkAllCase(int[][] sudoku, String log)  //Vérifie la validité de toutesles cases du sudoku (valeur comprise entre 0 et 9 et conforme au sudoku initial)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!checkCase(i, j, sudoku[i][j], log))
                        return false;
                }
            }
            return true;
        }

        public bool checkCase(int line, int column, int value, String log)  //Vérifie la validité d'une case de sudoku (valeur comprise entre 0 et 9 et conforme au sudoku initial)
        {
            if (initialSudoku[line][column] != 0 && value != initialSudoku[line][column])
            {
                Console.WriteLine("        !!! WARNING !!! : Cette case ne peut être modifiée, c'est une case fixée par le sudoku. (case [" + line + "][" + column + "]) lors de la commande " + log);
                return false;
            }

            if (value < 0 || value > 9)
            {
                Console.WriteLine("        !!! WARNING !!! : Valeur non valable à la case [" + line + "][" + column + "] (" + value + ", au lieu de [0,1,2,3,4,5,6,7,8,9]) lors de la commande " + log);
                return false;
            }
            return true;
        }       
    }
}
