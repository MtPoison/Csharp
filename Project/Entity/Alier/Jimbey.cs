﻿
public class Jimbey : EntityAbstrac
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
}
