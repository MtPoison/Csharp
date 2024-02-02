﻿using MapGame;
using MapEntities;

namespace MapGame
{
    public class World
    {
        private Map[,] worldMaps;
        private int worldSize = 3;
        private EnemyMap enemymap;

        public World()
        {
            worldMaps = new Map[worldSize, worldSize];
            InitializeWorld();
        }

        private void InitializeWorld()
        {
            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    bool isCenterMap = (i == 1 && j == 1); // Carte centrale avec le village
                    bool isSpecialMap = (i == 0 && j == 2); // Carte spéciale en haut à droite
                    char[,] selectedLayout = CreateRandomLayout(isCenterMap, isSpecialMap);
                    worldMaps[i, j] = new Map(20, 20);
                    worldMaps[i, j].InitializeCustomMap(selectedLayout);
                    if (!(i == 1 && j == 1) && !(i == 0 && j == 2)) // Si ce n'est pas le village ni la zone spéciale
                    {
                        PlaceEnemiesRandomly(worldMaps[i, j]); // Placer des ennemis aléatoirement
                    }
                }
            }
            InitializeEnemy();
        }

        private char[,] CreateRandomLayout(bool isCenterMap, bool isSpecialMap = false)
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
                // Créer un village au centre
                for (int x = 10; x <= 16; x++)
                    for (int y = 12; y <= 18; y++)
                        layout[x, y] = 'H'; // H représente un bâtiment

                layout[13, 18] = layout[13, 12] = ']'; // Portes sur les côtés gauche et droit
            }

            if (isSpecialMap)
            {
                // Créer une forteresse ou tout autre structure spéciale
            }

            // S'assurer que les bords sont praticables
            for (int i = 0; i < 20; i++)
            {
                layout[0, i] = layout[19, i] = '.';
                layout[i, 0] = layout[i, 19] = '.';
            }

            return layout;
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

        public void MovePlayerToNewMap(Player player)
        {
            Map? currentMap = GetMapAt(player.WorldX, player.WorldY);
            if (currentMap == null) return;

            int oldLocalX = player.LocalX;
            int oldLocalY = player.LocalY;

            int oldWorldX = player.WorldX;
            int oldWorldY = player.WorldY;

            int mapHeight = 18;
            int mapWidth = 18;

            bool needsNewMap = false;

            // Déterminer la nouvelle position mondiale du joueur
            if (player.LocalX < 1)
            {
                player.WorldX--;
                player.LocalX = mapWidth;
                needsNewMap = true;
            }
            else if (player.LocalX > mapHeight)
            {
                player.WorldX++;
                player.LocalX = 0;
                needsNewMap = true;
            }
            else if (player.LocalY < 1)
            {
                player.WorldY--;
                player.LocalY = mapHeight;
                needsNewMap = true;
            }
            else if (player.LocalY > mapWidth)
            {
                player.WorldY++;
                player.LocalY = 0;
                needsNewMap = true;
            }

            // Vérifier les limites du monde
            if (player.WorldX < 0 || player.WorldX >= worldSize || player.WorldY < 0 || player.WorldY >= worldSize)
            {
                needsNewMap = false;
                player.WorldX = Math.Max(0, Math.Min(player.WorldX, worldSize - 1));
                player.WorldY = Math.Max(0, Math.Min(player.WorldY, worldSize - 1));
            }

            if (needsNewMap)
            {
                // Effacer la position du joueur sur l'ancienne carte
                Map? oldMap = GetMapAt(oldWorldX, oldWorldY);
                if (oldMap != null)
                {
                    oldMap.ClearPlayerPosition(oldLocalX, oldLocalY);
                }

                // Placer le joueur sur la nouvelle carte
                Map? newMap = GetMapAt(player.WorldX, player.WorldY);
                if (newMap != null)
                {
                    newMap.PlacePlayer(player.LocalX, player.LocalY);
                }
            }
        }

        public void CheckForEncounter(Player player)
        {
            Map? currentMap = GetMapAt(player.WorldX, player.WorldY);
            if (currentMap != null && currentMap.IsEnemy(player.LocalX, player.LocalY))
            {
                // Gérer la rencontre entre le joueur et l'ennemi
                HandleEncounter(player, enemymap);
            }
        }
        private void HandleEncounter(Player player, EnemyMap enemy)
        {
            // Combat entre le joueur et l'ennemi
        }

        private void EnsurePlayerOnLand(Map currentMap, Player player)
        {
            if (currentMap.IsWater(player.LocalX, player.LocalY))
            {
                // Trouver la case d'herbe la plus proche pour repositionner le joueur
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int newX = player.LocalX + offsetX;
                        int newY = player.LocalY + offsetY;

                        if (newX >= 0 && newX < 20 && newY >= 0 && newY < 20)
                        {
                            if (currentMap.IsGrass(newX, newY))
                            {
                                // Place le joueur sur cette position d'herbe
                                currentMap.PlacePlayer(newX, newY);
                                player.LocalX = newX;
                                player.LocalY = newY;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void InitializeEnemy()
        {
            // Placer l'ennemi de manière aléatoire sur la carte centrale
            Random random = new Random();
            int enemyLocalX = random.Next(1, 19); // Éviter les bords
            int enemyLocalY = random.Next(1, 19); // Éviter les bords

            enemymap = new EnemyMap(1, 1, enemyLocalX, enemyLocalY); // Positionner l'ennemi dans le monde 3x3
            Map centerMap = worldMaps[1, 1];
            centerMap.PlaceEnemy(enemymap.LocalX, enemymap.LocalY);
        }

        private void PlaceEnemiesRandomly(Map map)
        {
            Random random = new Random();
            int numberOfEnemies = random.Next(1, 3); // Entre 1 et 3 ennemis

            for (int i = 0; i < numberOfEnemies; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(17);
                    y = random.Next(17);
                } while (map.IsWater(x, y) || map.IsPlayer(x, y) || map.matrix[x, y] == 'O'); 

                map.PlaceEnemy(x, y); // Placer l'ennemi
            }
        }
    }
}
