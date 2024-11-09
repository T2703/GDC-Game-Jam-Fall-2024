using Godot;
using System;

public partial class Pillbug : BaseEnemy
{
    public override void _Ready()
    {
        base._Ready();
		Health = 3;
		Speed = 200f;
		Damage = 1;
		LeftBoundary = 20;
		RightBoundary = 20;
    }
}
