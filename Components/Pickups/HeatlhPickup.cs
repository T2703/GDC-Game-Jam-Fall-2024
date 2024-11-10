using Godot;
using System;

// Healing item duh
public partial class HeatlhPickup : Area2D
{
	// Player reference
	private Player player;

	public int healingAmount = 1;

	private float Lifespan = 10f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("HealthPickup");
	}

	private void OnHealthPickupBodyEntered(Node body) 
	{
		if (body is Player player1)
		{
			if (player1.health < 5) 
			{
				player = player1;
				player.Heal(healingAmount);
				QueueFree();
			}
		}
	}

	public override void _Process(double delta)
	{

		if (Lifespan <= 0) QueueFree();
	}
}
