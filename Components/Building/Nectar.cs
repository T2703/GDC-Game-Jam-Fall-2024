using Godot;
using System;

public partial class Nectar : StaticBody2D
{
	private Node2D player;
	private Node2D moth;
	private bool playerInRange = false;
	private Area2D detectionRange;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Node2D>("Player");
		moth = GetParent().GetNode<Node2D>("Moth");
		GD.Print("Moth ", moth);

		if (moth != null)
		{
			moth.Connect("tree_exited", new Callable(this, nameof(OnMothDestroyed)));
		}

		detectionRange = GetNode<Area2D>("DetectionArea");
		detectionRange.Connect("body_entered", new Callable(this, nameof(OnEnemyDamageEntered)));
		detectionRange.Connect("body_exited", new Callable(this, nameof(OnEnemyDamageExited)));
	}

	private void OnMothDestroyed()
	{
		QueueFree(); // Free the Nectar object when moth is destroyed
	}

	// When the player enters the enemy's range
	private void OnEnemyDamageEntered(Node body)
	{
		if (body is Player player)
		{
			if (player.attackCooldownTimer <= 0) 
			{
				player.TakeDamage(2, GlobalPosition);
			}

			playerInRange = true;  
		}
		else if (body is PlayerBullet bullet)
        {
            bullet.QueueFree();
        }
	}

	private void OnEnemyDamageExited(Node body)
	{
		if (body is Player)
		{
			playerInRange = false;  
		}
	}
}
