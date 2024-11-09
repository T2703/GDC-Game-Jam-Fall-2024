using Godot;
using System;

public partial class BaseEnemy : CharacterBody2D
{
	// Basic enemy properties
	public int Health { get; set; }
    public float Speed { get; set; }
	public int Damage { get; set; }

	private const float Gravity = 400.0f;

	// Ref to the player node.
	private Node2D player;

	// Reference to Area2D for detecting the player
    private Area2D detectionRange;

	// Reference to Area2D for damaging
    private Area2D damageRange;

	// Track whether the player is in the enemy's range
	private bool playerInRange = false;

	// Track whether the player is inside the enemy
	private bool playerInEnemy = false;

	// Patrol boundaries 
	public float LeftBoundary  { get; set; }
	public float RightBoundary { get; set; }

	// -1 for left, 1 for right
	private int movementDirection = 1;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Node2D>("Player");

		// Detection Range 
		detectionRange = GetNode<Area2D>("DetectionArea");
		detectionRange.Connect("body_entered", new Callable(this, nameof(OnEnemyRangeEntered)));
		detectionRange.Connect("body_exited", new Callable(this, nameof(OnEnemyRangeExited)));

		// Damage Range
		damageRange = GetNode<Area2D>("DamageArea");
		damageRange.Connect("body_entered", new Callable(this, nameof(OnEnemyDamageEntered)));
		damageRange.Connect("body_exited", new Callable(this, nameof(OnEnemyDamageExited)));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		//GD.Print(playerInEnemy);
		// Continuously check if the player is within range
        if (playerInRange)
        {
            MoveTowardsPlayer();
        }
		else if (playerInEnemy)
        {
            player.Call("TakeDamage", Damage);
        }
		else 
		{
			Patrol();
		}

		ApplyGravity((float)delta);
		MoveAndSlide();
	}

	// Moving towards the player horizontally
	private void MoveTowardsPlayer() 
	{
		float direction = player.GlobalPosition.X > GlobalPosition.X ? 1 : 1;
		Velocity = new Vector2(direction * Speed, Velocity.Y);
	}

	// Patrol between boundaries if player is not in range,
	private void Patrol() {
		if ((GlobalPosition.X <= LeftBoundary && movementDirection == -1) || 
		    (GlobalPosition.X >= RightBoundary && movementDirection == 1))
		{
			movementDirection *= -1;
		}

		Velocity = new Vector2(movementDirection * Speed, Velocity.Y);
	}

	// This applies the gravity
	private void ApplyGravity(float delta)
    {
        if (!IsOnFloor())
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * delta);
        }
		else if (Velocity.Y > 0)
		{
			// Reset vertical velocity when on the ground
			Velocity = new Vector2(Velocity.X, 0);
		}
    }

	// When the player enters the enemy's range
	private void OnEnemyRangeEntered(Node body)
	{
		
		if (body is Player)
		{
			playerInRange = true;  
		}
	}

	// When the player leaves the enemy's range
	private void OnEnemyRangeExited(Node body)
	{
		if (body is Player)
		{
			playerInRange = false;  
			//playerInEnemy = false;
		}
	}

	// When the player enters the enemy's range
	private void OnEnemyDamageEntered(Node body)
	{
		if (body is Player)
		{
			playerInEnemy = true;  
		}
		else if (body is PlayerBullet bullet)
        {
            TakeDamage(bullet.Damage);
            bullet.QueueFree(); // Remove bullet after hit
            GD.Print("Bullet hit enemy");
        }
	}

	// When the player leaves the enemy's range
	private void OnEnemyDamageExited(Node body)
	{
		if (body is Player)
		{
			playerInEnemy = false;  
		}
	}

	// When the enemy takes damage.
    public virtual void TakeDamage(int damage) 
	{
		Health -= damage;
        if (Health <= 0) QueueFree();
	}
}
