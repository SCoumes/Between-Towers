using Godot;
using System.Collections.Generic;

public partial class Spawner : Node2D
{
    [Export]
    public bool Active = true;

    private bool LeaveScreenNow = false;

    [Export]
    public int SpawnerSpeed = 100;

    [Export]
    public Game game;

    private Stack<EnemyTemplate> enemiesToSpawn = new();

    public double SpawnDuration = 2;
    public double durationLeft = 0;

    private PackedScene EnemyScene = GD.Load<PackedScene>("res://scenes/Enemy.tscn");

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

        durationLeft = SpawnDuration;

        var enemy = EnemyScene.Instantiate<Enemy>();
        enemy.Template = enemiesToSpawn.Pop();
        game.AddChild(enemy);
        enemy.Position = Position;

        if (enemiesToSpawn.Count == 0)
            LeaveScreenNow = true;
    }

    public void exitedScreen(){
        if (!LeaveScreenNow){
        SpawnerSpeed *= -1;
        } else {
            Active = false;
        }
    }

    public void SetWave(List<EnemyTemplate> enemies)
    {
        Position = Vector2.Zero;
        SpawnerSpeed = Waves.getSpawnerSpeed();
        enemiesToSpawn.Clear();
        enemies.Reverse();
        foreach (var enemy in enemies)
        {
            enemiesToSpawn.Push(enemy);
        }
    }
}
