﻿
public abstract class EntityAbstrac
{
    public int _heal;
    public int _attack;
    public int _stamina;
    public int _precision;
    public int _speed;
    public int _level;

    public abstract void DisplayDetails();
    public abstract void AddHeal(int add);
    public abstract void AddStamina(int add);
    public abstract void lessStamina(int less);
    public abstract void TakeDamage(int less);
}

