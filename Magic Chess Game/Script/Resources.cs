using Godot;
using System;
using System.Collections.Generic;
public static class Resources{

    public static OrmMaterial3D _highLight_Material = ResourceLoader.Load<OrmMaterial3D>("res://Assets/Materials/High_Light_Material.tres");
	public static PackedScene tileScene = ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/hex_tile.tscn");
	public static Dictionary<PieceType,PackedScene> pieceCollection = new Dictionary<PieceType,PackedScene>(){
		{PieceType.None, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/Soldier.tscn")},
		{PieceType.Soldier, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/Soldier.tscn")},
		{PieceType.RedKing, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RedKing.tscn")},
		{PieceType.BlueKing, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/BlueKing.tscn")},
		{PieceType.Queen, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/Queen.tscn")},
		{PieceType.Assassin, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/Assasin.tscn")},
		{PieceType.Centaur, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/Centaur.tscn")},
		{PieceType.Shapeshifter, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/Shapeshifter.tscn")},
		{PieceType.Mage, ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/Wizard.tscn")},
	};
	public static StandardMaterial3D[] material = {
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_1.tres"),
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_2.tres"),
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_3.tres")
	};

	public static Dictionary<Type,PackedScene> sceneCollection = new Dictionary<Type,PackedScene>(){
		{typeof(LoginView), ResourceLoader.Load<PackedScene>("res://Scene/LoginScene.tscn")},
		{typeof(LobbyView), ResourceLoader.Load<PackedScene>("res://Scene/LobbyScene.tscn")},
		{typeof(GameView), ResourceLoader.Load<PackedScene>("res://Scene/GameScene.tscn")},
	};
}


