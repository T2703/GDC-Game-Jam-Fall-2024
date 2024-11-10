using Godot;
using System;

public partial class Pause : CanvasLayer
{
	public static Pause Instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		ProcessMode = Node.ProcessModeEnum.WhenPaused;
		GetNode<Button>("Resume").Connect("pressed", new Callable(this, nameof(OnResumePressed)));
		GetNode<Button>("Retry").Connect("pressed", new Callable(this, nameof(OnRetryPressed)));
		GetNode<Button>("Quit").Connect("pressed", new Callable(this, nameof(OnQuitPressed)));
		Visible = false;
	}

	private void OnResumePressed()
    {
        GetTree().Paused = false;
		Visible = false;
    }

	private void OnRetryPressed()
    {
		ClearLevelState();
		QueueFree();
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Level1.tscn");
    }

	private void OnQuitPressed()
    {
        QueueFree();
        GetTree().ChangeSceneToFile("res://Components/UI/menu.tscn");
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
