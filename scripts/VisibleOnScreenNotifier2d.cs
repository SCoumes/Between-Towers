using Godot;
using System;

public partial class VisibleOnScreenNotifier2d : VisibleOnScreenNotifier2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenExited += ((Spawner)GetParent()).exitedScreen;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
