using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
	}

	public void _on_play_pressed()
	{
		GD.Print("lol");
		//QueueFree();
        GetTree().ChangeSceneToFile("res://Components/UI/Intro.tscn");
	}

	public void _on_settings_pressed()
	{
		GetTree().ChangeSceneToFile("res://Components/UI/settings.tscn");
	}

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}
