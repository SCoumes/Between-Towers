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


    public Tower FocusTower => TowersComponent.GetChildren().Select(c => c as Tower).First(t => t.IsFocused);

    [Export]
    public GroundDetection groundDetection;
    
    public override void _Ready()
    {
        //SetupWaves();

        Waves.game = this;

        TowersComponent.GetChildren().Select(c => c as Tower).First().SetFocus();

        SetBuildButton(new TowerModule.ArcherModule());
        SetBuildButton(new TowerModule.MarksmanModule());
        SetBuildButton(new TowerModule.CannonModule());
        SetBuildButton(new TowerModule.MagicModule());

        moduleDescriptionPanel.ModifyModule += (string action) =>
        {
            switch (action)
            {
                case "DESTROY":
                    FocusTower.RemoveModuleAndAbove(focusModule);


                    return;
            }
        };
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
        button.Pressed += () =>
        {
            var cost = 5;

            if (Gold < cost)
                return;


            if (FocusTower.Modules.Count > 0)
            {
                if (template.Level > FocusTower.Modules.Last().Level)
                    return;
            }

            // Copy to avoid reflection
            TowerModule module = null;
            module = (TowerModule)template.GetType().GetConstructors()[0].Invoke(null);

            Gold -= cost;
            FocusTower.AddModule(module);
        };

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
