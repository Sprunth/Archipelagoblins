using Godot;
using System;

public partial class AgentMover : Node
{
	[Export]
	public int Speed { get; set; } = 200;
	private Vector2 target = new Vector2(800, 300);

	CharacterBody2D agent;
	AnimatedSprite2D sprite;
	NavigationAgent2D navAgent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		agent = GetParent<CharacterBody2D>();
		if (agent == null)
		{
			GD.PrintErr("AgentMover: agent is null");
			return;
		}

		navAgent = agent.GetNode<NavigationAgent2D>("navigation/NavigationAgent2D");
		if (navAgent == null)
		{
			GD.PrintErr("AgentMover: navAgent is null");
			return;
		}

		sprite = agent.GetNode<AnimatedSprite2D>("sprite");
		if (sprite == null)
		{
			GD.PrintErr("AgentMover: sprite is null");
			return;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (agent.Velocity.Length() > 0.1f)
		{
			sprite.Animation = "walk";
			sprite.FlipH = agent.Velocity.X < 0;
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
			target = agent.GetGlobalMousePosition();
		}
		base._Input(@event);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (agent.Position.DistanceTo(target) < 1)
		{
			agent.Velocity = Vector2.Zero;
			return;
		}

		var direction = navAgent.GetNextPathPosition() - agent.GlobalPosition;
		direction = direction.Normalized();

		agent.Velocity = agent.Velocity.Lerp(direction * Speed, 50f * (float)delta);

		agent.MoveAndSlide();
		base._PhysicsProcess(delta);
	}

	public void OnNavTimerTimeout()
	{
		navAgent.TargetPosition = target;
	}
}
