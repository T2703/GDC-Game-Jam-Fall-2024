using Godot;
using System;

public partial class Intro : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
	}


	public void _on_continue_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Levels/Level1.tscn");
	}
}
