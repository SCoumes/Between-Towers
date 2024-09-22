using Godot;
using System;

public abstract partial class Module : Node2D
{
	 public static readonly int Cost;
	 public readonly new string Name;
	 public int Index;
	 public Tower Tower;

	public void OnClick(Viewport viewport, InputEvent InEvent, int shape_idx){
		if (InEvent is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left){
			Game.game.OnTowerModuleButtonPressed(Tower, Index);
		}
	}
}

