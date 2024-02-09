using MapEntities;
using Newtonsoft.Json;

public class Fight
{
    private bool tourAlier = false;
    private bool allieSwitch = false;
    private bool firstSwitch = false;
    Random random = new Random();

    public void ResetEnemyHealth(EntityContainer entities, bool sanglier, int iaType)
    {
        EntityAbstract enemy = null;

        if (sanglier)
        {
            enemy = entities.enemiesList.FirstOrDefault(e => e.name == "Sanglier");
        }
        else
        {
            if (iaType == 1)
            {
                enemy = entities.enemiesList.FirstOrDefault(e => e.name == "Sanglier");
            }
            else if (iaType == 2)
            {
                enemy = entities.enemiesList.FirstOrDefault(e => e.name == "Marine");
            }
            else if (iaType == 3)
            {
                enemy = entities.enemiesList.FirstOrDefault(e => e.name == "Kobby");
            }
        }

        if (enemy != null)
        {
            enemy.health = enemy.maxhealth;
        }
    }


    public void StartCombat(EntityContainer entities, bool sanglier, Player player, int iaType)
    {
        string path = "../../../Entities/entity.json";

        EntityAbstract allie = GetFirstAliveAlly(path);
        EntityAbstract enemie = entities.enemiesList[0];


        if (allie != null)
        {
            ResetEnemyHealth(entities, sanglier, iaType);

           

            if (sanglier)
            {
                enemie = entities.enemiesList[3];
                player.SangliersTues++;
            }

            ShowStatusOfFighters(allie, enemie);

            // IA easy
            if (iaType == 1)
            {
                while (enemie.health > 0)
                {

                    DetermineTour(ref allie, enemie);
                    
                    if (tourAlier)
                    {
                        
                        if (!HandleAllieTurn(entities, ref allie, enemie, player)) { break; }
                    }
                    else
                    {
                        HandleEnemyTurnIAEasy(allie, enemie, entities, player);

                    }
                    ShowStatusOfFighters(allie, enemie);
                }
            }
            // IA Normal
            else if (iaType == 2)
            {
                while (enemie.health > 0)
                {

                    DetermineTour(ref allie, enemie);

                    if (tourAlier)
                    {
                        
                        if( !HandleAllieTurn(entities, ref allie, enemie, player))
                        { break; }
                    }
                    else
                    {
                        HandleEnemyTurnIANormal(allie, enemie, entities, player);
                    }
                    ShowStatusOfFighters(allie, enemie);
                }
            }
            // IA Hard
            else if (iaType == 3)
            {
                while (enemie.health > 0)
                {

                    DetermineTour(ref allie, enemie);

                    if (tourAlier)
                    {
                        
                        if (!HandleAllieTurn(entities, ref allie, enemie, player)) { break; }
                    }
                    else
                    {
                        if (!firstSwitch)
                        {
                            firstSwitch = true;
                            IAHardCalculSwitchEnemy(allie, ref enemie, ref entities, player);
                        }
                        else
                        {
                            HandleEnemyTurnIAHard(allie, ref enemie, ref entities, player);
                        }

                    }
                    ShowStatusOfFighters(allie, enemie);
                }

            }
        }
    }

    // Renvoie le premier allié en vie
    private EntityAbstract GetFirstAliveAlly(string path)
    {
        string json = File.ReadAllText(path);
        var loadedEntities = JsonConvert.DeserializeObject<EntityContainer>(json);

        if (loadedEntities != null && loadedEntities.alliesList != null)
        {
            foreach (var ally in loadedEntities.alliesList)
            {
                if (ally.health > 0 && ally.stamina > 0)
                {
                    return ally;
                }
            }
        }
        return null;
    }

    private void DetermineTour(ref EntityAbstract allie, EntityAbstract enemie)
    {
        tourAlier = allie.speed > enemie.speed || tourAlier;
    }

