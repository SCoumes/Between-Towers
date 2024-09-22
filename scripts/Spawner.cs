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
    private double delay;

    [Export] 
    public Sprite2D sprite;


    private bool LeaveScreenNow = false;
    private Stack<EnemyTemplate> enemiesToSpawn = new();
    private int minRange;
    private int maxRange;

    public double SpawnDuration = 0.4; // Time window between two spawns
    public double durationLeft = 0; // Time left to spawn next enemy
    public Vector2 StartingPosition;

    private PackedScene EnemyScene = GD.Load<PackedScene>("res://scenes/Enemy.tscn");

    public override void _Ready()
    {
        animationPlayer.Play("fly");
    }

    public override void _Process(double delta)
    {
        // Delay before launching the dragon
        if (delay > 0)
        {
            delay -= delta;
            return;
        }
        if (!Active)
            return;
        Position += (float)delta * SpawnerSpeed * Vector2.Right;

        // Delay before consecutive drops
        if (durationLeft > 0)
        {
            durationLeft -= delta;
            return;
        }

        if ((Position.X < minRange -100 && SpawnerSpeed < 0)  || (Position.X > maxRange + 100 && SpawnerSpeed > 0))
        {
            exitedScreen();
            return;
        }

        if (Position.X < minRange || Position.X > maxRange)
        {
            return;
        }

        durationLeft = SpawnDuration;

        if (enemiesToSpawn.Count == 0){
            LeaveScreenNow = true;
        } else {
            var enemy = EnemyScene.Instantiate<Enemy>();
            enemy.Template = enemiesToSpawn.Pop();
            Game.game.EnemiesComponent.AddChild(enemy);
            Game.ActiveEnemies++;
            enemy.Position = Position;
        }

        
    }

    public void exitedScreen(){
        if (!LeaveScreenNow)
        {
            SpawnerSpeed *= -1;
            sprite.FlipH = !sprite.FlipH;
        } 
        else 
        {
            Position = 100 * Vector2.Left;
            Active = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemies"></param>
    /// <param name="minRange"></param>
    /// <param name="maxRange"></param>
    /// <param name="delay"> Time before launch of dragon</param>
    public void SetWave(List<EnemyTemplate> enemies, int minRange, int maxRange, double delay)
    {
        Active = true;
        Position = StartingPosition;
        SpawnerSpeed = Waves.getSpawnerSpeed();
        this.delay = delay;
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
