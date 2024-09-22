using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class Game : Node2D
{
    [Export]
    public Node2D TowersComponent;
    [Export]
    public Label GoldCounter;
    [Export]
    private ModuleDescriptionPanel moduleDescriptionPanel;
    [Export]
    private Container buildButtonsContainer;
    [Export]
    private ButtonGroup UpgradeButtonGroup;
    [Export]
    private Spawner spawner;
    [Export]
    private Tower testTower;

    public static int Gold
    {
        get => gold;
        set { dirtyGold = true; gold = value; }
    }

    private static int gold = 999;
    private static bool dirtyGold = true;

    private static bool waveInProgress = false;

    public static int ActiveEnemies = 0;

    [Export]
    public GroundDetection groundDetection;

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


    public static Game game;

    public override void _Ready()
    {
        //SetupWaves();

        SetBuildButton("Build");
        SetBuildButton("Archer");
        SetBuildButton("Upgrade");

        game = this;

        tower3.UpgradeTower();
    }

    public override void _Process(double delta)
    {
        if (dirtyGold)
        {
            GoldCounter.Text = Gold.ToString();
            dirtyGold = false;
        }

        if (waveInProgress && ActiveEnemies == 0 && Waves.DoneSpawning())
        {
            GD.Print("Wave finished !");
            Gold += 10;
            waveInProgress = false;
        }
    }

    public void OnTowerButtonPressed()
    {
    }

    public void SetBuildButton(String name)
    {
        var button = new Button();
        button.Text = name;
        if (name == "Upgrade")
        {
            button.Pressed += () => {
                testTower.UpgradeTower();
            };
        }
        else {
            button.Pressed += () => {
                testTower.AddModule(name, testTower.size-1);
            };
        }

        buildButtonsContainer.AddChild(button);
    }

    public void OnNextWaveButtonPressed()
    {
        if (!waveInProgress){
            Waves.NextWave();
            waveInProgress = true;
        }
    }

    public void OnTowerModuleButtonPressed(Tower tower, int moduleIndex)
    {
        Button button = UpgradeButtonGroup.GetPressedButton() as Button;
        tower.OnModuleCliked(button.Text, moduleIndex);
    }
}


