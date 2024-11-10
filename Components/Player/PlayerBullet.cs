using Godot;
using System;

public partial class PlayerBullet : Area2D
{
	// Speed of the bullet.
	public float Speed = 1000f;

	// Direction of the bullet.
	private Vector2 Direction;

	// This causes the bullet to disappear after 3 seconds.
	private float Lifespan = 1.5f;

	// The damage
	public int Damage = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBulletBodyEntered)));
	}

	private void OnBulletBodyEntered(Node body)
	{
		if (body is BaseEnemy enemy)
		{
			enemy.TakeDamage(Damage);
			QueueFree();
		}
		else if (body is StaticBody2D || body is Area2D)
        {
			QueueFree();
        }
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
