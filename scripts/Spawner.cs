using Godot;
using System.Collections.Generic;

public partial class Spawner : Node2D
{
    [Export]
    public bool Active = true;

    private Stack<EnemyTemplate> enemiesToSpawn = new();

    public double SpawnDuration = 2;
    public double durationLeft = 0;

    private PackedScene EnemyScene = GD.Load<PackedScene>("res://scenes/Enemy.tscn");

    public override void _Process(double delta)
    {
        if (!Active)
            return;

        if (durationLeft > 0)
        {
            durationLeft -= delta;
            return;
        }

        durationLeft = SpawnDuration;

        var enemy = EnemyScene.Instantiate<Enemy>();
        enemy.Template = enemiesToSpawn.Pop();
        AddChild(enemy);

        if (enemiesToSpawn.Count == 0)
            Active = false;
    }

    public void SetWave(List<EnemyTemplate> enemies)
    {
        enemiesToSpawn.Clear();
        enemies.Reverse();
        foreach (var enemy in enemies)
        {
            enemiesToSpawn.Push(enemy);
        }
    }
}
