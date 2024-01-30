using MapGame;
using PlayerGame;
using System;


public class Program { 
    static void Main()
    {
        Initialization init = new Initialization();
        /* charger toutes les entités dans le fichier */
        List<EntityAbstract> entities = init.LoadEntityStats("C:\\Users\\polob\\OneDrive\\Bureau\\projet\\Csharp\\Project\\entity.txt");
        /* initialization de "marine" avec les stats du fichier */
        Marine marine = (Marine)entities.FirstOrDefault(entity => entity._name.ToLower() == "marine");
        /* Ré-Initialization de "marine2" avec une nouvelle instance puis le SetStats pour lui attribuer toutes ces propres stats */
        Marine marine2 = new Marine();
        marine2.SetStatsMarine(marine);

        const int mapRows = 20;
        const int mapColumns = 20;

        World world = new World();
        Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);

        while (true)
        {
            /* Clear la console ici pour pouvoir print en plus de la map */
            Console.Clear();

            Console.WriteLine("Stats marine :");
            marine.DisplayDetails();
            marine.TakeDamage(10);

            Console.WriteLine("Stats marine2 :");
            marine2.DisplayDetails();
            marine2.AddHealth(10);

            Map currentMap = world.GetMapAt(player.WorldX, player.WorldY);
            currentMap.PrintMap();
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            int newLocalX = player.LocalX;
            int newLocalY = player.LocalY;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    newLocalX--;
                    break;
                case ConsoleKey.DownArrow:
                    newLocalX++;
                    break;
                case ConsoleKey.LeftArrow:
                    newLocalY--;
                    break;
                case ConsoleKey.RightArrow:
                    newLocalY++;
                    break;
            }
            if (currentMap.CanMoveTo(newLocalX, newLocalY))
            {
                // Déplacer le joueur localement
                currentMap.MovePlayer(player.LocalX, player.LocalY, newLocalX, newLocalY);
                player.LocalX = newLocalX;
                player.LocalY = newLocalY;
            }
            else
            {
                // Sinon, vérifier si le joueur doit changer de carte
                world.MovePlayerToNewMap(player);
            }

            // Déplace le joueur localement ou vers une nouvelle carte si nécessaire
            if (newLocalX < 0 || newLocalX >= mapRows || newLocalY < 0 || newLocalY >= mapColumns)
            {
                world.MovePlayerToNewMap(player);
            }
            else if (currentMap.CanMoveTo(newLocalX, newLocalY))
            {
                currentMap.MovePlayer(player.LocalX, player.LocalY, newLocalX, newLocalY);
                player.LocalX = newLocalX;
                player.LocalY = newLocalY;
            }
            currentMap = world.GetMapAt(player.WorldX, player.WorldY);
        }
    }
}