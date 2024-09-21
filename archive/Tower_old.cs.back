using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Tower : Node2D
{
    public List<TowerModule> Modules = new();

    [Export]
    public TileMapLayer masonryLayer;
    [Export]
    public TileMapLayer moduleLayer;
    [Export]
    public Node2D shooters;
    [Export]
    public Button TowerButton;

    private PackedScene ShooterScene = GD.Load<PackedScene>("res://scenes/Shooter.tscn");
    private static ButtonGroup towerButtonGroup = GD.Load<ButtonGroup>("res://resources/TowersSelectionButtonGroup.tres");

    public bool IsFocused => TowerButton.ButtonPressed;

    public override void _Ready()
    {
        //Don't know why but they end up distints if set in the editor
        TowerButton.ButtonGroup = towerButtonGroup;
        masonryLayer.Clear();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {

            if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == MouseButton.Left)
            {
                //TODO IF VIEWPORT MOVES
                var clickCoordinates = moduleLayer.LocalToMap(moduleLayer.ToLocal(mouseButtonEvent.GlobalPosition));

                if (clickCoordinates.X != 0 || clickCoordinates.Y > 0)
                    return;

                Game.FocusModule = Modules.ElementAt(-clickCoordinates.Y);
            }
        }
    }

    public void AddModule(TowerModule module)
    {
        var moduleCoordinates = new Vector2I(0, -Modules.Count);

        masonryLayer.SetCell(moduleCoordinates, 0, Modules.Count == 0 ? Vector2I.Zero : Vector2I.Right) ;
        Modules.Add(module);

        if (module == null)
            return;

        moduleLayer.SetCell(moduleCoordinates, 0, module.TextureCoordinates);

        var shooter = ShooterScene.Instantiate<Shooter>();
        shooter.Position = masonryLayer.MapToLocal(moduleCoordinates) + masonryLayer.Position;
        shooter.Module = module;
        shooters.AddChild(shooter);
        shooter.SetDetectionRadius(module.Radius);
    }

    public void RemoveModuleAndAbove(TowerModule module)
    {
        var moduleIndex = Modules.IndexOf(module);

        for (var i = Modules.Count - 1; i >= moduleIndex; i--)
        {
            masonryLayer.SetCell(i * Vector2I.Up);
            moduleLayer.SetCell(i * Vector2I.Up);
        }

        foreach (var shooter in shooters.GetChildren())
        {
            var shooterCoordinates = moduleLayer.LocalToMap((shooter as Shooter).Position);

            // Assumes same local position for shooters
            if (-shooterCoordinates.Y >= moduleIndex)
                shooter.CallDeferred(MethodName.QueueFree);
        }

        Modules = Modules.Take(moduleIndex).ToList();
    }

    public void SetFocus()
    {
        TowerButton.ButtonPressed = true;
    }
}