    private bool HandleAllieTurn(EntityContainer entities, ref EntityAbstract allie, EntityAbstract enemie, Player p)
    {
        bool canAttack = false;
        List<string> options;
        int selectedIndex;

        bool foundAlliesToReplace = false;

        if (allie.health > 0.0 && allie.stamina > 0.0)
        {
            foundAlliesToReplace = true;
        }

        // cherche un allié qui est en vie et qui a du stamina
        if (!foundAlliesToReplace)
        {
            foreach (EntityAbstract ally in entities.alliesList)
            {
                if (ally.health > 0.0 && ally.stamina > 0.0)
                {
                    allie = ally;
                    foundAlliesToReplace = true;
                    break;
                }
            }
        }

        // Loose s'il en trouve aucun
        if (!foundAlliesToReplace)
        {
            if (p.NBAlcool > 0)
            {
                for (int i = 0; i < allie.listCapacities.Count(); i++)
                {
                    if (allie.stamina <= allie.listCapacities[i].stamina && allie.level >= allie.listCapacities[i].level)
                    {
                        canAttack = true;
                    }
                }
                if (canAttack)
                {
                    options = new List<string> { "Plus assez de mana, prends-en dans ton inventaire!" };
                    selectedIndex = RunOptions(options, allie, enemie);
                    switch (selectedIndex)
                    {
                        case 0:
                            playerInventory(p, allie, enemie);
                            break;
                    }
                }
                else
                {
                    options = new List<string> { "Tu as perdu" };
                    selectedIndex = RunOptions(options, allie, enemie);
                    return false;
                }
            }
            else
            {
                options = new List<string> { "Tu as perdu" };
                selectedIndex = RunOptions(options, allie, enemie);
                return false;
            }
        }
        else
        {
            options = new List<string> { "Changer d'allié", "Attaquer", "Inventory" };
            selectedIndex = RunOptions(options, allie, enemie);
            switch (selectedIndex)
            {
                case 0:
                    ChangeAllie(entities, ref allie, enemie);
                    allieSwitch = true;
                    break;
                case 1:
                    AttackEnemy(allie, enemie);
                    break;
                case 2:
                    playerInventory(p, allie, enemie);
                    break;
            }
        }

        CheckHealth(enemie, allie, p);
        tourAlier = false;
        return true;
    }

    // Inventaire en combat, le player peux use des items
    public void playerInventory(Player player, EntityAbstract allie, EntityAbstract enemie)
    {
        List<string> options;
        int selectedIndex;

        options = new List<string> { $"Soin Disponible : {player.NBViande}", $"Stamina Disponible : {player.NBAlcool}" };

        selectedIndex = RunOptions(options, allie, enemie);
        switch (selectedIndex)
        {
            case 0:
                if (player.NBViande > 0)
                {
                    Health(player, allie);
                }
                break;
            case 1:
                if (player.NBAlcool > 0)
                {
                    Stamina(player, allie);
                }
                break;
        }
    }

    public void Health(Player player, EntityAbstract allie) 
    {
        player.RemoveViande(1);
        allie.AddHealth(20);
    }

    public void Stamina(Player player, EntityAbstract allie)
    {
        player.RemoveAlcool(1);
        allie.AddStamina(20);
    }

    // Récupère l'index choisi par le player
    public int RunOptionsSwitch(List<string> options, ref EntityAbstract allie, EntityAbstract enemie)
    {
        ConsoleKey keyPressed;
        int selectedIndex = 0;
        bool isSelected = false;

        do
        {
            DisplayOptionsSwitch(options, selectedIndex, ref allie, enemie);

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
            else if (keyPressed == ConsoleKey.Enter)
            {
                isSelected = true;
            }
        } while (!isSelected);

        return selectedIndex;
    }

    private void DisplayOptionsSwitch(List<string> options, int selectedIndex, ref EntityAbstract allie, EntityAbstract enemie)
    {
        ShowStatusOfFighters(allie, enemie);

        Console.WriteLine("Veuillez choisir un allié en utilisant son index :");
        for (int i = 0; i < options.Count; i++)
        {
            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine($"* [{i + 1}] {options[i]}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"  [{i + 1}] {options[i]}");
            }
        }
        Console.ResetColor();
    }

