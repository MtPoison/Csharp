using InGame;
using MapEntities;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace MapGame
{
    public class World
    {
        public EntityContainer EntityContainer { get; private set; }

        private Map[,] worldMaps;
        private int worldSize = 3;
        private List<EnemyMap> enemyMaps = new List<EnemyMap>();
        private List<int> positionX = new List<int>();
        private bool CombatStart = false;
        Fight fight = new Fight();
        Random random = new Random();
        private bool AllieHealth = false;
        List<string> alliesNames;
        private Dictionary<(int, int, int, int), EnemyMap> enemyPositions = new Dictionary<(int, int, int, int), EnemyMap>();

        public World()
        {
            worldMaps = new Map[worldSize, worldSize];
            EntityContainer = new EntityContainer();
            InitializeWorld();
        }

        private void InitializeWorld()
        {
            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    bool isBorderMap = IsBorderMap(i, j);
                    bool isCenterMap = (i == 1 && j == 1);
                    bool isSpecialMap = (i == 0 && j == 2);
                    char[,] selectedLayout = CreateRandomLayout(isBorderMap, isCenterMap, isSpecialMap, i, j);
                    worldMaps[i, j] = new Map(20, 20);
                    worldMaps[i, j].InitializeCustomMap(selectedLayout);
                    if (!(i == 1 && j == 1) && !(i == 0 && j == 2)) 
                    {
                        PlaceEnemiesRandomly(worldMaps[i, j], i, j);
                    }
                }
            }
            //InitializeEnemy();
        }

      

        public char[,] CreateRandomLayout(bool isBorderMap, bool isCenterMap, bool isSpecialMap, int mapX, int mapY)
        {
            char[,] layout = new char[20, 20];
            Random random = new Random();

            // Initialiser tout le terrain avec de l'herbe
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    layout[i, j] = '.';

            // Générer des lacs aléatoirement pour les cartes non spéciales
            if (!isCenterMap && !isSpecialMap)
            {
                // Déterminer le nombre de lacs à créer
                int numberOfLakes = random.Next(3, 5); // Entre 3 et 5 lacs

                // Créer chaque lac
                for (int lake = 0; lake < numberOfLakes; lake++)
                {
                    // Choisir un point de départ pour le lac
                    int lakeStartX = random.Next(3, 17);
                    int lakeStartY = random.Next(3, 17);
                    // Déterminer la taille du lac
                    int lakeWidth = random.Next(5, 10);
                    int lakeHeight = random.Next(3, 9);

                    // Remplir la zone du lac avec de l'eau
                    for (int x = lakeStartX; x < lakeStartX + lakeWidth; x++)
                        for (int y = lakeStartY; y < lakeStartY + lakeHeight; y++)
                            if (x >= 0 && x < 20 && y >= 0 && y < 20)
                                layout[x, y] = '~';
                }
            }

            if (isCenterMap)
            {
                // Déterminer le nombre de maisons à créer
                int numberOfHouses = random.Next(4, 6);

                // Créer chaque maison
                for (int house = 0; house < numberOfHouses; house++)
                {
                    // Choisir une taille impaire aléatoire pour la maison
                    int houseWidth = random.Next(1, 3) * 2 + 1; // Donne un nombre impair entre 3 et 5
                    int houseHeight = random.Next(1, 3) * 2 + 1; // Donne un nombre impair entre 3 et 5

                    // Choisir un emplacement aléatoire pour la maison
                    int houseX, houseY;
                    bool spaceFound;
                    do
                    {
                        spaceFound = true;
                        houseX = random.Next(1, 20 - houseWidth);
                        houseY = random.Next(1, 20 - houseHeight);

                        // Vérifier si l'espace est libre et sans maisons adjacentes
                        for (int x = houseX - 1; x <= houseX + houseWidth && spaceFound; x++)
                            for (int y = houseY - 1; y <= houseY + houseHeight; y++)
                                if (x >= 0 && x < 20 && y >= 0 && y < 20 && layout[x, y] != '.')
                                    spaceFound = false;

                    } while (!spaceFound);

                    // Construire la maison
                    for (int x = houseX; x < houseX + houseWidth; x++)
                        for (int y = houseY; y < houseY + houseHeight; y++)
                            layout[x, y] = 'H'; // H représente un bâtiment

                    // Placer une porte au milieu d'un des côtés de la maison
                    switch (random.Next(4))
                    {
                        case 0: // Porte en droite
                            layout[houseX + houseWidth / 2, houseY] = ']';
                            break;
                        case 1: // Porte en gauche
                            layout[houseX + houseWidth / 2, houseY + houseHeight - 1] = '[';
                            break;
                        case 2: // Porte à haut
                            layout[houseX, houseY + houseHeight / 2] = '―';
                            break;
                        case 3: // Porte à bas
                            layout[houseX + houseWidth - 1, houseY + houseHeight / 2] = '―';
                            break;
                    }
                }
            }

            if (isSpecialMap)
            {
                CreateFortress(layout);
            }

            // Gérer les cartes de bord
            if (isBorderMap)
            {
                // Appliquer l'eau et le sable sur les côtés extérieurs
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        // Placer l'eau de façon aléatoire sur les bords
                        if (mapX == 0 && i < random.Next(1, 5)) layout[i, j] = '~';
                        if (mapX == worldSize - 1 && i > 15 + random.Next(0, 4)) layout[i, j] = '~';
                        if (mapY == 0 && j < random.Next(1, 5)) layout[i, j] = '~';
                        if (mapY == worldSize - 1 && j > 15 + random.Next(0, 4)) layout[i, j] = '~';
                    }
                }

                // Ajouter du sable pour la transition
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (layout[i, j] == '.' && AdjacentToWater(layout, i, j))
                        {
                            layout[i, j] = 'S'; // 'S' pour sable
                        }
                    }
                }
            }

            return layout;
        }
        private void CreateFortress(char[,] layout)
        {
            int fortressWidth = 10;
            int fortressHeight = 10;
            int startX = (20 - fortressWidth) / 2;
            int startY = (20 - fortressHeight) / 2;

            // Construire les murs de la forteresse
            for (int x = startX; x < startX + fortressWidth; x++)
            {
                for (int y = startY; y < startY + fortressHeight; y++)
                {
                    if (x == startX || x == startX + fortressWidth - 1 || y == startY || y == startY + fortressHeight - 1)
                    {
                        layout[x, y] = 'F'; // 'F' pour les murs de la forteresse
                    }
                    else
                    {
                        layout[x, y] = ' '; // Espace ou autre caractère pour l'intérieur de la forteresse
                    }
                }
            }

            // Ajouter une porte à la forteresse
            layout[startX + fortressWidth / 2, startY] = 'D'; // 'D' pour porte

            // Placer des ennemis à l'intérieur de la forteresse
            PlaceEnemiesInFortress(startX, startY, fortressWidth, fortressHeight, layout);
        }

        private void PlaceEnemiesInFortress(int startX, int startY, int width, int height, char[,] layout)
        {
            for (int i = 0; i < 5; i++) // Nombre d'ennemis
            {
                int x, y;
                do
                {
                    x = random.Next(startX + 1, startX + width - 1);
                    y = random.Next(startY + 1, startY + height - 1);
                } while (layout[x, y] != ' '); // Modifier pour correspondre à l'espace vide

                EnemyMap newEnemyMap = new EnemyMap(0, 2, x, y, "");
                enemyMaps.Add(newEnemyMap);
            }
        }


        private bool AdjacentToWater(char[,] layout, int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int checkX = x + i;
                    int checkY = y + j;

                    if (checkX >= 0 && checkX < 20 && checkY >= 0 && checkY < 20)
                    {
                        if (layout[checkX, checkY] == '~') return true;
                    }
                }
            }
            return false;
        }

        public Map? GetMapAt(int x, int y)
        {
            if (x >= 0 && x < worldSize && y >= 0 && y < worldSize)
            {
                return worldMaps[x, y];
            }
            else
            {
                return null;
            }
        }
        private bool IsBorderMap(int x, int y)
        {
            // Une carte est sur le bord si elle est sur la première ou la dernière ligne/colonne
            return x == 0 || y == 0 || x == worldSize - 1 || y == worldSize - 1;
        }


        public void MovePlayerToNewMap(Player player)
        {
            // Obtenir la carte actuelle
            Map? currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            if (currentMap == null) return;

            int mapHeight = 18;
            int mapWidth = 18;

            // Stocker les anciennes coordonnées locales
            int oldLocalX = player.LOCALX;
            int oldLocalY = player.LOCALY;

            bool needsNewMap = false;

            // Déterminer si le joueur atteint les bords de la carte
            if (player.LOCALX < 1)
            {
                player.WORLDX--;
                player.LOCALX = mapWidth;
                needsNewMap = true;
            }
            else if (player.LOCALX > mapHeight)
            {
                player.WORLDX++;
                player.LOCALX = 0;
                needsNewMap = true;
            }
            else if (player.LOCALY < 1)
            {
                player.WORLDY--;
                player.LOCALY = mapHeight;
                needsNewMap = true;
            }
            else if (player.LOCALY > mapWidth)
            {
                player.WORLDY++;
                player.LOCALY = 0;
                needsNewMap = true;
            }

            // Vérifier les limites du monde
            if (player.WORLDX < 0 || player.WORLDX >= worldSize || player.WORLDY < 0 || player.WORLDY >= worldSize)
            {
                needsNewMap = false;
                player.WORLDX = Math.Max(0, Math.Min(player.WORLDX, worldSize - 1));
                player.WORLDY = Math.Max(0, Math.Min(player.WORLDY, worldSize - 1));
            }
            if (needsNewMap)
            {
                // Effacer la position du joueur sur l'ancienne carte
                currentMap.ClearPlayerPosition(oldLocalX, oldLocalY);

                // Placer le joueur sur la nouvelle carte
                Map? newMap = GetMapAt(player.WORLDX, player.WORLDY);
                if (newMap != null)
                {
                    // Si la position initiale sur la nouvelle carte est de l'eau, chercher une position d'herbe
                    if (newMap.IsWater(player.LOCALX, player.LOCALY))
                    {
                        PlacePlayerOnNearestGrass(newMap, player);
                    }
                    else
                    {
                        // Si la position initiale est praticable, placer le joueur là
                        newMap.PlacePlayer(player.LOCALX, player.LOCALY);
                    }
                }
            }
        }

        private void PlacePlayerOnNearestGrass(Map map, Player player)
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (map.IsGrass(x, y))
                    {
                        map.PlacePlayer(x, y);
                        player.LOCALX = x;
                        player.LOCALY = y;
                        return;
                    }
                }
            }
        }
        public bool IsPlayerNextToDoor(Player player)
        {
            Map currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            return currentMap != null && currentMap.IsNextToDoor(player.LOCALX, player.LOCALY);
        }

        public bool IsPlayerNextToFortressDoor(Player player)
        {
            Map currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            return currentMap != null && currentMap.IsNextToFortressDoor(player.LOCALX, player.LOCALY);
        }

        public void CheckHealthStartFight(EntityContainer entities) 
        { 
            for(int i = 0; i < entities.AlliesList.Count() ;i++) 
            {
                if (entities.AlliesList[i]._health > 0)
                {
                    AllieHealth = true;
                }
                else
                {
                    AllieHealth = true;
                }
            }
        }


        public void CheckForEncounter(Player player, Allies allies, Enemy enemy)
        {
            if (enemyMaps.Count != 0)
            {
                foreach (var enemyMap in enemyMaps)
                {
                    // Vérifiez si l'ennemi est sur le même axe X et dans le même monde que le joueur
                    if (enemyMap.LOCALX == player.LOCALX && enemyMap.WORLDX == player.WORLDX && enemyMap.WORLDY == player.WORLDY && !enemyMap.COMBATSTART && AllieHealth)
                    {
                        // Déclenche un combat
                        HandleEncounter(allies, enemy, player, 1);
                        enemyMap.COMBATSTART = true;
                    }
                }
            }
        }


        private void HandleEncounter(Allies allies, Enemy enemy, Player p, int combatType)
        {
            // Combat entre le joueur et l'ennemi
            fight.startCombat(allies.entitiesContainer, false, p, combatType);
        }


        /*private void InitializeEnemy()
        {
            // Générer un nombre aléatoire entre 0 et 99
            int randChance = random.Next(100);
            int chanceSpawnEnemy = 70; // Par exemple, 70% de chance de faire apparaître un ennemi lors de l'initialisation

            // Vérifier si le nombre aléatoire est inférieur au seuil de pourcentage
            if (randChance < chanceSpawnEnemy)
            {
                int enemyLocalX = random.Next(1, 19);
                int enemyLocalY = random.Next(1, 19);

                EnemyMap newEnemyMap = new EnemyMap(1, 1, enemyLocalX, enemyLocalY);
                enemyMaps.Add(newEnemyMap);

                Map centerMap = worldMaps[1, 1];
                centerMap.PlaceEnemy(newEnemyMap.LOCALX, newEnemyMap.LOCALY);
            }
        }*/

        private void PlaceEnemiesRandomly(Map map, int worldX, int worldY)
        {
            int chanceSpawnEnemy = 75; // Pourcentage de chance de placer un ennemi
            string[] aiLevels = { "cyan", "darkMagenta", "darkRed" }; // Différents niveaux d'IA

            for (int i = 0; i < 3; i++) // Nombre d'ennemis à placer
            {
                int randChance = random.Next(100);
                if (randChance < chanceSpawnEnemy)
                {
                    int localX, localY;
                    do
                    {
                        localX = random.Next(20);
                        localY = random.Next(20);
                    } while (map.matrix[localX, localY] != '.'); // S'assurer que la position est sur l'herbe

                    string aiLevel = aiLevels[random.Next(aiLevels.Length)]; // Sélection aléatoire du niveau d'IA
                    map.PlaceEnemy(worldX, worldY, localX, localY, aiLevel); // Modifier pour utiliser les coordonnées correctes et le niveau d'IA
                }
            }
        }




        public void CheckRandEnemy(Player player, Allies allies)
        {
            int randEnemy = random.Next(1, 19);
            if (randEnemy == player.LOCALX && AllieHealth && !(player.WORLDX == 1 && player.WORLDY == 1))
            {
                int randChance = random.Next(100);
                int chanceStartCombat1 = 50;  // Par exemple, 50% de chance pour le premier type de combat

                if (randChance < chanceStartCombat1)
                {
                   fight.startCombat(allies.entitiesContainer, true, player, 1); // Passer en paramètre le type de combat
                }
            }
        }

        public void StartFortressBattle(Player player, World world)
        {
            // Combat à la forteresse
        }

        public List<EnemyMap> GetEnemyMaps()
        {
            return enemyMaps;
        }

        public void DisplayInventoryAndTeam(Player player, EntityContainer entityContainer)
        {
            int inventoryX = 43;
            int inventoryY = 2;

            // Créez un cadre pour l'inventaire
            DrawBox(inventoryX - 2, inventoryY - 1, 30, 15);

            // Titre de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t     Inventaire");
            Console.ForegroundColor = ConsoleColor.Gray;

            // Affiche les détails de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine($"  Viande : {player.NBViande}");
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine($"  Alcool : {player.NBAlcool}");
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine($"  Or      : {player.NBGold}");

            // Séparation visuelle
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine(new string('-', 30));

            if (entityContainer.AlliesList != null)
            {
                // Affiche l'équipe des alliés
                Console.SetCursorPosition(inventoryX, inventoryY++);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\tEquipe");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(inventoryX, inventoryY++);

                for (int i = 0; i < entityContainer.AlliesList.Count; i++)
                {
                    var ally = entityContainer.AlliesList[i];
                    if (ally != null)
                    {
                        UpdateInfoAllies(entityContainer, "../../../Entities/entity.json");
                        Console.SetCursorPosition(inventoryX, inventoryY);

                        // Nom du personnage en couleur différente
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(ally._name);
                        Console.ResetColor();

                        // HP en couleur différente
                        Console.Write(" - HP:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(ally._health);
                        Console.ResetColor();

                        // Stamina en couleur différente
                        Console.Write(" - Stamina:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(ally._stamina);
                        Console.ResetColor();

                        inventoryY++;
                    }
                    else
                    {
                        Console.SetCursorPosition(inventoryX, inventoryY++);
                        Console.WriteLine("Allié non initialisé");
                    }
                }
            }
        }

        public void DisplayInventoryAndTeam2(Player player, EntityContainer entityContainer, ref EntityAbstract allie)
        {
            List<string> options;
            int selectedIndex;

            options = new List<string> { $"Viande : {player.NBViande}", $"Alcool : {player.NBAlcool}", $"Or : {player.NBGold}", "Fermer" };
            selectedIndex = RunOptionsInventory(options, allie);

            switch (selectedIndex)
            {
                case 0:
                    if (player.NBViande > 0)
                    {
                        alliesNames = entityContainer.AlliesList
                            .Select(a => a._name)
                            .ToList();

                        EntityAbstract newAllie = null;
                        do
                        {
                            selectedIndex = RunOptionsInventory(alliesNames, allie);
                            string selectedName = alliesNames[selectedIndex];
                            newAllie = entityContainer.AlliesList.FirstOrDefault(a => a._name == selectedName);
                        } while (newAllie == null);

                        allie = newAllie;
                        allie.AddHealth(20);
                        player.RemoveViande(1);
                        AddAllyJson(allie, "../../../Entities/entity.json", 20, "Health");
                    }
                    break;
                case 1:
                    if (player.NBAlcool > 0)
                    {
                        alliesNames = entityContainer.AlliesList
                            .Select(a => a._name)
                            .ToList();

                        EntityAbstract newAllie = null;
                        do
                        {
                            selectedIndex = RunOptionsInventory(alliesNames, allie);
                            string selectedName = alliesNames[selectedIndex];
                            newAllie = entityContainer.AlliesList.FirstOrDefault(a => a._name == selectedName);
                        } while (newAllie == null);

                        allie = newAllie;
                        allie.AddStamina(20);
                        player.RemoveAlcool(1);
                        AddAllyJson(allie, "../../../Entities/entity.json", 20, "Stamina");
                    }
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        public void DisplayInfoAllies(EntityContainer entityContainer)
        {
            List<string> options;
            int selectedIndex;
            
            options = new List<string> { "Fermer" };
            selectedIndex = RunOptionsInfo(options, entityContainer);

            switch (selectedIndex)
            {
                case 0:
                    break;
                default:
                    break;
            }
        }

        private void AddAllyJson(EntityAbstract entity, string path, int countType, string type)
        {
            EntityContainer entities;

            // Utilisez un bloc using pour libérer la ressource après la lecture du fichier
            using (StreamReader reader = File.OpenText(path))
            {
                string json = reader.ReadToEnd();
                entities = JsonConvert.DeserializeObject<EntityContainer>(json);
            }

            if (entity is Allies)
            {
                var targetAllies = entities.AlliesList.FirstOrDefault(a => a._name.Equals(entity._name, StringComparison.OrdinalIgnoreCase));
                if (targetAllies != null)
                {
                    if (type == "Health")
                    {
                        targetAllies._health += countType;
                    }
                    else if (type == "Stamina")
                    {
                        targetAllies._stamina += countType;
                    }
                }
            }

            // Utilisez à nouveau un bloc using pour libérer la ressource après l'écriture dans le fichier
            using (StreamWriter writer = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, entities);
            }
        }


        public int RunOptionsInventory(List<string> options, EntityAbstract allie)
        {
            ConsoleKey keyPressed;
            int selectedIndex = 0;
            do
            {
                DisplayOptionsInventory(options, selectedIndex, allie);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : options.Count - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex < options.Count - 1) ? selectedIndex + 1 : 0;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }

        public int RunOptionsInfo(List<string> options, EntityContainer entities)
        {
            ConsoleKey keyPressed;
            int selectedIndex = 0;
            do
            {
                DisplayOptionsInfo(options, selectedIndex, entities);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }

        private void DisplayOptionsInventory(List<string> options, int selectedIndex, EntityAbstract allie)
        {
            Console.Clear();
            int inventoryX = 10;
            int inventoryY = 10;

            // Créez un cadre pour l'inventaire
            DrawBox(inventoryX - 2, inventoryY - 1, 30, 15);

            // Titre de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t     Inventaire\n");
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($"\t\t* {options[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"\t\t  {options[i]}");
                }
            }
            Console.ResetColor();
        }

        private void DisplayOptionsInfo(List<string> options, int selectedIndex, EntityContainer entities)
        {
            Console.Clear();
            entities = JsonConvert.DeserializeObject<EntityContainer>(File.ReadAllText("../../../Entities/entity.json"));
            int inventoryX = 10;
            int inventoryY = 10;

            // Titre de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t     Informations\n");

            Console.WriteLine("Persoonage :\n");
            for(int i = 0; i < entities.AlliesList.Count(); i++)
            {
                Console.WriteLine($"{i+1}. {entities.AlliesList[i]._name} - Level : {entities.AlliesList[i]._level}");
                for (int j = 0; j < entities.AlliesList[i]._ListCapacities.Count(); j++)
                {
                    if (entities.AlliesList[i]._level >= entities.AlliesList[i]._ListCapacities[j]._level)
                    {
                        Console.WriteLine($"Attaque débloqué : {entities.AlliesList[i]._ListCapacities[j]._name}");
                    }
                    else if (entities.AlliesList[i]._level < entities.AlliesList[i]._ListCapacities[j]._level)
                    {
                        Console.WriteLine($"Attaque à débloqué : {entities.AlliesList[i]._ListCapacities[j]._name} - Level requis : {entities.AlliesList[i]._ListCapacities[j]._level}");
                    }
                }
                Console.WriteLine("\n");
            }


            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($"\t\t* {options[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"\t\t  {options[i]}");
                }
            }


            Console.ResetColor();
        }

        public void UpdateInfoAllies(EntityContainer entityContainer, string path)
        {
            // Utilisez un bloc using pour libérer la ressource après la lecture du fichier
            using (StreamReader reader = File.OpenText(path))
            {
                string json = reader.ReadToEnd();
                var entities = JsonConvert.DeserializeObject<EntityContainer>(json);

                foreach (var ally in entities.AlliesList)
                {
                    var targetAlly = entityContainer.AlliesList.FirstOrDefault(a => a._name.Equals(ally._name, StringComparison.OrdinalIgnoreCase));
                    if (targetAlly != null)
                    {
                        targetAlly._health = ally._health;
                        targetAlly._stamina = ally._stamina;
                    }
                }
            }
        }

        // Méthode pour dessiner un cadre autour de l'inventaire
        private void DrawBox(int x, int y, int width, int height)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string horizontalLine = "+" + new string('-', width + 2) + "+";

            // Dessine la ligne supérieure
            Console.SetCursorPosition(x, y);
            Console.Write(horizontalLine);

            // Dessine les côtés
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("|");
                Console.SetCursorPosition(x + width + 3, y + i);
                Console.Write("|");
            }

            // Dessine la ligne inférieure
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write(horizontalLine);

            Console.ResetColor();
        }
    }
}