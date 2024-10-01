using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	[Export] public float speed = 300f;
	
	public override void _PhysicsProcess(double delta){
		LookAt(GetGlobalMousePosition());
		
		Vector2 move_input = Input.GetVector("move_left","move_right","move_up","move_down");
		
		Velocity = move_input * speed;
		
		MoveAndSlide();
	}
}
