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
    private Spawner spawner;

    public static int Gold
    {
        get => gold;
        set { dirtyGold = true; gold = value; }
    }

    public static TowerModule FocusModule
    {
        get => focusModule;
        set { dirtyModule = true; focusModule = value; }
    }

    private static int gold = 999;
    private static bool dirtyGold = true;
    private static TowerModule focusModule = null;
    private static bool dirtyModule = false;

    private static bool waveInProgress = false;

    public static int ActiveEnemies = 0;

    [Export]
    public GroundDetection groundDetection;
    
    public override void _Ready()
    {
        //SetupWaves();

        Waves.game = this;

        SetBuildButton(new TowerModule.ArcherModule());
        SetBuildButton(new TowerModule.MarksmanModule());
        SetBuildButton(new TowerModule.CannonModule());
        SetBuildButton(new TowerModule.MagicModule());

    }

    public override void _Process(double delta)
    {
        if (dirtyGold)
        {
            GoldCounter.Text = Gold.ToString();
            dirtyGold = false;
        }

        if (dirtyModule)
        {
            moduleDescriptionPanel.SetModuleDescription(focusModule);
            dirtyModule = true;
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

    public void SetBuildButton(TowerModule template)
    {
        var button = new Button();
        button.Text = template.Name;
        button.Pressed += () => {};

        buildButtonsContainer.AddChild(button);
    }

    public void OnNextWaveButtonPressed()
    {
        if (!waveInProgress){
            Waves.NextWave();
            waveInProgress = true;
        }
    }


}
