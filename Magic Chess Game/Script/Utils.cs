using Godot;
using System;
using System.Collections.Generic;
public static class Utils{

    public static OrmMaterial3D _highLight_Material = ResourceLoader.Load<OrmMaterial3D>("res://Assets/Materials/High_Light_Material.tres");
	public static PackedScene hexTileScene = ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/hex_tile.tscn");
	public static PackedScene rectTileScene = ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/rect_tile.tscn");
	public static Dictionary<PieceType,PackedScene> pieceCollection = new Dictionary<PieceType,PackedScene>(){
		{PieceType.None, 				ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/Soldier.tscn")},
		{PieceType.HexSoldier, 			ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/Soldier.tscn")},
		{PieceType.HexRedKing, 			ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/RedKing.tscn")},
		{PieceType.HexBlueKing, 		ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/BlueKing.tscn")},
		{PieceType.HexQueen, 			ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/Queen.tscn")},
		{PieceType.HexAssassin, 		ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/Assasin.tscn")},
		{PieceType.HexCentaur, 			ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/Centaur.tscn")},
		{PieceType.HexShapeshifter, 	ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/Shapeshifter.tscn")},
		{PieceType.HexMage, 			ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/HexagonalPrefabs/Wizard.tscn")},
		{PieceType.RectSoldier, 		ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/Soldier.tscn")},
		{PieceType.RectRedKing, 		ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/RedKing.tscn")},
		{PieceType.RectBlueKing, 		ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/BlueKing.tscn")},
		{PieceType.RectQueen, 			ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/Queen.tscn")},
		{PieceType.RectAssassin, 		ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/Assasin.tscn")},
		{PieceType.RectCentaur, 		ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/Centaur.tscn")},
		{PieceType.RectShapeshifter, 	ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/Shapeshifter.tscn")},
		{PieceType.RectMage, 			ResourceLoader.Load<PackedScene>("res://Scene/PrefabScenes/RectangularPrefabs/Wizard.tscn")}
	};

	public static StandardMaterial3D[] material = {
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_1.tres"),
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_2.tres"),
	ResourceLoader.Load<StandardMaterial3D>("res://Assets/Materials/Tile_Material_3.tres")
	};

	public static Dictionary<Type,PackedScene> sceneCollection = new Dictionary<Type,PackedScene>(){
		{typeof(LoginView), ResourceLoader.Load<PackedScene>("res://Scene/LoginScene.tscn")},
		{typeof(LobbyView), ResourceLoader.Load<PackedScene>("res://Scene/LobbyScene.tscn")},
		{typeof(GameViewRect), ResourceLoader.Load<PackedScene>("res://Scene/GameSceneRectBoard.tscn")},
		{typeof(GameViewHex), ResourceLoader.Load<PackedScene>("res://Scene/GameSceneHexBoard.tscn")},
	};

}


