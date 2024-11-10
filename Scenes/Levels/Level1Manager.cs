using Godot;
using System;

public partial class Level1Manager : Node2D
{
	private AudioStreamPlayer2D backgroundMusicPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the AudioStreamPlayer node
        backgroundMusicPlayer = GetNode<AudioStreamPlayer2D>("BackgroundMusic");
        
		// Debugging to check if the node is found
        if (backgroundMusicPlayer != null)
        {
            GD.Print("Background music player found!");

            // Play the music
            backgroundMusicPlayer.Play();
            GD.Print("Playing music");
        }
        else
        {
            GD.Print("AudioStreamPlayer2D not found!");
        }
	}

}
