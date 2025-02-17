using Godot;
using System;

public partial class Pawn : AnimatedSprite2D
{
	[Export]
	public int Speed { get; set; } = 400;
	private Vector2 target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("click"))
		{
			target = GetGlobalMousePosition();
		}
		base._Input(@event);
	}

	public override void _PhysicsProcess(double delta)
	{
		this.Position = this.Position.MoveToward(target, Speed * (float)delta);
		base._PhysicsProcess(delta);
	}
}
