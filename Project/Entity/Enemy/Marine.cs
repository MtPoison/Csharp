
using System.Runtime.CompilerServices;

class Marine : EntityAbstrac
{
    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {_name} Heal: {_heal}, Attack: {_attack}, Stamina: {_stamina}, Precision: {_precision}, Speed: {_speed}, Level: {_level}");
    }

    public override void AddHeal(int add)
    {
        _heal += add;
    }

    public override void TakeDamage(int damage)
    {
        _heal -= damage;
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
        if(_experience >= _maxExerience)
        {
            int tmp = _experience - _maxExerience;
            _experience = 0;
            _experience = tmp;
            _level++;
            _maxExerience = 100 * _level;
        }
    }
}
