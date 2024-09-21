using Godot;
using System.Linq;
using System.Collections.Generic;

public class Waves
{
    private static int waveIndex = 0;

    public static List<EnemyTemplate> NextWave()
    {
        waveIndex += 1;
        switch (waveIndex)
        {
            case 1:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 10; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    return enemies;
                }

            case 2:
                {
                    var enemies = new List<EnemyTemplate>(); 
                    for (var i = 0; i < 4; i++) { enemies.Add(EnemyTemplate.BASIC); }
                    for (var i = 0; i < 2; i++) { enemies.Add(EnemyTemplate.STRONG); }
                    return enemies;
            }
            default:
                {
                    var enemies = new List<EnemyTemplate>();
                    for (var i = 0; i < 50; i++) { enemies.Add(EnemyTemplate.QUICK); }
                    return enemies;
                }
        }
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