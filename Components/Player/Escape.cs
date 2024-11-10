using Godot;
using System;

public partial class Escape : Area2D
{
	private Node2D player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Node2D>("Player");
		Connect("body_entered", new Callable(this, nameof(OnPackageEntered)));
	}

	private void OnPackageEntered(Node body)
	{
		if (body is Player player)
		{
			if (player.packageCount <= 0) 
			{
				GetTree().ChangeSceneToFile("res://Components/UI/Victory.tscn");
			}
			else{
				GD.Print("NO");
			}
		} 

	}
}
