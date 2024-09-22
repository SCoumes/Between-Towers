using Godot;
using System;

public partial class Tooltips : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override Control _MakeCustomTooltip(string forText)
	{
		var label = new Label();
		label.Text = forText;
		label.AddThemeColorOverride("font_color", new Color(1, 1, 1));
		return label;
	}
}
