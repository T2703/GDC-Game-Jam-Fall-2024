using Godot;
using System;

public partial class Target : Area2D
{
	private Dagrass dagrass;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("area_entered", new Callable(this, nameof(OnEnemyRangeEntered)));
		dagrass = GetNode<Dagrass>("Dagrass"); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnEnemyRangeEntered(Node body)
	{
		if (body is PlayerBullet bullet)
		{
			bullet.QueueFree();
			dagrass.MakeVisibleForTime(5f);
		}
	}
}
