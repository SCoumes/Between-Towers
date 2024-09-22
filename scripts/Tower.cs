using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;


public partial class Tower : Node2D
{

	public int size=0;
	public List<Module> modules = new(); // The size of this should always be equal to size. So the highest module has index size-1
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
		size++;
		modules.Add(null);
		AddModule("Build", size-1);
	}

	public void AddModule(String ModuleName, int ModuleIndex){
		// TODO: also check tower size and that the module index is on a build module
		Module module;
		int Cost;
		switch (ModuleName){
			case "Build":
				PackedScene ModuleScene = GD.Load<PackedScene>("res://scenes/Modules/build_module.tscn");
				module = ModuleScene.Instantiate<BuildModule>();
				Cost = BuildModule.Cost;
				break;
			case "Archer":
				PackedScene ArcherScene = GD.Load<PackedScene>("res://scenes/Modules/archer_module.tscn");
				module = ArcherScene.Instantiate<ArcherModule>();
				Cost = ArcherModule.Cost;
				break;
			default:
				throw new Exception("Invalid module name");
		}
		if (Game.Gold < Cost) {return;} 
		Game.Gold -= Cost; 

		Module OldModule = modules[ModuleIndex]; // If we are just upgrading, this will be null
		if (OldModule != null) {OldModule.QueueFree();}

		modules[ModuleIndex] = module;
		AddChild(module);
		module.Position = ModuleIndex * 32 * Vector2.Up;
		DrawTower();
	}

	public void DrawTower(){
		//draw tower
	}
}
