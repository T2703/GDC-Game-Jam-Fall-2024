using Godot;
using System;

public partial class Dagrass : StaticBody2D
{
    private Sprite2D floorSprite;
	private CollisionPolygon2D collisionShape;
    private bool isVisible = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        floorSprite = GetNode<Sprite2D>("TouchGrass");
		collisionShape = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
        floorSprite.Visible = isVisible;
		SetCollisionEnabled(isVisible);
    }

	public override void _Process(double delta)
    {
        SetCollisionEnabled(isVisible);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void MakeVisibleForTime(float seconds)
    {
        isVisible = true;
        floorSprite.Visible = isVisible;

		SetCollisionEnabled(true);

        Timer timer = new Timer();
        timer.WaitTime = seconds;
        timer.OneShot = true;

        timer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));

        AddChild(timer);
        timer.Start();
    }

    private void OnTimerTimeout()
    {
        isVisible = false;
        floorSprite.Visible = isVisible;
		SetCollisionEnabled(false);
    }

	private void SetCollisionEnabled(bool enabled)
    {
        if (collisionShape != null)
        {
            collisionShape.Disabled = !enabled;
        }
    }
}