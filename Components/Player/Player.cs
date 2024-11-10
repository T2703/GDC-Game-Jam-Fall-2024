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

	// Time is money
	public int timerLevel = 160;
	private int remainingTime;

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

	// Packages to deliever
	public int packageCount = 3;

	// Flash
	private Timer flashTimer;
	private int flashCount = 0;

	private Label healthLabel;
	private Label packageLabel;
	private Label timerLabel;

	// Sound
	private AudioStreamPlayer2D gunShotSFX;
	private AudioStreamPlayer2D jumpSFX;

	private Level1Manager level1Manager;

    public override void _Ready()
    {
        base._Ready();
		sprite = GetNode<Sprite2D>("Player");
		gunShotSFX = GetNode<AudioStreamPlayer2D>("Gunshoot");
		jumpSFX = GetNode<AudioStreamPlayer2D>("Jump");

		flashTimer = GetNode<Timer>("FlashTimer");
		flashTimer.Connect("timeout", new Callable(this, nameof(OnFlashTimeout)));

		healthLabel = GetNode<Label>("HealthLabel");
		packageLabel = GetNode<Label>("PacakgeLabel");
		timerLabel = GetNode<Label>("TimeLabel");
		remainingTime = timerLevel;
		UpdateHealthLabel();
		UpdatePackageLabel();
		UpdateTimerLabel();

		Timer countdownTimer = new Timer();
        countdownTimer.WaitTime = 1.0f; 
        countdownTimer.OneShot = false;
        countdownTimer.Autostart = true;
        countdownTimer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));
        AddChild(countdownTimer);
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
			jumpSFX.Play();
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

			gunShotSFX.Play();
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
        	if (health <= 0) 
			{
				GameOver();
				QueueFree();
			}
			UpdateHealthLabel();

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
		UpdateHealthLabel();
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

	// Game over man!
	private void GameOver()
	{
		var gameOverScene = (PackedScene)ResourceLoader.Load("res://Components/UI/game_over.tscn");
        var gameOverInstance = gameOverScene.Instantiate();
		GetTree().CurrentScene.AddChild(gameOverInstance);
		//GetTree().Paused = true; 
	}

	private void UpdateHealthLabel()
	{
		healthLabel.Text = "Life: " + health;
	}

	public void UpdatePackageLabel()
	{
		packageLabel.Text = "Packages To Deliever: " + packageCount;
	}

	public void UpdateTimerLabel()
	{
		timerLabel.Text = "Time Left: " + remainingTime;
	}

	private void OnTimerTimeout()
    {
        remainingTime--; 

        timerLabel.Text = "Time Left: " + remainingTime.ToString();

        if (remainingTime <= 0)
        {
            GameOver();
			QueueFree();
        }
    }
}
