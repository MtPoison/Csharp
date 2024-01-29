using Project.Attack;
using System;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    static void Main()
    {
        Haki attack = new Haki { name = "Haki Observation", type = AttackType.Haki, damage = 10 };

        Console.WindowWidth = 40; // Ajuster la largeur de la console
        Console.WindowHeight = 20; // Ajuster la hauteur de la console

        char[,] carte =
        {
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '#'},
            {'#', '.', '.', '#', '#', '#', '#', '#', '#', '#', '#', '#', '.', '.', '.', '.', '.', '.', '#', '#'},
            {'#', '.', '.', '#', '.', '.', '.', '.', '.', '.', '.', '#', '#', '#', '#', '.', '.', '.', '#', '#'},
            {'#', '.', '.', '#', '.', '#', '#', '#', '#', '.', '.', '.', '.', '.', '#', '.', '.', '.', '#', '#'},
            {'#', '.', '.', '#', '.', '.', '.', '.', '#', '#', '#', '#', '#', '.', '#', '.', '#', '.', '.', '#'},
            {'#', '.', '.', '#', '.', '.', '#', '.', '.', '.', '.', '.', '.', '.', '#', '.', '#', '.', '.', '#'},
            {'#', '.', '.', '#', '#', '.', '#', '.', '#', '#', '#', '#', '#', '#', '#', '.', '#', '#', '.', '#'},
            {'#', '.', '.', '.', '#', '.', '#', '.', '.', '.', '.', '.', '.', '.', '#', '.', '.', '.', '.', '#'},
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'}
        };


        /* pos du départ du joueur */
        int posX = 1;
        int posY = 1;

        /* Afficher la map au start */
        AfficherCarte(carte, posX, posY);

        static void AfficherCarte(char[,] carte, int posX, int posY)
        {
            for (int i = 0; i < carte.GetLength(0); i++)
            {
                for (int j = 0; j < carte.GetLength(1); j++)
                {
                    if (i == posY && j == posX)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("P ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(carte[i, j] + " ");
                    }
                }
                Console.WriteLine();
            }
        }


        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.Z:
                    if (posY > 1 && carte[posY - 1, posX] == '.')
                    {
                        carte[posY, posX] = '.';
                        posY--;
                    }
                    break;
                case ConsoleKey.S:
                    if (posY < carte.GetLength(0) - 2 && carte[posY + 1, posX] == '.')
                    {
                        carte[posY, posX] = '.';
                        posY++;
                    }
                    break;
                case ConsoleKey.Q:
                    if (posX > 1 && carte[posY, posX - 1] == '.')
                    {
                        carte[posY, posX] = '.';
                        posX--;
                    }
                    break;
                case ConsoleKey.D:
                    if (posX < carte.GetLength(1) - 2 && carte[posY, posX + 1] == '.')
                    {
                        carte[posY, posX] = '.';
                        posX++;
                    }
                    break;
            }

            Console.Clear();
            AfficherCarte(carte, posX, posY);
            
            attack.Execute();
            attack.damage -= 1;

        } while (keyInfo.Key != ConsoleKey.Escape);

        Console.ReadLine();



    }
}
