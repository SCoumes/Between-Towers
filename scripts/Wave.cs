using Godot;
using System.Linq;
using System.Collections.Generic;

public class Waves
{
    private static int waveIndex = 0;
    private static List<Spawner> spawners = new();


    public static Game game;

    public static void NextWave(){
        waveIndex += 1;
        List<List<EnemyTemplate>> EnemiesLists = _GetEnemies(waveIndex);
        List<int[]> Ranges = _getRanges(waveIndex); // Same toplevel list size as EnemiesLists, each array has 2 elements
        foreach (var enemies in EnemiesLists)
        {
            _SpawnDragon(enemies, Ranges[EnemiesLists.IndexOf(enemies)]);
        }
    }

    /// <summary>
    /// Checks if all the enemies have been spawned.
    /// </summary>
    /// <returns></returns>
    public static bool DoneSpawning()
    {
        return spawners.All(spawner => !spawner.Active);
    }

    private static void _cleanSpawners()
    {
        foreach (var spawner in spawners)
        {
            spawner.QueueFree();
        }
        spawners.Clear();
    }

    private static List<List<EnemyTemplate>> _GetEnemies(int IndexNumber)
    {
        waveIndex += 1;
        switch (IndexNumber)
        {
            case 1:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 10; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    return new(){enemies};
                }

            case 2:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 4; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    for (var i = 0; i < 2; i++) { enemies.Add(EnemyTemplate.STRONG); }
                    return new(){enemies};
            }
            default:
                {
                    var enemies = new List<EnemyTemplate>();
                    for (var i = 0; i < 50; i++) { enemies.Add(EnemyTemplate.QUICK); }
                    return new(){enemies};
                }
        }
    }

    private static List<int[]> _getRanges(int IndexNumber)
    {
        switch (IndexNumber)
        {
            case 1:
                return new() { new int[2] {100, 800} };
            case 2:
                return new() { new int[2] {100, 800} };
            default:
                return new() { new int[2] {100, 800} };
        }
    }

    private static void _SpawnDragon(List<EnemyTemplate> enemies, int[] range)
    {
        int min = range[0];
        int max = range[1];
        PackedScene SceneOfSpawner = GD.Load<PackedScene>("res://scenes/Spawner.tscn");
        var spawner = SceneOfSpawner.Instantiate<Spawner>();
        game.AddChild(spawner);
        spawner.game = game;
        spawner.SetWave(enemies, min, max);
        spawners.Add(spawner);
    }

    public static Vector2 getWind()
    {
        switch (waveIndex)
        {
            case 1:
                return Vector2.Left ;
            case 2:
                return Vector2.Right;
            default:
                return 2 * Vector2.Right;
        }
    }

    public static int getSpawnerSpeed()
    {
        switch (waveIndex)
        {
            case 1:
                return 100;
            case 2:
                return 100;
            default:
                return 50;
        }
    }
}

    // public static Waves  
    // public static void SetupWaves()
    // {
    //     Waves = new();

    //     var Wave1 = new List<EnemyTemplate>();
    //     for (var i = 0; i < 10; i++) { Wave1.Add(EnemyTemplate.BASIC); }
    //     Waves.Enqueue(Wave1);


    //     var Wave2 = new List<EnemyTemplate>();
    //     for (var j = 0; j < 4; j++)
    //     {
    //         for (var i = 0; i < 4; i++) { Wave2.Add(EnemyTemplate.BASIC); }
    //         for (var i = 0; i < 2; i++) { Wave2.Add(EnemyTemplate.STRONG); }

    //     }
    //     Waves.Enqueue(Wave2);

    //     var Wave3 = new List<EnemyTemplate>();
    //     for (var i = 0; i < 50; i++) { Wave3.Add(EnemyTemplate.QUICK); }
    //     Waves.Enqueue(Wave3);

    // }