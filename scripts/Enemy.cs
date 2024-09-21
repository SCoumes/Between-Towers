using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export]
    public ProgressBar HealthBar;

    public EnemyTemplate Template;

    public bool Dead => dead;

    private bool dead = false;
    private int currentHP;
    private Vector2 velocity;

    public override void _Ready()
    {
        HealthBar.MaxValue = Template.MaxHP;
        currentHP = Template.MaxHP;
        HealthBar.Value = currentHP;
        velocity = Template.Speed * Vector2.Down;
    }

    public override void _Process(double delta)
    {
        velocity = Wind.Blow(velocity, delta); 
        Translate((float)delta * velocity);
    }

    public void Damage(int damage)
    {
        currentHP -= damage;
        HealthBar.Value = currentHP;

        if (currentHP <= 0)
        {
            dead = true;
            Game.Gold += 1;
            CallDeferred(MethodName.QueueFree);
        }
    }
}
