using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;

	public int PlayerHealth { get; set; } = 5;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Check condition for all enemies being dead.
	public void AllEnemiesAreDead()
	{
		var enemies = GetTree().GetNodesInGroup("enemies");

		if (enemies.Count == 0)
		{
			GD.Print("WIN");
		}
	}
}
