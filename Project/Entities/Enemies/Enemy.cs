
using Newtonsoft.Json;

public class Enemy : EntityAbstract
{
    public List<EnemyCapacity> _ListCapacities { get; set; } = new List<EnemyCapacity>();
    private Enemy enemy;
    private Enemy enemy2;
    public EnemyContainer enemyContainer;

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
        enemy = new Enemy
        {
            _name = "Soldat Marine 1",
            _health = 200,
            _stamina = 100,
            _speed = 30,
            _level = 1,
            _ListCapacities = new List<EnemyCapacity>
            {
                new EnemyCapacity
                {
                    _name = "Epee",
                    _stamina = 5,
                    _speed = 5,
                    _level = 0
                },
                new EnemyCapacity
                {
                    _name = "Pistolet",
                    _stamina = 5,
                    _speed = 5,
                    _level = 0
                }

            },
        };

        enemy2 = new Enemy
        {
            _name = "Soldat Marine 2",
            _health = 200,
            _stamina = 100,
            _speed = 30,
            _level = 1,
            _ListCapacities = new List<EnemyCapacity>
            {
                new EnemyCapacity
                {
                    _name = "Epee",
                    _stamina = 5,
                    _speed = 5,
                    _level = 1
                },
                new EnemyCapacity
                {
                    _name = "Pistolet",
                    _stamina = 5,
                    _speed = 5,
                    _level = 1
                }

            },
        };

        enemyContainer = new EnemyContainer
        {
            Enemy1 = enemy,
            Enemy2 = enemy2
        };

        string json = JsonConvert.SerializeObject(enemyContainer);
        File.WriteAllText(path, json);
        Console.WriteLine("Données sauvegardées dans le fichier : " + path);
    }

    public override void GetInfoEntity(string path)
    {
        string json = File.ReadAllText(path);
        enemyContainer = JsonConvert.DeserializeObject<EnemyContainer>(json);
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
}
