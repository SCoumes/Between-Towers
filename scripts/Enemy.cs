using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export]
    public ProgressBar HealthBar;
    [Export]
    private AnimationPlayer animationPlayer;


    public EnemyTemplate Template;

    public bool Dead => dead;

    private bool dead = false;
    private int currentHP;

    public override void _Ready()
    {
        HealthBar.MaxValue = Template.MaxHP;
        currentHP = Template.MaxHP;
        HealthBar.Value = currentHP;

        animationPlayer.Play("idle");
    }

    public override void _Process(double delta)
    {
        Translate(Template.Speed * (float)delta * Vector2.Down);
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
