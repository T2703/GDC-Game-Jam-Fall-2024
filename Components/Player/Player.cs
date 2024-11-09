using Godot;
using System;

// The player class so how movement and attacking works.
public partial class Player : CharacterBody2D
{
	public const float Speed = 420.0f;
	public const float JumpVelocity = -600.0f;
	public int health = 100;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("game_space") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("game_left", "game_right", "game_up", "game_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		if (Input.IsActionJustPressed("mouse1")) ShootAttackOne();

		Velocity = velocity;
		MoveAndSlide();
	}
	
	// Shooting attack baisc.
	private void ShootAttackOne() 
	{
		PackedScene bulletScene = (PackedScene)ResourceLoader.Load("res://Components/Player/player_bullet.tscn");

		if (bulletScene != null)
		{
			// Create the bullet instance
			PlayerBullet bullet = bulletScene.Instantiate<PlayerBullet>();

			// Set the bullet's position to the gun's position (or adjust as needed)
			bullet.Position = GlobalPosition;

			// Calculate direction towards the mouse
			Vector2 direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
            
			// Fire the bullet in the direction
			bullet.Fire(direction);

			// Add the bullet to the scene tree
			GetTree().Root.AddChild(bullet);

		}
	}
}
