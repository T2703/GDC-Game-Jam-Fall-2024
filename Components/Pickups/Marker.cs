using Godot;
using System;

public partial class Marker : Area2D
{
	private Node2D player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Node2D>("Player");
		Connect("body_entered", new Callable(this, nameof(OnPackageEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPackageEntered(Node body)
	{
		if (body is Player player)
		{
			player.packageCount -= 1;
			player.UpdatePackageLabel();
			QueueFree();

			if (player.packageCount <= 0)
			{
				GD.Print("WIN");
			}
		} 

	}
}
