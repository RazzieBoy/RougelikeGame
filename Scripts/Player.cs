using Godot;
using System;

public partial class Player : CharacterBody2D{
	//Allows for simple debbuing to change the speed in the inspector
	[Export] public float speed = 300f;
	[Export] public float dashSpeed = 1000f;
	[Export] public float dashTime = 0.2f;
	[Export] public float dashCooldown = 2f;
	
	private bool dashing = false;
	private Vector2 dashDir = Vector2.Zero;
	private float dashTimeLeft = 0f;
	private float dashCooldownTime = 0f;
	
	public event EventHandler<float> DashCooldownUpdatedEventHandler;
	
	public override void _PhysicsProcess(double delta){
		//Makes the player always look at where the computer mouse is located
		LookAt(GetGlobalMousePosition());
		
		if (dashCooldownTime > 0){
			dashCooldownTime -= (float)delta;
			DashCooldownUpdatedEventHandler?.Invoke(this, dashCooldownTime);
		}
		if (dashing){
			Velocity = dashDir * dashSpeed;
			dashTimeLeft -= (float)delta;
			
			if (dashTimeLeft <= 0){
				dashing = false;
			}
		}
		else{
			//Moves the player smoothly in any direction wanted on the X and Y axis
			Vector2 move_input = Input.GetVector("move_left","move_right","move_up","move_down");
			Velocity = move_input * speed;
			
			if (Input.IsActionJustPressed("utility") && dashCooldownTime <= 0 && move_input != Vector2.Zero){
				StartDash(move_input);
			}
		}
		MoveAndSlide();
	}
	
	private void StartDash(Vector2 direction){
		dashing = true;
		dashDir = direction.Normalized();
		dashTimeLeft = dashTime;
		dashCooldownTime = dashCooldown;
		
		DashCooldownUpdatedEventHandler?.Invoke(this, dashCooldownTime);
	}
}
