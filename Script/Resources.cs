using Godot;
using System.Collections.Generic;
public static class Resources{

    public static OrmMaterial3D _highLight_Material = ResourceLoader.Load<OrmMaterial3D>("res://Assets/Materials/High_Light_Material.tres");
	public static PackedScene tileScene = ResourceLoader.Load<PackedScene>("res://Scene/hex_tile.tscn");
	public static PackedScene mainScene = ResourceLoader.Load<PackedScene>("res://Scene/level_1.tscn");
	public static Dictionary<PieceType,PackedScene> pieceCollection = new Dictionary<PieceType,PackedScene>(){
		{PieceType.None, ResourceLoader.Load<PackedScene>("res://Scene/Soldier.tscn")},
		{PieceType.Soldier, ResourceLoader.Load<PackedScene>("res://Scene/Soldier.tscn")},
		{PieceType.RedKing, ResourceLoader.Load<PackedScene>("res://Scene/RedKing.tscn")},
		{PieceType.BlueKing, ResourceLoader.Load<PackedScene>("res://Scene/BlueKing.tscn")},
		{PieceType.Queen, ResourceLoader.Load<PackedScene>("res://Scene/Queen.tscn")},
		{PieceType.Assassin, ResourceLoader.Load<PackedScene>("res://Scene/Assasin.tscn")},
		{PieceType.Centaur, ResourceLoader.Load<PackedScene>("res://Scene/Centaur.tscn")},
		{PieceType.Shapeshifter, ResourceLoader.Load<PackedScene>("res://Scene/Shapeshifter.tscn")},
		{PieceType.Mage, ResourceLoader.Load<PackedScene>("res://Scene/Wizard.tscn")},
	};
	public static StandardMaterial3D[] material = {
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_1.tres"),
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_2.tres"),
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_3.tres")
	};
}


