using Godot;
using System;

public partial class DeathArea : Area2D
{
	// Ref to the player node.
	private Node2D player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Node2D>("Player");
		Connect("body_entered", new Callable(this, nameof(OnDeadEntered)));
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	private void OnDeadEntered(Node body)
	{
		if (body is Player player) player.TakeDamage(100, Vector2.Zero);

	}
}
