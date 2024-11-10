using Godot;
using System;

public partial class Settings : Control
{
	private CheckBox oneHitCheckBox;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
	}

	public void _on_back_pressed()
	{
		GetTree().ChangeSceneToFile("res://Components/UI/menu.tscn");
	}
}
