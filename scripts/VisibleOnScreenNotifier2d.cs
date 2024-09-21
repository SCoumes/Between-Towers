using Godot;
using System;

public partial class VisibleOnScreenNotifier2d : VisibleOnScreenNotifier2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GetParent() is Enemy parent){
			ScreenExited += parent.exitedScreen;
		}
		else if (GetParent() is Spawner parent2){
			ScreenExited += parent2.exitedScreen;
		} else {
			throw new Exception("VisibleOnScreenNotifier2d: Parent is not Enemy or Spawner");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
