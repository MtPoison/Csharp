
public abstract class EntityAbstrac
{
    public string _name;
    public int _heal;
    public int _attack;
    public int _stamina;
    public int _precision;
    public int _speed;
    public int _level;
    public int _experience;
    public int _maxExerience;

    public abstract void DisplayDetails();
    public abstract void AddHeal(int add);
    public abstract void AddStamina(int add);
    public abstract void AddLevel();
    public abstract void AddExperience(int add);
    public abstract void LessStamina(int less);
    public abstract void TakeDamage(int less);
}

