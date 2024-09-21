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

    private bool waveInProgress = false;

    private int waveIndex = 0;
    private Queue<List<EnemyTemplate>> Waves;

    public Tower FocusTower => TowersComponent.GetChildren().Select(c => c as Tower).First(t => t.IsFocused);

    public override void _Ready()
    {
        SetupWaves();

        (TowersComponent.GetChild(0) as Tower).AddModule(null);
        (TowersComponent.GetChild(1) as Tower).AddModule(new TowerModule.PrincessModule());
        (TowersComponent.GetChild(2) as Tower).AddModule(null);
        TowersComponent.GetChildren().Select(c => c as Tower).First().SetFocus();

        SetBuildButton(null);
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

        if (waveInProgress && !spawner.Active && spawner.GetChildCount() == 0)
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
        button.Text = template == null ? "PLAIN_WALL" : template.Name;
        button.Pressed += () =>
        {
            var cost = template == null ? 2 : 5;

            if (Gold < cost)
                return;

            TowerModule module = null;
            if (template != null)
            {

                if (FocusTower.Modules.Count > 0)
                {
                    if (template.Level > FocusTower.Modules.Last().Level)
                        return;
                }

                // Copy to avoid reflection
                module = (TowerModule)template.GetType().GetConstructors()[0].Invoke(null);
            }

            Gold -= cost;
            FocusTower.AddModule(module);
        };

        buildButtonsContainer.AddChild(button);
    }

    public void OnNextWaveButtonPressed()
    {
        waveIndex++;
        spawner.SetWave(Waves.Dequeue());
        spawner.Active = true;
        waveInProgress = true;
    }

    private void SetupWaves()
    {
        Waves = new();

        var Wave1 = new List<EnemyTemplate>();
        for (var i = 0; i < 10; i++) { Wave1.Add(EnemyTemplate.BASIC); }
        Waves.Enqueue(Wave1);


        var Wave2 = new List<EnemyTemplate>();
        for (var j = 0; j < 4; j++)
        {
            for (var i = 0; i < 4; i++) { Wave2.Add(EnemyTemplate.BASIC); }
            for (var i = 0; i < 2; i++) { Wave2.Add(EnemyTemplate.STRONG); }

        }
        Waves.Enqueue(Wave2);

        var Wave3 = new List<EnemyTemplate>();
        for (var i = 0; i < 50; i++) { Wave3.Add(EnemyTemplate.QUICK); }
        Waves.Enqueue(Wave3);

    }
}
