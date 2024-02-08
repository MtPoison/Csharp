
using MapEntities;
using Newtonsoft.Json;

public abstract class EntityAbstract
{
    public string _name;
    public string _type;
    public int _blocked;
    public int _currentBlocked;
    public int _maxhealth;
    public float _health;
    public float _stamina;
    public int _maxStamina;
    public float _currentStamina;
    public int _speed;
    public float _resistanceFeu;
    public float _resistanceEau;
    public float _resistanceVent;
    public float _resistancePhysique;
    public float _boostDamage;
    public int _level;
    public int _experience;
    public int _maxExerience;
    public bool gameReset = false;
    public bool gameStart = true;

    public List<EntitiesCapacities> _ListCapacities { get; set; }
    public EntityContainer entitiesContainer;

    public int _currentLevel { get; set; }

    public abstract void DisplayDetails();
    public abstract void AddHealth(int add);
    public abstract void AddStamina(int add);
    public abstract void LessStamina(float less);
    public abstract void TakeDamage(float less);
    public abstract void AddLevel();
    public abstract void AddExperience(int add);
    public abstract void Loot(Player p);

    public void CreateEntity(string path, EntityContainer entities)
    {
        Allies allies = new Allies
        {
            _name = "Ace",
            _type = "Logia",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 300,
            _health = 300.0f,
            _stamina = 200.0f,
            _maxStamina = 200,
            _speed = 40,
            _resistanceFeu = 2.0f,
            _resistanceEau = 0.5f,
            _resistanceVent = 0.75f,
            _resistancePhysique = 1.5f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Chopped",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.4f,
                    _level = 0
                },
                 new EntitiesCapacities
                {
                    _name = "Fire Punch",
                    _type = "Feu",
                    _damage = 40.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 2
                },
                new EntitiesCapacities
                {
                    _name = "Great Ring of Fire: Horse of the Sun",
                    _type = "Feu",
                    _damage = 80.0f,
                     _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 3
                }
            },
            _currentLevel = 1,
            _currentStamina = 200,
        };

        Allies allies2 = new Allies
        {
            _name = "Luffy",
            _type = "Paramecia",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300.0f,
            _maxStamina = 300,
            _speed = 50,
            _resistanceFeu = 1.5f,
            _resistanceEau = 0.5f,
            _resistanceVent = 1.25f,
            _resistancePhysique = 2.0f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Jet Pistol",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.4f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Red Hawk",
                    _type = "Physique",
                    _damage = 50.0f,
                    _stamina = 100.0f,
                    _speed = 70,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 2
                },
                new EntitiesCapacities
                {
                    _name = "Gear 5",
                    _type = "Physique",
                    _damage = 60.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 3
                },
                new EntitiesCapacities
                {
                    _name = "Haki",
                    _type = "Physique",
                    _damage = 70.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 5
                }
            },
            _currentLevel = 1,
            _currentStamina = 300,
        };

        Allies allies3 = new Allies
        {
            _name = "Jimbey",
            _type = "Logia",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300.0f,
            _maxStamina = 300,
            _speed = 50,
            _resistanceFeu = 0.5f,
            _resistanceEau = 2.0f,
            _resistanceVent = 0.75f,
            _resistancePhysique = 1.0f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Fish-Man Karate",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Water Shot",
                    _type = "Eau",
                    _damage = 30.0f,
                    _stamina = 100.0f,
                    _speed = 70,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 1
                },
                new EntitiesCapacities
                {
                    _name = "Heart of Water",
                    _type = "Eau",
                    _damage = 50.0f,
                    _stamina = 100.0f,
                    _speed = 70,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 4
                }
            },
            _currentLevel = 1,
            _currentStamina = 300,
        };

        Enemy enemy = new Enemy
        {
            _name = "Marine",
            _type = "Humain",
            _difficultyIA = "Normal",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300.0f,
            _maxStamina = 300,
            _speed = 50,
            _resistanceFeu = 0.5f,
            _resistanceEau = 0.5f,
            _resistanceVent = 0.5f,
            _resistancePhysique = 0.75f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
                    {
                        new EntitiesCapacities
                        {
                            _name = "Punch",
                            _type = "Physique",
                            _damage = 5.0f,
                            _stamina = 20.0f,
                            _speed = 20,
                            _boostDamage = 1.0f,
                            _criticalChance = 1.2f,
                            _level = 0
                        },
                        new EntitiesCapacities
                        {
                            _name = "Epee",
                            _type = "Physique",
                            _damage = 20.0f,
                            _stamina = 20.0f,
                            _speed = 20,
                            _boostDamage = 1.0f,
                            _criticalChance = 1.2f,
                            _level = 0
                        }
                    },
            _currentLevel = 1,
            _currentStamina = 300,
        };

        Enemy enemy2 = new Enemy
        {
            _name = "Amarial Sengoku",
            _type = "Zoan",
            _difficultyIA = "Dificil",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300.0f,
            _maxStamina = 300,
            _speed = 50,
            _resistanceFeu = 1.5f,
            _resistanceEau = 1.5f,
            _resistanceVent = 1.5f,
            _resistancePhysique = 1.5f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
                    {
                        new EntitiesCapacities
                        {
                            _name = "Geyser of Purification",
                            _type = "Eau",
                            _damage = 30.0f,
                            _stamina = 20.0f,
                            _speed = 20,
                            _criticalChance = 1.2f,
                            _level = 0
                        },
                        new EntitiesCapacities
                        {
                            _name = "Buddhist Inferno",
                            _type = "Feu",
                            _damage = 30.0f,
                            _stamina = 20.0f,
                            _speed = 20,
                            _criticalChance = 1.2f,
                            _level = 0
                        },
                        new EntitiesCapacities
                        {
                            _name = "Haki",
                            _type = "Vent",
                            _damage = 50.0f,
                            _stamina = 20.0f,
                            _speed = 20,
                            _criticalChance = 1.2f,
                            _level = 0
                        },
                        new EntitiesCapacities
                        {
                            _name = "Daibutsu",
                            _type = "Physique",
                            _damage = 100.0f,
                            _stamina = 100.0f,
                            _speed = 70,
                            _criticalChance = 1.2f,
                            _level = 0
                        }
                    },
            _currentLevel = 1,
            _currentStamina = 300,
        };

        Enemy enemy3 = new Enemy
        {
            _name = "Kobby",
            _type = "Paramecia",
            _difficultyIA = "Hard",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300.0f,
            _maxStamina = 300,
            _speed = 50,
            _resistanceFeu = 0.75f,
            _resistanceEau = 0.75f,
            _resistanceVent = 0.75f,
            _resistancePhysique = 1.25f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Six Powers",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _criticalChance = 1.2f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Honesty Impact",
                    _type = "Eau",
                    _damage = 50.0f,
                    _stamina = 100.0f,
                    _speed = 70,
                    _criticalChance = 1.2f,
                    _level = 0
                }
            },
            _currentLevel = 1,
            _currentStamina = 300,
        };

        Enemy enemy4 = new Enemy
        {
            _name = "Doflamingo",
            _type = "Paramecia",
            _difficultyIA = "Hard",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300.0f,
            _maxStamina = 300,
            _speed = 50,
            _resistanceFeu = 1.10f,
            _resistanceEau = 1.10f,
            _resistanceVent = 2.0f,
            _resistancePhysique = 1.5f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Ball of yarn",
                    _type = "Vent",
                    _damage = 40.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _criticalChance = 1.2f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Bird Cage",
                    _type = "Vent",
                    _damage = 70.0f,
                    _stamina = 100.0f,
                    _speed = 70,
                    _criticalChance = 1.2f,
                    _level = 0
                }
            },
            _currentLevel = 1,
            _currentStamina = 300,
        };

        Enemy enemy5 = new Enemy
        {
            _name = "Sanglier",
            _type = "Humain",
            _difficultyIA = "Normal",
            _blocked = 0,
            _currentBlocked = 0,
            _maxhealth = 100,
            _health = 100.0f,
            _stamina = 300.0f,
            _maxStamina = 300,
            _speed = 50,
            _resistanceFeu = 1.0f,
            _resistanceEau = 1.0f,
            _resistanceVent = 1.0f,
            _resistancePhysique = 1.0f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Morsure",
                    _type = "Physique",
                    _damage = 5.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _criticalChance = 1.2f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Charge",
                    _type = "Physique",
                    _damage = 10.0f,
                    _stamina = 100.0f,
                    _speed = 70,
                    _criticalChance = 1.4f,
                    _level = 0
                }
            },
            _currentLevel = 1,
            _currentStamina = 300,
        };

        entitiesContainer = new EntityContainer
        {
            AlliesList = new List<Allies> { allies, allies2, allies3 },
            EnemiesList = new List<Enemy> { enemy, enemy, enemy3, enemy4, enemy5 },
        };

        // Check le nb d'allié pour l'hud
        foreach (var addAllies in entitiesContainer.AlliesList)
        {
            if (!entities.AlliesList.Any(noDoublons => noDoublons._name == addAllies._name))
            {
                entities.AlliesList.Add(addAllies);
            }
        }


        if (gameReset || !GetExistsJson(path) || GetEmptyJson(path))
        {
            string json = JsonConvert.SerializeObject(entitiesContainer);
            File.WriteAllText(path, json);
            Console.WriteLine("Données sauvegardées dans le fichier : " + path);
        }

    }

    private bool GetExistsJson(string path)
    {
        return File.Exists(path);
    }

    private bool GetEmptyJson(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            return string.IsNullOrEmpty(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading JSON file: " + ex.Message);
            return true; 
        }
    }

    public void GetInfoEntity(string path)
    {
        string json = File.ReadAllText(path);
        entitiesContainer = JsonConvert.DeserializeObject<EntityContainer>(json);
    }

    public EntityContainer GetInfoEntityUpdateLevel(string path)
    {
        string json = File.ReadAllText(path);
        EntityContainer entities = JsonConvert.DeserializeObject<EntityContainer>(json);

        foreach (var ally in entities.AlliesList)
        {
            ally._currentLevel = ally._level;
        }

        return entities;
    }

    public EntityContainer GetInfoEntityUpdateStamina(string path)
    {
        string json = File.ReadAllText(path);
        EntityContainer entities = JsonConvert.DeserializeObject<EntityContainer>(json);

        foreach (var ally in entities.AlliesList)
        {
            ally._currentStamina = ally._stamina;
        }

        return entities;
    }

    public EntityContainer GetInfoEntityUpdateBlocked(string path)
    {
        string json = File.ReadAllText(path);
        EntityContainer entities = JsonConvert.DeserializeObject<EntityContainer>(json);

        foreach (var ally in entities.AlliesList)
        {
            ally._currentBlocked = ally._blocked;
        }

        return entities;
    }


    public void UpdateJsonLevel(EntityContainer entities, string path)
    {
        var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a._name.Equals(this._name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate._currentLevel = _level;
        }

        // Maj du JSON avec la modif du level
        string updatedJson = JsonConvert.SerializeObject(entities);
        File.WriteAllText(path, updatedJson);
    }

    public void UpdateJsonStamina(EntityContainer entities, string path)
    {
        var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a._name.Equals(this._name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate._currentStamina = _stamina;
        }

        using (StreamWriter writer = File.CreateText(path))
        {
            string updatedJson = JsonConvert.SerializeObject(entities);
            writer.Write(updatedJson);
        }
    }




    public void UpdateJsonBlocked(EntityContainer entities, string path, int value)
    {
        var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a._name.Equals(this._name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate._currentBlocked = value;
        }

        // Maj du JSON avec la modif du blocked
        using (StreamWriter writer = File.CreateText(path))
        {
            string updatedJson = JsonConvert.SerializeObject(entities);
            writer.Write(updatedJson);
        }
    }

}