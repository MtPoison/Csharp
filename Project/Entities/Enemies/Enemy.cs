
using MapEntities;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

public class Enemy : EntityAbstract
{
    Random random = new Random();
    public string _difficultyIA;
    public string AI_Level { get; set; }

    public void InitializeAI(string aiLevel)
    {
        AI_Level = aiLevel;
        // Initialiser l'IA basée sur aiLevel...
    }

    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {_name} Health: {_health}, Stamina: {_stamina}, Speed: {_speed}, Level: {_level}");
    }

    public override void AddHealth(int add)
    {
        _health += add;
    }

    public override void TakeDamage(float damage)
    {
        _health -= damage;
    }

    public override void AddStamina(int add)
    {
        _stamina += add;
    }

    public override void LessStamina(float less)
    {
        _stamina -= less;
    }
    public override void AddExperience(int add)
    {
        _experience += add;
    }

    public override void AddLevel()
    {
        if (_experience >= _maxExerience)
        {
            int tmp = _experience - _maxExerience;
            _experience = 0;
            _experience = tmp;
            _level++;
            _maxExerience = 100 * _level;
        }
    }

    public override void Loot(Player p)
    {
        int nbLoot = random.Next(1, 3);
        if (_name == "Marine")
        {
            p.AddAlcool(nbLoot);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\tTu as recuperé {nbLoot} bouteille(s) d'alcool(s)");
            Console.ResetColor();
        }
        if (_name == "Sanglier")
        {
            p.AddViande(nbLoot);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\tTu as recuperé {nbLoot} viande(s)");
            Console.ResetColor();
        }
    }
}
