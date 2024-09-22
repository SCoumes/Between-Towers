using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;


public partial class Tower : Node2D
{

	public int size=0;
	public List<Module> modules = new(); // The size of this should always be equal to size. So the highest module has index size-1
	// Called when the node enters the scene tree for the first time.
	
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

	public void OnModuleCliked(String ButtonText, int ModuleIndex){
		switch (ButtonText){
			case "Upgrade":
				if (Game.Gold < 5*size) {return;}
				Game.Gold -= 5*size;
				UpgradeTower();
				break;
			case "Remove":
				AddModule("Build", ModuleIndex);
				break;
			case "Archers":
				AddModule("Archer", ModuleIndex);
				break;
			case "Canons":
				AddModule("Marksman", ModuleIndex);
				break;
			default:
				throw new Exception("Invalid button name");
		}
	}

	public void AddModule(String ModuleName, int ModuleIndex){
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
			case "Marksman":
				PackedScene MarksmanScene = GD.Load<PackedScene>("res://scenes/Modules/marksman.tscn");
				module = MarksmanScene.Instantiate<MarksmanModule>();
				Cost = MarksmanModule.Cost;
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
		module.Index = ModuleIndex;
		module.Tower = this;
		DrawTower();
	}

	public void DrawTower(){
		//draw tower
	}
}
