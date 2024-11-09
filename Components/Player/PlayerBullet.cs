using Godot;
using System;

public partial class PlayerBullet : Area2D
{
	// Speed of the bullet.
	public float Speed = 3500f;

	// Direction of the bullet.
	private Vector2 Direction;

	// This causes the bullet to disappear after 3 seconds.
	private float Lifespan = 3f;

	// The damage
	public int Damage = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
		Lifespan -= (float)delta;

		if (Lifespan <= 0) QueueFree();
	}

	public void Fire(Vector2 direction) 
	{
		Direction = direction.Normalized();
	}
}
