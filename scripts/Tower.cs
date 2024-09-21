using Godot;
using System;
using System.Collections.Generic;


public partial class Tower : Node2D
{

	public int size;
	public List<TowerModule> modules; // The size of this should always be equal to size
	// Called when the node enters the scene tree for the first time.

	private Game game;

 	[Export]
	private Sprite2D massonry;
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Upgrade the tower. This is in charge of checking gold cost and failing if there is not enough gold.
	/// </summary>
	public void UpgradeTower(){
		//upgrade tower
	}

	public void AddModule(String ModuleName, int ModuleIndex){
		// TODO: also check tower size and that the module index is on a build module
		TowerModule module;
		switch (ModuleName){
			case "Archer":
				module = new TowerModule.ArcherModule();
				break;
			case "Marksman":
				module = new TowerModule.MarksmanModule();
				break;
			case "Cannon":
				module = new TowerModule.CannonModule();
				break;
			case "Magic":
				module = new TowerModule.MagicModule();
				break;
			default:
				throw new Exception("Invalid module name");
		}
		if (Game.Gold < 5) {return;} // TODO: Change this to the actual cost of the module
		modules.Add(module);
		Game.Gold -= 5; // TODO: Change this to the actual cost of the module
		
		var shooter = ShooterScene.Instantiate<Shooter>();
        shooter.Position = masonryLayer.MapToLocal(moduleCoordinates) + masonryLayer.Position;
        shooter.Module = module;
        shooters.AddChild(shooter);
        shooter.SetDetectionRadius(module.Radius);

		DrawTower();
	}

	public void DrawTower(){
		//draw tower

	}
}