    private void ChangeAllie(EntityContainer entities, ref EntityAbstract allie, EntityAbstract enemie)
    {
        // va chercher tout les alliés dans le json qui ont de la vie
        List<string> alliesNames = entities.alliesList
            .Where(a => a.health > 0.0)
            .Select(a => a.name)
            .ToList();

        int selectedIndex = RunOptionsSwitch(alliesNames, ref allie, enemie);
        string selectedName = alliesNames[selectedIndex];
        EntityAbstract newAllie = entities.alliesList.FirstOrDefault(a => a.name == selectedName);

        allie = newAllie;
        ShowStatusOfFighters(allie, enemie);
    }


    private void ChangeEnemy(EntityAbstract allie, ref EntityAbstract enemie)
    {
        // l'ia va changer automatiquement d'enemie
        EntityAbstract newEnemy = enemie;
        ShowStatusOfFighters(allie, newEnemy);
    }

    private void AttackEnemy(EntityAbstract allie, EntityAbstract enemie)
    {
        // affiche les capacités pour attaqué l'énemie en fonction du stamina restant
        Console.WriteLine($"Choisis la capacité que tu veux utiliser pour attaquer :\n");
        List<string> displayedCapacities = new List<string>();

        for (int i = 0; i < allie.listCapacities.Count; i++)
        {
            if (allie.level >= allie.listCapacities[i].level && !displayedCapacities.Contains(allie.listCapacities[i].name))
            {
                Console.WriteLine($"- {allie.listCapacities[i].name}");
                displayedCapacities.Add(allie.listCapacities[i].name);
            }
        }

        int selectedIndex = RunOptions(displayedCapacities, allie, enemie);

        if (allie.stamina >= allie.listCapacities[selectedIndex].stamina)
        {
            ManageDamageByType(selectedIndex, allie, enemie);
        }

        ShowStatusOfFighters(allie, enemie);
    }

    private void ManageDamageByType(int selectedIndex, EntityAbstract allie, EntityAbstract enemie)
    {
        // GESTION DES DEGATS EN FONCTION DE LA RESITANCE DE CHAQUE TYPE DE FRUIT DU DEMON //

        /* Si l'enemy est de type Humain alors je lui fait 50% de dégât en plus*/
        if (allie.type == "Logia" && enemie.type == "Humain")
        {
            enemie.TakeDamage(allie.listCapacities[selectedIndex].damage * 1.50f);
            allie.LessStamina(allie.listCapacities[selectedIndex].stamina);
        }
        /* Si l'enemy est de type Paramecia alors je lui fait 25% de dégâts en plus*/
        else if (allie.type == "Logia" && enemie.type == "Paramecia")
        {
            enemie.TakeDamage(allie.listCapacities[selectedIndex].damage * 1.25f);
            allie.LessStamina(allie.listCapacities[selectedIndex].stamina);
        }
        /* Si l'enemy est de type Zoan alors je lui fait 10% de dégâts en moins*/
        else if (allie.type == "Logia" && enemie.type == "Zoan")
        {
            enemie.TakeDamage(allie.listCapacities[selectedIndex].damage * 0.90f);
            allie.LessStamina(allie.listCapacities[selectedIndex].stamina);
        }
        /* Si l'enemy est de type Humain alors je lui fait 25% de dégâts en plus*/
        else if (allie.type == "Paramecia" && enemie.type == "Humain")
        {
            enemie.TakeDamage(allie.listCapacities[selectedIndex].damage * 1.25f);
            allie.LessStamina(allie.listCapacities[selectedIndex].stamina);
        }
        /* Si l'enemy est de type Logia alors je lui fait 60% de dégâts en moins*/
        else if (allie.type == "Paramecia" && enemie.type == "Logia")
        {
            enemie.TakeDamage(allie.listCapacities[selectedIndex].damage * 0.40f);
            allie.LessStamina(allie.listCapacities[selectedIndex].stamina);
        }
        /* Si l'enemy est de type Humain alors je lui fait 10% de dégâts en moins*/
        else if (allie.type == "Paramecia" && enemie.type == "Humain")
        {
            enemie.TakeDamage(allie.listCapacities[selectedIndex].damage * 0.90f);
            allie.LessStamina(allie.listCapacities[selectedIndex].stamina);
        }
    }

