using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class Game : Node
{
    [Export]
    public Node2D TowersComponent;
    [Export]
    public Label GoldCounter;
    [Export]
    private Container buildButtonsContainer;
    [Export]
    private ButtonGroup UpgradeButtonGroup;
    /*[Export]
    private Spawner spawner;*/
    [Export]
    private Tower testTower;
    [Export]
    public Node EnemiesComponent;

    public static int Gold
    {
        get => gold;
        set { dirtyGold = true; gold = value; }
    }

    private static int gold;
    private static bool dirtyGold = true;

    private static bool waveInProgress = false;

    public static int ActiveEnemies = 0;

    [Export]
    public GroundDetection groundDetection;

    [Export]
    public Label WindValue;

    [Export]
    public Tower tower1;

    [Export]
    public Tower tower2;

    [Export]
    public Tower tower3;

    [Export]
    public Tower tower4;

    [Export]
    public Tower tower5;

    [Export]
    private AudioStreamPlayer buildupMusicPlayer;
    [Export]
    private AudioStreamPlayer battleMusicPlayer;

    [Export]
    private Label playerHealthLabel;
    [Export]
    private Control gameOverDisplay;
    [Export]
    private Control gameWonDisplay;
    [Export]
    public Label CurrentWaveLabel;


    public int PlayerHealth{
        get => playerHealth;
        set { dirtyHealth = true; playerHealth = value; }
    }

    private int playerHealth;
    private bool dirtyHealth = true;

    public static Game game;

    public override void _Ready()
    {
        //SetupWaves();

        game = this;

        StartGame();
    }

    public override void _Process(double delta)
    {
        if (dirtyGold)
        {
            GoldCounter.Text = Gold.ToString();
            dirtyGold = false;
        }

        if (dirtyHealth)
        {
            playerHealthLabel.Text = PlayerHealth.ToString();

            if (PlayerHealth == 0)
            {
                GameOver();
            }

            dirtyHealth = false;
        }

        if (waveInProgress && ActiveEnemies == 0 && Waves.DoneSpawning())
        {
            battleMusicPlayer.Stop();
            buildupMusicPlayer.Play();
            Waves.EndWave();
            waveInProgress = false;
        }
    }

    public void OnNextWaveButtonPressed()
    {
        if (!waveInProgress){
            buildupMusicPlayer.Stop();
            battleMusicPlayer.Play();
            Waves.NextWave();
            waveInProgress = true;
        }
    }

    public void OnTowerModuleButtonPressed(Tower tower, int moduleIndex)
    {
        Button button = UpgradeButtonGroup.GetPressedButton() as Button;
        tower.OnModuleCliked(button.Text, moduleIndex);
    }

    public void GameWon()
    {
        gameWonDisplay.Visible = true;
    }

    public void InfiniteMode()
    {
        gameWonDisplay.Visible = false;
    }

    public void GameOver()
    {
        gameOverDisplay.Visible = true;
        Waves.EndWave();

        buildupMusicPlayer.Stop();
        battleMusicPlayer.Stop();
        
        foreach (var enemy in EnemiesComponent.GetChildren())
        {
            enemy.CallDeferred(Enemy.MethodName.QueueFree);
        }
        GD.Print("Game Over");
    }

    public void StartGame()
    {
        gameOverDisplay.Visible = false;
        gameWonDisplay.Visible = false;

        Gold = 30;
        PlayerHealth = 15;

        Waves.WaveIndex = 0;

        tower1.ResetTower();
        tower2.ResetTower();
        tower3.ResetTower();
        tower4.ResetTower();
        tower5.ResetTower();

        tower3.UpgradeTower();

        buildupMusicPlayer.Play();
    }
}


