using Godot;
using System;

public partial class Pawn : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 200;
	private Vector2 target = new Vector2(800, 300);

	AnimatedSprite2D sprite;
	NavigationAgent2D navAgent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("navigation/NavigationAgent2D");
		sprite = GetNode<AnimatedSprite2D>("sprite");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Velocity.Length() > 0.1f)
		{
			sprite.Animation = "walk";
		}
		else
		{
			sprite.Animation = "idle";
		}
		base._Process(delta);
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
		if (Position.DistanceTo(target) < 1)
		{
			Velocity = Vector2.Zero;
			return;
		}

		var direction = navAgent.GetNextPathPosition() - GlobalPosition;
		direction = direction.Normalized();

		Velocity = Velocity.Lerp(direction * Speed, 50f * (float)delta);

		MoveAndSlide();
	}

	public void OnNavTimerTimeout()
	{
		navAgent.TargetPosition = target;
	}
}
