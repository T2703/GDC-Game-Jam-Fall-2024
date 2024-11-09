using Godot;
using System;
using System.Runtime.CompilerServices;

// The player class so how movement and attacking works.
public partial class Player : CharacterBody2D
{
    // Constants for player movement
    public const float Speed = 420.0f;
    public const float JumpVelocity = -600.0f;

	// Player's health in hearts.
	public int health = 5;

	// Player sprite reference.
	private Sprite2D sprite;

	// Track the last movement direction for shooting.
	private Vector2 lastDirection = Vector2.Right;

	// Cooldowns for attack
	private float attackCooldown = 0.5f;
	public float attackCooldownTimer = 0f;

	// Knockback props
	private Vector2 knockbackDirection = Vector2.Zero;
	private float knockbackForce = 4500.0f;
	private float knockbackTimer = 0f;
	private float knockbackDuration = 0.3f;

	// Flash
	private Timer flashTimer;
	private int flashCount = 0;

    public override void _Ready()
    {
        base._Ready();
		sprite = GetNode<Sprite2D>("Player");
		flashTimer = GetNode<Timer>("FlashTimer");
		flashTimer.Connect("timeout", new Callable(this, nameof(OnFlashTimeout)));
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (knockbackTimer > 0)
		{
			velocity += knockbackDirection * knockbackForce * (float)delta;
			knockbackTimer -= (float)delta;
		}
		else 
		{
			HandleInput(ref velocity, delta);
		}

		if (attackCooldownTimer > 0) attackCooldownTimer -= (float)delta;

		Velocity = velocity;
		MoveAndSlide();
	}

	// Handles the input for the player.
	private void HandleInput(ref Vector2 velocity, double delta)
    {
        // Apply gravity if not on the floor
        ApplyGravity(ref velocity, delta);

        // Handle jumping input
        if (Input.IsActionJustPressed("game_space") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Handle movement input
        Vector2 inputDirection = Input.GetVector("game_left", "game_right", "game_up", "game_down");
        if (inputDirection != Vector2.Zero)
        {
            velocity.X = inputDirection.X * Speed;
			lastDirection = inputDirection;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

		HandleSpriteFlip(inputDirection);

        // Check for shooting input
        if (Input.IsActionJustPressed("mouse1"))
        {
            ShootAttackOne();
        }
    }

	// This applies the gravity for the player
	private void ApplyGravity(ref Vector2 velocity, double delta)
    {
        if (!IsOnFloor())
        {
			knockbackForce = 5000.0f;
			knockbackDuration = 0.2f;
            velocity += GetGravity() * (float)delta;
        }
		else 
		{
			knockbackForce = 4500.0f;
			knockbackDuration = 0.3f;
		}
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
            
			// Fire the bullet in the direction
				bullet.Fire(lastDirection);

			// Add the bullet to the scene tree
			GetTree().Root.AddChild(bullet);

		}
	}

	// Method to filp the sprite based on movement direction
	private void HandleSpriteFlip(Vector2 direction) {
		// This does flipping if moving right or left.
		if (direction.X < 0) sprite.Scale = new Vector2(-1, 1);
		else if (direction.X > 0) sprite.Scale = new Vector2(1, 1);
	}

	// For when the player takes damage.
	public virtual void TakeDamage(int damage, Vector2 enemyPos) 
	{
		if (attackCooldownTimer <= 0) 
		{
			health -= damage;
			GD.Print(health);
        	if (health <= 0) QueueFree();

			knockbackDirection = (GlobalPosition - enemyPos).Normalized();
			knockbackTimer = knockbackDuration;

			attackCooldownTimer = attackCooldown;

			flashCount = 10;
			flashTimer.Start();
		}

	}

	// Healing method for the player.
	public void Heal(int amount) 
	{
		health += amount;
		health = Math.Min(health, 5);
	}

	private void OnFlashTimeout()
	{
		sprite.Visible = !sprite.Visible;

		flashCount--;
		if (flashCount <= 0) 
		{
			flashTimer.Stop();
			sprite.Visible = true;
		}
	}
}
