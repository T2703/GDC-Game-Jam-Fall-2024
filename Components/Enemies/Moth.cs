using Godot;
using System;

public partial class Moth : BaseEnemy
{
	private int verticalDirection = 1;
	private float directionChangeInterval = 2.0f; // Interval in seconds
	private float timeSinceLastChange = 0.0f;

	public override void _Ready()
	{
		base._Ready();
		Health = 10;
		Speed = 200f; // Set your desired speed
		Damage = 1;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		// Increment timer
		timeSinceLastChange += (float)delta;

		// Change direction at regular intervals
		if (timeSinceLastChange >= directionChangeInterval)
		{
			verticalDirection *= -1; // Flip direction
			timeSinceLastChange = 0.0f; // Reset timer
		}
	}

	public override void Patrol()
	{
		// Move up and down based on the direction and speed
		Velocity = new Vector2(Velocity.X, Speed * verticalDirection);
	}
}
