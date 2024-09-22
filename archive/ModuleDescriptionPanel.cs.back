using Godot;
using System;

public partial class ModuleDescriptionPanel : PanelContainer
{
    [Signal]
    public delegate void ModifyModuleEventHandler(string action);

    [Export]
    Label moduleNameLabel;
    [Export]
    Label moduleLevelLabel;

    public void SetModuleDescription(Module module)
    {
        moduleNameLabel.Text = module.Name;
    }

    public void OnUpgradeButtonPressed()
    {

    }
    
    public void OnModuleButtonPressed(string action)
    {
        EmitSignal(SignalName.ModifyModule, action);
    }
}
