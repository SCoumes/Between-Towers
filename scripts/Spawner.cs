using Godot;
using System.Collections.Generic;

public partial class Spawner : Node2D
{
    [Export]
    public bool Active = true;

    [Export]
    public int SpawnerSpeed = 100;

    [Export]
    public Game game;
    [Export]
    public AnimationPlayer animationPlayer;
    [Export] 
    public Sprite2D sprite;


    private bool LeaveScreenNow = false;
    private Stack<EnemyTemplate> enemiesToSpawn = new();
    private int minRange;
    private int maxRange;

    public double SpawnDuration = 1; // Time window between two spawns
    public double durationLeft = 0; // Time left to spawn next enemy
    public Vector2 StartingPosition = Vector2.Zero;

    private PackedScene EnemyScene = GD.Load<PackedScene>("res://scenes/Enemy.tscn");

    public override void _Ready()
    {
        animationPlayer.Play("fly");
    }

    public override void _Process(double delta)
    {

        if (!Active)
            return;
        Position += (float)delta * SpawnerSpeed * Vector2.Right;

        if (durationLeft > 0)
        {
            durationLeft -= delta;
            return;
        }

        if (Position.X < minRange || Position.X > maxRange)
        {
            return;
        }

        durationLeft = SpawnDuration;

        var enemy = EnemyScene.Instantiate<Enemy>();
        enemy.Template = enemiesToSpawn.Pop();
        game.AddChild(enemy);
        Game.ActiveEnemies++;
        enemy.Position = Position;

        if (enemiesToSpawn.Count == 0)
            LeaveScreenNow = true;
    }

    public void exitedScreen(){
        if (!LeaveScreenNow)
        {
            SpawnerSpeed *= -1;
            sprite.FlipH = !sprite.FlipH;
        } 
        else 
        {
            Active = false;
        }
    }

    public void SetWave(List<EnemyTemplate> enemies, int minRange, int maxRange)
    {
        Active = true;
        Position = StartingPosition;
        SpawnerSpeed = Waves.getSpawnerSpeed();
        enemiesToSpawn.Clear();
        enemies.Reverse();
        foreach (var enemy in enemies)
        {
            enemiesToSpawn.Push(enemy);
        }
        this.minRange = minRange;
        this.maxRange = maxRange;
    }
}
