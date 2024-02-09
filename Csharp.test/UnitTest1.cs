
using MapEntities;
using MapGame;
namespace Csharp.test
{
    public class Tests
    {
        [Test]
        public void VerifyTheMapIsNotNull()
        {
            var world = new World();

            var map = world.GetMapAt(0, 0);

            Assert.IsNotNull(map);
        }
        [Test]
        public void VerrifyIfContenteAGreatEllement()
        {
            World world = new World();
            char[,] layout = world.CreateRandomLayout(false, false, false, 20, 20);

            Assert.IsTrue(ContainsElement(layout, '.')); 
        }

        [Test]
        public void VerifyIfOneStructurAreCreate()
        {
            World world = new World();

            char[,] layout = world.CreateRandomLayout(false, true, false, 20, 20);

            Assert.IsTrue(ContainsElement(layout, 'H')); 
            Assert.IsTrue(ContainsElement(layout, ']') || ContainsElement(layout, '[') || ContainsElement(layout, '―'));
        }

        private bool ContainsElement(char[,] layout, char element)
        {
            for (int i = 0; i < layout.GetLength(0); i++)
            {
                for (int j = 0; j < layout.GetLength(1); j++)
                {
                    if (layout[i, j] == element)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [Test]
        public void VerifyIfTheListAddElementNotNull() 
        {
            var entities = new EntityContainer();
            Allies allie = new Allies
            {
                name = "Ace",
                type = "Logia",
                maxhealth = 300,
                health = 300.0f,
                stamina = 200.0f,
                maxStamina = 200,
                speed = 40,
                resistanceFeu = 2.0f,
                resistanceEau = 0.5f,
                resistanceVent = 0.75f,
                resistancePhysique = 1.5f,
                boostDamage = 1.0f,
                level = 1,
                listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Chopped",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.4f,
                    level = 0
                },
                 new EntitiesCapacities
                {
                    name = "Fire Punch",
                    type = "Feu",
                    damage = 40.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Great Ring of Fire: Horse of the Sun",
                    type = "Feu",
                    damage = 80.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 0
                }
            },
                currentLevel = 1,
                currentStamina = 200,
            };
            Enemy enemie = new Enemy
            {
                name = "Marine",
                type = "Humain",
                difficultyIA = "Normal",
                maxhealth = 500,
                health = 500.0f,
                stamina = 300.0f,
                maxStamina = 300,
                speed = 50,
                resistanceFeu = 0.5f,
                resistanceEau = 0.5f,
                resistanceVent = 0.5f,
                resistancePhysique = 0.75f,
                boostDamage = 1.0f,
                level = 1,
                listCapacities = new List<EntitiesCapacities>
                    {
                        new EntitiesCapacities
                        {
                            name = "Punch",
                            type = "Physique",
                            damage = 5.0f,
                            stamina = 20.0f,
                            speed = 20,
                            boostDamage = 1.0f,
                            criticalChance = 1.2f,
                            level = 0
                        },
                        new EntitiesCapacities
                        {
                            name = "Epee",
                            type = "Physique",
                            damage = 20.0f,
                            stamina = 20.0f,
                            speed = 20,
                            boostDamage = 1.0f,
                            criticalChance = 1.2f,
                            level = 0
                        }
                    },
                currentLevel = 1,
                currentStamina = 300,
            };

            entities.alliesList = new List<Allies> { allie };
            entities.enemiesList = new List<Enemy> { enemie };

            Assert.IsNotNull(entities.alliesList);
            Assert.IsNotNull(entities.enemiesList);

        }
        [Test]
        public void VerifyIfTakeDamage()
        {
            Allies allie = new Allies
            {
                name = "Ace",
                type = "Logia",
                maxhealth = 300,
                health = 300.0f,
                stamina = 200.0f,
                maxStamina = 200,
                speed = 40,
                resistanceFeu = 2.0f,
                resistanceEau = 0.5f,
                resistanceVent = 0.75f,
                resistancePhysique = 1.5f,
                boostDamage = 1.0f,
                level = 1,
                listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Chopped",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.4f,
                    level = 0
                },
                 new EntitiesCapacities
                {
                    name = "Fire Punch",
                    type = "Feu",
                    damage = 40.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Great Ring of Fire: Horse of the Sun",
                    type = "Feu",
                    damage = 80.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 0
                }
            },
                currentLevel = 1,
                currentStamina = 200,
            };

            allie.TakeDamage(20);
            Assert.AreEqual(280, allie.health);
        }
        [Test]
        public void VerifyIfGetHealth()
        {
            var entities = new EntityContainer();
            Allies allie1 = new Allies
            {
                name = "Ace",
                type = "Logia",
                maxhealth = 300,
                health = 300.0f,
                stamina = 200.0f,
                maxStamina = 200,
                speed = 40,
                resistanceFeu = 2.0f,
                resistanceEau = 0.5f,
                resistanceVent = 0.75f,
                resistancePhysique = 1.5f,
                boostDamage = 1.0f,
                level = 1,
                listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Chopped",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.4f,
                    level = 0
                },
                 new EntitiesCapacities
                {
                    name = "Fire Punch",
                    type = "Feu",
                    damage = 40.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Great Ring of Fire: Horse of the Sun",
                    type = "Feu",
                    damage = 80.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 0
                }
            },
                currentLevel = 1,
                currentStamina = 200,
            };
            Allies allies2 = new Allies
            {
                name = "Monkey D.Luffy",
                type = "Paramecia",
                maxhealth = 500,
                health = 500.0f,
                stamina = 300.0f,
                maxStamina = 300,
                speed = 50,
                resistanceFeu = 1.5f,
                resistanceEau = 0.5f,
                resistanceVent = 1.25f,
                resistancePhysique = 2.0f,
                boostDamage = 1.0f,
                level = 1,
                listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Jet Pistol",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 20.0f,
                    speed = 20,
                    boostDamage = 1.0f,
                    criticalChance = 1.4f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Red Hawk",
                    type = "Physique",
                    damage = 50.0f,
                    stamina = 100.0f,
                    speed = 70,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 2
                },
                new EntitiesCapacities
                {
                    name = "Gear 5",
                    type = "Physique",
                    damage = 60.0f,
                    stamina = 20.0f,
                    speed = 20,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 3
                },
                new EntitiesCapacities
                {
                    name = "Haki",
                    type = "Physique",
                    damage = 70.0f,
                    stamina = 20.0f,
                    speed = 20,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 5
                }
            },
                currentLevel = 1,
                currentStamina = 300,
            };

            entities.alliesList = new List<Allies> { allie1, allies2 };

            entities.alliesList[1].AddHealth(20);
            Assert.AreEqual(520, entities.alliesList[1].health);
        }
    }
}