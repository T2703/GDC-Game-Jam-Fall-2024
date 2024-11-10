using Godot;
using System;

public partial class GameOver : Control
{
	private AudioStreamPlayer2D screamSFX;

    public override void _Ready()
    {
        screamSFX = GetNode<AudioStreamPlayer2D>("SFX");
		screamSFX.Play();
		Level1Manager.Instance?.StopMusic(); // Stop the level 1 music
    }
    public void _on_retry_pressed()
	{
		ClearLevelState();
		QueueFree();
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Level1.tscn");
	}

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}

	private void ClearLevelState()
    {
        var items = GetTree().GetNodesInGroup("HealthPickup"); 
        foreach (Node item in items)
        {
            item.QueueFree(); 
			GD.Print(item);
        }

    }
}
