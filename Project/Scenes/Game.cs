using MapGame;
using MapEntities;
using MenuPr;
using ShopDemo;

using Project.Quest;
using Wood;
using Project.Quest2;

namespace InGame
{
    class Game
    {
        Enemy enemy = new Enemy();
        Allies allies = new Allies();
        Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);
        World world = new World();
        Shop shop = new Shop();
        const int mapRows = 20;
        const int mapColumns = 20;
        private Map map;

        public void Start()
        {
            string title = "One Piece Game";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.ResetColor();
            RunMainMenu();
        }

        private void RunMainMenu()
        {
            string prompt = @"
                                                                        


             ██████╗ ███╗   ██╗███████╗    ██████╗ ██╗███████╗ ██████╗███████╗
            ██╔═══██╗████╗  ██║██╔════╝    ██╔══██╗██║██╔════╝██╔════╝██╔════╝
            ██║   ██║██╔██╗ ██║█████╗      ██████╔╝██║█████╗  ██║     █████╗  
            ██║   ██║██║╚██╗██║██╔══╝      ██╔═══╝ ██║██╔══╝  ██║     ██╔══╝  
            ╚██████╔╝██║ ╚████║███████╗    ██║     ██║███████╗╚██████╗███████╗
             ╚═════╝ ╚═╝  ╚═══╝╚══════╝    ╚═╝     ╚═╝╚══════╝ ╚═════╝╚══════╝
                                                                  
                                                          
                                      


            ";
            string[] options = { "Jouer", "Crédits", "Recommencer la partie", "Quitter" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    NarrateStory();
                    PlayGame();
                    break;
                case 1:
                    Credits();
                    break;
                case 2:
                    ResetGame();
                    break;
                case 3:
                    ExitGame();
                    break;
            }
        }

