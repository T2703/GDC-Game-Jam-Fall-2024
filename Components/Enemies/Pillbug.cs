using Godot;
using System;

public partial class Pillbug : BaseEnemy
{
    public override void _Ready()
    {
        base._Ready();
		Health = 3;
		Speed = 300f;
		Damage = 1;
    }
}
