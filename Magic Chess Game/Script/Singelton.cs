using Godot;
using System;

public abstract partial class Singelton<T> : Node3D where T : Node3D
{
	protected static T _instance;
    public static T Instance{
		get{return _instance;}
	}
}
