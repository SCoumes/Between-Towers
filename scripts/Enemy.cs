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
    private Vector2 velocity;
    public override void _Ready()
    {
        HealthBar.MaxValue = Template.MaxHP;
        currentHP = Template.MaxHP;
        HealthBar.Value = currentHP;

        velocity = Template.Speed * Vector2.Down;
        animationPlayer.Play("idle");
    }

    public override void _Process(double delta)
    {
        velocity += (float)delta * Waves.getWind();
        Translate((float)delta * velocity);
    }

    public void Damage(int damage)
    {
        currentHP -= damage;
        HealthBar.Value = currentHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void exitedScreen()
    {
        Die();
    }

    public void Die(){
            dead = true;
            Game.Gold += 1;
            Game.ActiveEnemies -= 1;
            CallDeferred(MethodName.QueueFree);
 
    }
}
