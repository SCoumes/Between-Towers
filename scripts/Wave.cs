using Godot;
using System.Linq;
using System.Collections.Generic;

public class Waves
{
    private static int waveIndex = 15;
    private static List<Spawner> spawners = new();

    public static void NextWave(){
        waveIndex += 1;
        List<List<EnemyTemplate>> EnemiesLists = _GetEnemies(waveIndex);
        List<int[]> Ranges = _getRanges(waveIndex); // Same toplevel list size as EnemiesLists, each array has 2 elements
        double delay = 0.0;
        GD.Print(EnemiesLists);
        foreach (var enemies in EnemiesLists)
        {
            GD.Print(enemies);
            _SpawnDragon(enemies, Ranges[EnemiesLists.IndexOf(enemies)], delay);
            delay += GetDelay(waveIndex);
        }
        _SetWind();
    }

    public static void EndWave(){
        _cleanSpawners();
        _towerUlocks(waveIndex);
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
    private static void _SpawnDragon(List<EnemyTemplate> enemies, int[] range, double delay)
    {
        int min = range[0];
        int max = range[1];
        PackedScene SceneOfSpawner = GD.Load<PackedScene>("res://scenes/Spawner.tscn");
        var spawner = SceneOfSpawner.Instantiate<Spawner>();
        Game.game.AddChild(spawner);
        spawner.StartingPosition = new Vector2(-64, 30);
        spawner.game = Game.game;
        spawner.SetWave(enemies, min, max, delay);
        spawners.Add(spawner);
    }

    private static void _SetWind()
    {
        Game.game.WindValue.Text = getWindSpeed().ToString();
    }
    private static void _towerUlocks(int IndexNumber)
    {
        switch (IndexNumber)
        {
            case 2:
                Game.game.tower2.UpgradeTower();
                Game.game.tower4.UpgradeTower();
                break;
            case 4:
                Game.game.tower1.UpgradeTower();
                Game.game.tower5.UpgradeTower();
                break;
            default:
                break;
        }
    }   
    private static List<List<EnemyTemplate>> _GetEnemies(int IndexNumber, bool debug = false)
    {
        if (debug) { return new(){new(){EnemyTemplate.BASIC}}; }
        switch (IndexNumber)
        {
            case 1:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 6; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    return new(){enemies};
                }

            case 2:{
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 6; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    return new(){enemies};
            }

            case 3:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 2; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    var enemies2 = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 2; i++) { enemies2.Add(EnemyTemplate.BASIC); }
                    var enemies3 = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 6; i++) { enemies3.Add(EnemyTemplate.BASIC); }
                    return new(){enemies, enemies2, enemies3};
            }
            case 4:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 20; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    var enemies2 = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 20; i++) { enemies2.Add(EnemyTemplate.BASIC); }
                    return new(){enemies, enemies2};
            }
            case 5:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 50; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    return new(){enemies};
            }
            case 6:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 50; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    return new(){enemies};
            }
            case 7:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 50; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    var enemies2 = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 4; i++) { enemies.Add(EnemyTemplate.STRONG); }
                    return new(){enemies, enemies2};
            }
            default:
                {
                    List<List<EnemyTemplate>> result = new();
                    for (var i = 0; i < IndexNumber/3; i++)
                    {
                        var enemies = new List<EnemyTemplate>();
                        if (i % 2 == 0)
                        {
                            for (var j = 0; j < IndexNumber; j++) { enemies.Add(EnemyTemplate.BASIC); }
                        }
                        else
                        {
                            for (var j = 0; j < IndexNumber/5; j++) { enemies.Add(EnemyTemplate.STRONG); }
                        }
                        result.Add(enemies);
                    }
                    return result;
                }
        }
    }

    private static List<int[]> _getRanges(int IndexNumber)
    {
        switch (IndexNumber)
        {
            case 1:
                return new() { new int[2] {500, 600} };
            case 2:
                return new() { new int[2] {300, 400} };
            case 3:
                return new() { new int[2] {300, 400}, new int[2] {800, 900}, new int[2] {500, 600} };
            case 4:
                return new() { new int[2] {350, 450}, new int[2] {750, 850}};
            case 5:
                return new() { new int[2] {100, 1050} };
            case 6:
                return new() { new int[2] {100, 950} };
            default:
            {
                List<int[]> result = new();
                for (var i = 0; i < IndexNumber/3; i++)
                {
                    result.Add(new int[2] {100, 950});
                }
                return result;
            }
        }
    }

    private static int getWindSpeed()
    {
        switch (waveIndex)
        {
            case 1:
                return 0;
            case 2:
                return 30;
            case 3:
                return 0;
            case 4:
                return 0;
            case 5:
                return 0;
            case 6:
                return 50;
            default:
                return 50 - (20 * (waveIndex%5));
        }
    }

    /// <summary>
    /// Decides the delay between two consecutive spawners for a given wave.
    /// </summary>
    /// <param name="IndexNumber"></param>
    /// <returns> </returns>
    private static double GetDelay(int IndexNumber){
        switch (IndexNumber)
        {
            case 1:
                return 3.0;
            case 2:
                return 3.0;
            case 3:
                return 3.0;
            case 4:
                return 6.0;
            case 5:
                return 3.0;
            default:
                return 6.0;
        }
    }

    public static Vector2 getWind()
    {
        return getWindSpeed() * Vector2.Right;
    }

    public static int getSpawnerSpeed()
    {
        switch (waveIndex)
        {
            case 3:
                return 100;
            case 5:
                return 150;
            default:
                return 100;
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