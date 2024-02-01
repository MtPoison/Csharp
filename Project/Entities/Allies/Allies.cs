
using Newtonsoft.Json;

public class Allies : EntityAbstract
{
    public List<AlliesCapacity> _ListCapacities { get; set; } = new List<AlliesCapacity>();
    private Allies allies;
    private Allies allies2;
    public AlliesContainer alliesContainer;
    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {_name} Health: {_health}, Stamina: {_stamina}, Speed: {_speed}, Level: {_level}");
    }

    public override void SetStatsEntity(EntityAbstract entity)
    {
        _name = entity._name;
        _health = entity._health;
        _stamina = entity._stamina;
        _speed = entity._speed;
        _level = entity._level;
    }

    public override void CreateEntity(string path)
    {
        allies = new Allies
        {
            _name = "Jimbey",
            _health = 300,
            _stamina = 200,
            _speed = 40,
            _level = 1,
            _ListCapacities = new List<AlliesCapacity>
            {
                new AlliesCapacity
                {
                    _name = "Haki",
                    _stamina = 10,
                    _speed = 5,
                    _level = 1
                },
                new AlliesCapacity
                {
                    _name = "Paladin",
                    _stamina = 10,
                    _speed = 5,
                    _level = 1
                }

            },
        };

        allies2 = new Allies
        {
            _name = "Luffy",
            _health = 500,
            _stamina = 300,
            _speed = 50,
            _level = 1,
            _ListCapacities = new List<AlliesCapacity>
            {
                new AlliesCapacity
                {
                    _name = "Haki",
                    _stamina = 20,
                    _speed = 20,
                    _level = 2
                },
                new AlliesCapacity
                {
                    _name = "Gear 5",
                    _stamina = 100,
                    _speed = 70,
                    _level = 5
                }

            },
        };

        alliesContainer = new AlliesContainer
        {
            Allies1 = allies,
            Allies2 = allies2
        };

        string json = JsonConvert.SerializeObject(alliesContainer);
        File.WriteAllText(path, json);
        Console.WriteLine("Données sauvegardées dans le fichier : " + path);
    }

    public override void GetInfoEntity(string path)
    {
        string json = File.ReadAllText(path);
        alliesContainer = JsonConvert.DeserializeObject<AlliesContainer>(json);
    }

    public override void AddHealth(int add)
    {
        _health += add;
    }

    public override void TakeDamage(int damage)
    {
        _health -= damage;
    }

    public override void AddStamina(int add)
    {
        _stamina += add;
    }

    public override void LessStamina(int less)
    {
        _stamina -= less;
    }
    public override void AddExperience(int add)
    {
        _experience += add;
        AddLevel();
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
            Console.WriteLine($"Tu as monter de nv {_level} : {_experience}/{_maxExerience} ");
        }
    }
}