        private void ResetGame()
        {
            string cheminFichier = "../../../Entities/entity.json";

            try
            {
                if (File.Exists(cheminFichier))
                {
                    File.Delete(cheminFichier);
                    Console.WriteLine("La partie a été reset avec succès!");
                    Console.ReadKey(true);
                    RunMainMenu();
                }
                else
                {
                    Console.WriteLine("Le fichier JSON n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la suppression du fichier : {ex.Message}");
            }

        }

        private void DisplayArt()
        {
            string art = @"
                ███████╗████████╗ ██████╗ ██████╗ ██╗   ██╗    
                ██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗╚██╗ ██╔╝    
                ███████╗   ██║   ██║   ██║██████╔╝ ╚████╔╝     
                ╚════██║   ██║   ██║   ██║██╔══██╗  ╚██╔╝      
                ███████║   ██║   ╚██████╔╝██║  ██║   ██║       
                ╚══════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝   ╚═╝




            ";



            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(art);
            Console.ResetColor();

        }

        private void NarrateStory()
        {
            Console.Clear();
            Console.ResetColor();
            DisplayArt();

            string[] storyLines = {
                "\tSur une île mystérieuse aux confins du ", "Nouveau Monde, \n",
                "\tLuffy, Ace et Jimbey sont pris au piège, confrontés à un défi inédit.\n",
                "\tCette île, gouvernée par des ", "forces surnaturelles, \n",
                "\tcrée des effets miroirs déroutants, rendant toute évasion impossible.\n",
                "\tPour quitter cette île, nos héros doivent ", "devenir plus forts, \n",
                "\tgagner en compétences et ", "vaincre l'amiral de la Marine, \n",
                "\tle gardien de l'île.\n\n\n", "\t\tL'aventure commence au cœur d'un ",
                "village de marchands...\n"
            };

            ConsoleColor[] lineColors = {
                ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White,
                ConsoleColor.White, ConsoleColor.Magenta, ConsoleColor.White,
                ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White,
                ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.White,
                ConsoleColor.Blue
            };

            for (int i = 0; i < storyLines.Length; i++)
            {
                WriteAnimatedText(storyLines[i], lineColors[i], i % 2 == 0 ? 20 : 60);
            }

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\t\tAppuyez sur une touche pour commencer l'aventure...");
            Console.ResetColor();
            Console.ReadKey();
        }

        private void WriteAnimatedText(string text, ConsoleColor color, int delay)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(delay);
            }
            Console.ResetColor();
        }


        private void PlayGame()
        {
            Console.WriteLine("\t\tLancement du Jeu");

            Enemy enemy = new Enemy();
            Allies allies = new Allies();
            Fight fight = new Fight();
            Console.WriteLine("\t\tLancement En Cours");

            World world = new World();
            EntityContainer entities = new EntityContainer();
            Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);

            Map map = new Map(mapRows, mapColumns); 


            string path = "../../../Entities/entity.json";
            enemy.CreateEntity(path, entities);
            enemy.GetInfoEntity(path);
            allies.CreateEntity(path, entities);
            allies.GetInfoEntity(path);

            while (true)
            {
                Console.Clear();
                world.CheckForEncounter(player, allies, enemy);
                world.CheckRandEnemy(player, allies);
                Map currentMap = world.GetMapAt(player.WORLDX, player.WORLDY);
                WoodCollector woodCollector = new WoodCollector(player.WORLDX, player.WORLDY, player.LOCALX, player.LOCALY, 0, currentMap, currentMap.WoodPieces);
                woodCollector.UpdateWoodPieces(currentMap.WoodPieces);


                currentMap.PrintMap();
                world.DisplayInventoryAndTeam(player, entities);
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                int newLocalX = player.LOCALX;
                int newLocalY = player.LOCALY;

                List<string> options;
                int selectedIndex;

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
                    case ConsoleKey.T:
                        openInventory(player, entities, allies);
                        break;
                    case ConsoleKey.I:
                        openInfo(player, entities, allies);
                        break;
                    case ConsoleKey.F: // Interaction avec les PNJ (touche F)
                        if (map.QuestNPC1.IsNear(player))
                        {
                            map.QuestNPC1.Interact(woodCollector, player);
                        }

                        // Interaction avec le deuxième NPC de quête
                        if (map.QuestNPC2.IsNear(player))
                        {
                            map.QuestNPC2.Interact2(player);
                        }
                        break;
                    case ConsoleKey.E: // Interaction pour ramasser du bois (touche E)
                        if (world.IsNextToWood(player))
                        {
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("\tAppuyez sur 'E' pour ramasser le bois");
                            Console.ResetColor();

                            var key = Console.ReadKey(true);
                            if (key.Key == ConsoleKey.E)
                            {
                                // Collecter du bois via le collecteur de bois
                                woodCollector.CollectWood(currentMap, player);
                            }
                        }
                        break;
                }
            



                // Gérer le changement de carte si le joueur atteint les bords de la carte actuelle
                if (newLocalX < 0 || newLocalX >= mapRows || newLocalY < 0 || newLocalY >= mapColumns)
                {
                    world.MovePlayerToNewMap(player);
                }
                else if (currentMap.CanMoveTo(newLocalX, newLocalY))
                {
                    // Déplacer le joueur localement
                    currentMap.MovePlayer(player.LOCALX, player.LOCALY, newLocalX, newLocalY);
                    player.LOCALX = newLocalX;
                    player.LOCALY = newLocalY;
                }

                // Vérifier si le joueur est à côté d'une porte
                if (world.IsPlayerNextToDoor(player))
                {

                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\tAppuyez sur 'E' pour entrer");
                    Console.ResetColor();
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.E)
                    {
                        Shop.Run(player);
                    }
                }
                if (world.IsPlayerNextToFortressDoor(player))
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\t\t   ATTENTION : ZONE DANGEREUSE   ");

                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\t  Appuyez sur 'E' pour entrer dans la forteresse");
                    Console.ResetColor();

                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.E)
                    {
                        // Déclencher un combat ou une fonction spéciale
                        world.StartFortressBattle(player, allies);
                    }
                }
            }
        }
        private void ExitGame()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\tAppuyez sur une touche pour Quitter le Jeu");
            Console.ResetColor();
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        private void openInventory(Player player, EntityContainer entities, EntityAbstract allies)
        {
            Console.Clear();

            world.DisplayInventoryAndTeam2(player, entities, ref allies);

        }

        private void openInfo(Player player, EntityContainer entities, EntityAbstract allies)
        {
            Console.Clear();

            world.DisplayInfoAllies(entities);

        }

        private void Credits()
        {
            Console.Clear();
            string[] credits = {
                "\tQuentin LEFORESTIER",
                "\tLucie QUINTANA",
                "\tArthur BRU",
                "\tMathias REBECCA"
            };
            string art = @"

                 ██████╗██████╗ ███████╗██████╗ ██╗████████╗███████╗
                ██╔════╝██╔══██╗██╔════╝██╔══██╗██║╚══██╔══╝██╔════╝
                ██║     ██████╔╝█████╗  ██║  ██║██║   ██║   ███████╗
                ██║     ██╔══██╗██╔══╝  ██║  ██║██║   ██║   ╚════██║
                ╚██████╗██║  ██║███████╗██████╔╝██║   ██║   ███████║
                 ╚═════╝╚═╝  ╚═╝╚══════╝╚═════╝ ╚═╝   ╚═╝   ╚══════╝
                                                    



            ";



            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(art);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t\t\tGame Developed By: \n");
            Console.ResetColor();

            foreach (string line in credits)
            {
                foreach (char c in line)
                {
                    Console.Write(c);
                    System.Threading.Thread.Sleep(50); // Délai pour simuler l'effet de frappe
                }
                Console.WriteLine("\n");
                System.Threading.Thread.Sleep(300); // Délai entre les lignes
            }


            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\tAppuyez sur une touche pour retourner au Menu");
            Console.ResetColor();

            Console.ReadKey(true);
            RunMainMenu();
        }
    }
}