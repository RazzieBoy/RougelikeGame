using Godot;
using System;

public partial class RangedEnemy : CharacterBody2D{
	
	Player player;
	[Export] float speed = 25f;
	
	public override void _Ready(){
		player = (Player)GetTree().Root.GetNode("Main").GetNode("Player");
	}

	public override void _Process(double delta){
	}
	
	public override void _PhysicsProcess(double delta){
		if (player != null){
			LookAt(player.GlobalPosition);
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * speed;
		}
		else{
			Velocity = Vector2.Zero;
		}
		MoveAndSlide();
	}
}
