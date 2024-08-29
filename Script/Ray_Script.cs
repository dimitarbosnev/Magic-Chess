using Godot;

public partial class Ray_Script : RayCast3D
{
	[Export] Camera3D Camera = null;
	public Tile currentHover{get; private set;}
	
	public override void _Process(double delta)
	{
		if(currentHover != null){
			currentHover.RemoveHighlight();
			currentHover = null;
		}
		Vector2 mousePos = GetViewport().GetMousePosition();
		TargetPosition = Camera.ProjectLocalRayNormal(mousePos) * 20;
		if (GetCollider() is StaticBody3D body)
			if (body.GetParent() is Tile tile)
			{
				currentHover = tile;
				currentHover.HighlightTile();
			}
	}
}

