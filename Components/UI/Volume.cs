using Godot;
using System;

public partial class Volume : HSlider
{
	private const string AudioBusName = "SFX";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		float currentVolume = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex(AudioBusName));
		Value = Mathf.InverseLerp(-20.0f, 0.0f, currentVolume);
		Connect("value_changed", new Callable(this, nameof(OnVolumeChanged)));
	}

    // Called every time the slider value changes
    private static void OnVolumeChanged(float value)
    {
        float volumeDb = Mathf.Lerp(-20.0f, 0.0f, value);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(AudioBusName), volumeDb);
    }
}
