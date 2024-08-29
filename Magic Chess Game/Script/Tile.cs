using System;
using Godot;

public partial class Tile : MeshInstance3D
{
	
	//Stack<Material> overlayMaterials = new Stack<Material>();
	[Export] Label label;
	public Vector2I coordinates{get; private set;}
	private ChessPiece _piece;
	public ChessPiece piece{
		get{return _piece;}
		set{_piece = value; if(_piece != null)_piece.tile = this;}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HighlightTile()
	{
		//overlayMaterials.Push(_highLight_Material);
		//ApplyOverlayMaterial();
		MaterialOverlay = Resources._highLight_Material;
	}

	public void RemoveHighlight()
	{
		//if(overlayMaterials.Count > 0)
			//overlayMaterials.Pop();

		//if(overlayMaterials.Count > 0)
			//ApplyOverlayMaterial();
		//else
			//MaterialOverlay = null;
		MaterialOverlay = null;
	}
	private void ApplyOverlayMaterial(){
			//MaterialOverlay = overlayMaterials.Peek();
	}
	public void InitTile(int x, int y){		
		Name = "Tile: " + x + ", " + y;
		coordinates = new Vector2I(x, y);
		label.Text = y + "," + x;
	}
}
