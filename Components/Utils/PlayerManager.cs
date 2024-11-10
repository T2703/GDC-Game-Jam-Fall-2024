using Godot;
using System;

public partial class PlayerManager : Node
{
	public static PlayerManager Instance { get; private set; }
	public Player PlayerInstance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		Node parentNode = GetParent();
		GD.Print(GetParent() as Player);
        if (parentNode is Player player)
        {
            PlayerInstance = player;
        }
        else
        {
            GD.PrintErr("Failed to find Player instance as parent.");
        }
	}

    public void SetPlayerHealth(int newhealth)
    {
        PlayerInstance.health = newhealth;
    }
}
