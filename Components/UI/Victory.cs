using Godot;
using System;

public partial class Victory : Control
{
	private AudioStreamPlayer2D victorySFX;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
		victorySFX = GetNode<AudioStreamPlayer2D>("Clap");
		victorySFX.Play();
		Level1Manager.Instance?.StopMusic();
	}


	public void _on_complete_pressed()
	{
        GetTree().ChangeSceneToFile("res://Components/UI/menu.tscn");
	}
}