    private void UpdateJsonHealth(EntityAbstract entity, string path)
    {
        // Update le health de l'allié target dans le json
        EntityContainer entities;

        try
        {
            // Pour éviter les crash json, on libère la ressource après utlisation
            using (StreamReader reader = File.OpenText(path))
            {
                string json = reader.ReadToEnd();
                entities = JsonConvert.DeserializeObject<EntityContainer>(json);
            }

            if (entity is Allies)
            {
                var targetAllies = entities.alliesList.FirstOrDefault(a => a.name.Equals(entity.name, StringComparison.OrdinalIgnoreCase));
                if (targetAllies != null)
                {
                    if (entity.health < 0)
                    {
                        targetAllies.health = 0.0f;
                    }
                    else
                    {
                        targetAllies.health = entity.health;
                    }
                }
            }

            // Pour éviter les crash json, on libère la ressource après utlisation
            using (StreamWriter writer = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, entities);
                writer.Close();
            }
        }
        catch (IOException ex)
        {
            // exception de crash
            Console.WriteLine($"Une erreur s'est produite lors de l'accès au fichier : {ex.Message}");
        }
    }


    private void HandleEnemyTurnIAEasy(EntityAbstract allie, EntityAbstract enemie, EntityContainer entities, Player p)
    {
        // l'ia easy attaque avec des randoms
        int nombreAleatoire = random.Next(1, 4);
        int randomAttackEnemy = random.Next(0, 2);

        switch (nombreAleatoire)
        {
            case 1:
                allie.TakeDamage(enemie.listCapacities[randomAttackEnemy].damage);
                Console.WriteLine($"{enemie.name} attaque {allie.name} et inflige {enemie.listCapacities[0].damage} dégâts!");
                break;
            case 2:
                int healAmount = 10;
                enemie.AddHealth(healAmount);
                Console.WriteLine($"{enemie.name} se soigne et regagne {healAmount} points de vie!");
                break;
            case 3:
                int staminaAmount = 10;
                enemie.AddStamina(staminaAmount);
                Console.WriteLine($"{enemie.name} regagne {staminaAmount} points d'endurance!");
                break;
        }

        string path = "../../../Entities/entity.json";
        UpdateJsonHealth(allie, path);

        CheckHealth(enemie, allie, p);
        tourAlier = true;
    }

    private void HandleEnemyTurnIANormal(EntityAbstract allie, EntityAbstract enemie, EntityContainer entities, Player p)
    {
        // IA Normal
        IACalculMaxDamage(allie, enemie);

        string path = "../../../Entities/entity.json";
        UpdateJsonHealth(allie, path);

        CheckHealth(enemie, allie, p);
        tourAlier = true;
    }

    private void IACalculMaxDamage(EntityAbstract allie, EntityAbstract enemie)
    {
        // L'IA va calculer constamment la meilleur attaque qu'elle va pouvoir infliger au player
        int UpAttackIndex = -1;
        float damageUp = float.MinValue;

        for (int i = 0; i < enemie.listCapacities.Count(); i++)
        {
            float damage = 0;
            switch (enemie.listCapacities[i].type)
            {
                /* Check toutes ces capacités et renvoi l'attaque qui fera le plus mal au player */
                case "Eau":
                    damage = enemie.listCapacities[i].damage / allie.resistanceEau;
                    break;
                case "Feu":
                    damage = enemie.listCapacities[i].damage / allie.resistanceFeu;
                    break;
                case "Vent":
                    damage = enemie.listCapacities[i].damage / allie.resistanceVent;
                    break;
                case "Physique":
                    damage = enemie.listCapacities[i].damage / allie.resistancePhysique;
                    break;
            }

            if (damage > damageUp)
            {
                damageUp = damage;
                UpAttackIndex = i;
            }
        }

        if (UpAttackIndex != -1)
        {
            allie.TakeDamage(damageUp);
        }
    }

    private void HandleEnemyTurnIAHard(EntityAbstract allie, ref EntityAbstract enemy, ref EntityContainer enemies, Player p)
    {
        /* Si le player n'a pas changé de perso ou que ce n'est pas le début du tour alors l'IA attaque */
        if (allieSwitch)
        {
            allieSwitch = false;
            IAHardCalculSwitchEnemy(allie, ref enemy, ref enemies, p);
        }
        else
        {
            IACalculMaxDamage(allie, enemy);
        }

        string path = "../../../Entities/entity.json";
        UpdateJsonHealth(allie, path);

        CheckHealthAllie(allie, enemies, enemy, p);
        tourAlier = true;

    }

    private void IAHardCalculSwitchEnemy(EntityAbstract allie, ref EntityAbstract enemy, ref EntityContainer enemies, Player p)
    {
        // L'IA Hard va décider elle-même si elle va switch de perso ou pas ( elle switchera si elle n'a pas l'attaque nécessaire par exmeple )
        Console.WriteLine("IA Hard");

        Dictionary<string, float> attackInfo = new Dictionary<string, float>();

        int UpAttackIndex = -1;
        float damageUp = float.MinValue;

        for (int i = 0; i < enemies.enemiesList.Count(); i++)
        {
            if (enemies.enemiesList[i].difficultyIA == "Hard" && enemies.enemiesList[i].health > 0)
            {
                for (int j = 0; j < enemies.enemiesList[i].listCapacities.Count(); j++)
                {
                    float damage = 0;

                    switch (enemies.enemiesList[i].listCapacities[j].type)
                    {
                        /* Check toutes ces capacités et renvoi l'attaque qui fera le plus mal au player */
                        case "Eau":
                            damage = enemies.enemiesList[i].listCapacities[j].damage / allie.resistanceEau;
                            attackInfo.Add(enemies.enemiesList[i].listCapacities[j].name, damage);
                            break;
                        case "Feu":
                            damage = enemies.enemiesList[i].listCapacities[j].damage / allie.resistanceFeu;
                            attackInfo.Add(enemies.enemiesList[i].listCapacities[j].name, damage);
                            break;
                        case "Vent":
                            damage = enemies.enemiesList[i].listCapacities[j].damage / allie.resistanceVent;
                            attackInfo.Add(enemies.enemiesList[i].listCapacities[j].name, damage);
                            break;
                        case "Physique":
                            damage = enemies.enemiesList[i].listCapacities[j].damage / allie.resistancePhysique;
                            attackInfo.Add(enemies.enemiesList[i].listCapacities[j].name, damage);
                            break;
                    }
                    if (damage > damageUp)
                    {
                        damageUp = damage;
                        UpAttackIndex = i;
                    }
                }
            }
        }

        if (UpAttackIndex != -1)
        {
            enemy = enemies.enemiesList[UpAttackIndex];
            ChangeEnemy(allie, ref enemy);
        }
    }

    private void CheckHealthAllie(EntityAbstract allie, EntityContainer entities, EntityAbstract enemy, Player p)
    {
        for(int i = 0; i < entities.alliesList.Count(); i++)
        {
            if (allie.health < 0 && entities.alliesList[i].health > 0)
            {
                HandleAllieTurn(entities, ref allie, enemy, p);
            }
            else
            {
                int nombreAleatoire = random.Next(2, 10 * enemy.level);
                allie.AddExperience(nombreAleatoire);
                enemy.Loot(p);
            }
        }
    }

    private void CheckHealth(EntityAbstract enemie, EntityAbstract allie, Player p)
    {
        if (enemie.health <= 0)
        {
            Console.WriteLine(@"Tu as gagné");
            int nombreAleatoire = random.Next(2, 10 * enemie.level);
            allie.AddExperience(nombreAleatoire);
            enemie.Loot(p);

        }
        else if (allie.health <= 0)
        {
            Console.WriteLine("Tu as perdu");
        }
    }

    public int RunOptions(List<string> options, EntityAbstract allie, EntityAbstract enemie)
    {

        ConsoleKey keyPressed;
        int selectedIndex = 0;
        do
        {
            DisplayOptions(options, selectedIndex, allie, enemie);

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

    private void ShowStatusOfFighters(EntityAbstract allie, EntityAbstract enemie)
    {
        Console.Clear();
        Console.WriteLine(@"
 ██████╗ ██████╗ ███╗   ███╗██████╗  █████╗ ████████╗
██╔════╝██╔═══██╗████╗ ████║██╔══██╗██╔══██╗╚══██╔══╝
██║     ██║   ██║██╔████╔██║██████╔╝███████║   ██║   
██║     ██║   ██║██║╚██╔╝██║██╔══██╗██╔══██║   ██║   
╚██████╗╚██████╔╝██║ ╚═╝ ██║██████╔╝██║  ██║   ██║   
 ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═════╝ ╚═╝  ╚═╝   ╚═╝   
    

");


        Console.ResetColor();

        DisplayHealthBar(allie, enemie);

    }


    private void DisplayOptions(List<string> options, int selectedIndex, EntityAbstract allie, EntityAbstract enemie)
    {
        ShowStatusOfFighters(allie, enemie);

        Console.WriteLine("Veuillez choisir une option :");
        for (int i = 0; i < options.Count; i++)
        {
            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine($"* {options[i]}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"  {options[i]}");
            }
        }
        Console.ResetColor();
    }
    private void DisplayHealthBar(EntityAbstract allie, EntityAbstract enemie)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        if (allie != null && enemie != null)
        {
            Console.WriteLine($"Stamina de {allie.name}");
            DrawStaminaBar(allie.stamina, allie.maxStamina);

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(allie.name);
            DrawHealthBar(allie.health, allie.maxhealth);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(enemie.name);
            DrawHealthBar(enemie.health, enemie.maxhealth);
            Console.WriteLine();
        }
    }

    static void DrawHealthBar(float currentHealth, int maxHealth)
    {
        // dessine la bar de health
        int maxHealthImplicit = maxHealth ; 
        int barWidth = 40;

        currentHealth = Math.Max(currentHealth, 0);

        int currentWidth = (int)((double)currentHealth / maxHealthImplicit * barWidth);

        ConsoleColor color;
        double healthPercentage = (double)currentHealth / maxHealthImplicit;

        if (healthPercentage > 0.75)
            color = ConsoleColor.Green;
        else if (healthPercentage > 0.5)
            color = ConsoleColor.Yellow;
        else if (healthPercentage > 0.25)
            color = ConsoleColor.Red;
        else
            color = ConsoleColor.DarkRed;

        Console.ForegroundColor = color;
        Console.Write("[");
        for (int i = 0; i < barWidth; i++)
        {
            if (i < currentWidth)
                Console.Write("█");
            else
                Console.Write(" ");
        }
        Console.Write("]");
        Console.ResetColor();
        Console.WriteLine($" {currentHealth}/{maxHealthImplicit}");
    }

    static void DrawStaminaBar(float currentHealth, int maxHealth)
    {
        // dessine la bar de stamina
        int maxHealthImplicit = maxHealth;
        int barWidth = 40;

        currentHealth = Math.Max(currentHealth, 0);

        int currentWidth = (int)((double)currentHealth / maxHealthImplicit * barWidth);

        ConsoleColor color = ConsoleColor.Magenta;

        Console.ForegroundColor = color;
        Console.Write("[");
        for (int i = 0; i < barWidth; i++)
        {
            if (i < currentWidth)
                Console.Write("█");
            else
                Console.Write(" ");
        }
        Console.Write("]");
        Console.ResetColor();
        Console.WriteLine($" {currentHealth}/{maxHealthImplicit}");
    }
}