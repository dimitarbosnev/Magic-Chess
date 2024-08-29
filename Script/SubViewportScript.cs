using Godot;
using System;
[Tool]
public partial class SubViewportScript : SubViewport
{
	[Export] Label label;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Size = (Vector2I)label.Size;
	}
}
