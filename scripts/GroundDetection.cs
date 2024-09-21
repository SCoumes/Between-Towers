using Godot;
using System;

public partial class GroundDetection : Area2D
{

	public override void _Ready()
	{
		BodyEntered += ennemyEntered;
	}

	public static void ennemyEntered(object body)
	{
		if (body is Enemy _)
		{
			GD.Print("GroundDetection body entered");
		}
	}
}
