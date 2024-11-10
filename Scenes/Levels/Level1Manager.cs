using Godot;
using System;

public partial class Level1Manager : Node2D
{
	public static Level1Manager Instance;
	private AudioStreamPlayer2D backgroundMusicPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		
		// Get the AudioStreamPlayer node
        backgroundMusicPlayer = GetNode<AudioStreamPlayer2D>("BackgroundMusic");
        backgroundMusicPlayer.Stream = (AudioStream) GD.Load("res://Sounds/Music/Retro_Platforming_-_David_Fesliyan.mp3");
		// Debugging to check if the node is found
        if (backgroundMusicPlayer != null)
        {
            backgroundMusicPlayer.Play();
        }
        else
        {
            GD.Print("AudioStreamPlayer2D not found!");
        }
	}

	public void StopMusic()
    {
        backgroundMusicPlayer.Stop();
    }

}
