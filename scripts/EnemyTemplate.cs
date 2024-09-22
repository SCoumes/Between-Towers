using Godot;
using System;

public class EnemyTemplate
{
    public int MaxHP;
    public float Speed;
    public int HPCost;
    public int GoldValue;
    public Color Modulate = Colors.White;

    public static readonly EnemyTemplate BASIC = new()
    {
        MaxHP = 6,
        Speed = 150,
        HPCost = 1,
        GoldValue = 1
    };

    public static readonly EnemyTemplate QUICK = new()
    {
        MaxHP = 6,
        Speed = 300,
        HPCost = 1,
        GoldValue = 2
    };

    public static readonly EnemyTemplate STRONG = new()
    {
        MaxHP = 25,
        Speed = 100,
        HPCost = 1,
        GoldValue = 5,
        Modulate = Colors.Red,
    };
}
