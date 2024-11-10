using Godot;
using System;

public partial class BaseEnemy : CharacterBody2D
{
	// Basic enemy properties
	public int Health { get; set; }
    public float Speed { get; set; }
	public int Damage { get; set; }

	private const float Gravity = 400.0f;
	private const float BoundaryTolerance = 40f;

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
		if (IsOnWall()) 
		{
			if (movementDirection == 1)
			{
				movementDirection = -1;
			}
			else if (movementDirection == -1)
			{
				movementDirection = 1;
			}
			Scale = new Vector2(-1, Scale.Y);
		}
		else if (playerInEnemy)
        {
            player.Call("TakeDamage", Damage, GlobalPosition);
        }
		else
		{
			Patrol();
		}

		ApplyGravity((float)delta);
		MoveAndSlide();
	}
		
	public virtual void Patrol()
	{
		Velocity = new Vector2(Speed * movementDirection, Velocity.Y);
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
			
		}
	}

	// When the player enters the enemy's range
	private void OnEnemyDamageEntered(Node body)
	{
		if (body is Player player)
		{
			if (player.attackCooldownTimer <= 0) 
			{
				player.TakeDamage(Damage, GlobalPosition);
			}

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

	// Change direction on left bumper
	private void OnLeftBumperEntered(Node body)
	{
		if (body == this)
		{
			movementDirection = 1;
			Scale = new Vector2(1, Scale.Y);
		}
	}

	// Change direction on right bumper
	private void OnRightBumperEntered(Node body)
	{
		if (body == this)
		{
			movementDirection = -1;
			Scale = new Vector2(-1, Scale.Y);
		}
	}

	// When the enemy takes damage.
    public virtual void TakeDamage(int damage) 
	{
		Health -= damage;
        if (Health <= 0)
		{
			QueueFree();
			SpawnHealing();

			//GameManager.Instance.AllEnemiesAreDead();
		} 
	}

	protected virtual void SpawnHealing() 
	{
		if (GameManager.Instance.PlayerHealth == 1)
		{
			return;
		}
		if (new Random().NextDouble() <= 0.4)
		{
			PackedScene healthPickupScene = (PackedScene)ResourceLoader.Load("res://Components/Pickups/heatlh_pickup.tscn");

			if (healthPickupScene != null)
			{
			// Create the bullet instance
			HeatlhPickup heatlhPickup = healthPickupScene.Instantiate<HeatlhPickup>();

			// Set the bullet's position to the gun's position (or adjust as needed)
			heatlhPickup.Position = GlobalPosition;

			// Add the bullet to the scene tree
			GetTree().Root.AddChild(heatlhPickup);

			}
		}
	}
}
