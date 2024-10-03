using Godot;
using System;

public partial class Player : CharacterBody2D{
	//Allows for simple debbuing to change the speed in the inspector
	[Export] public float speed = 300f;
	[Export] public int health = 10;
	
	public override void _PhysicsProcess(double delta){
		//Makes the player always look at where the computer mouse is located
		LookAt(GetGlobalMousePosition());
		
		//Moves the player smoothly in any direction wanted on the X and Y axis
		Vector2 move_input = Input.GetVector("move_left","move_right","move_up","move_down");
		
		Velocity = move_input * speed;
		
		MoveAndSlide();
		
		if (Input.IsActionJustPressed("home")){
			GetTree().ChangeSceneToFile("res://main_menu.tscn");
		}
	}
}
