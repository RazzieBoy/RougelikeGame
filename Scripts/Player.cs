using Godot;
using System;

public partial class Player : CharacterBody2D{
	//Allows for ease of debugging with the dash
	[Export] public float speed = 150f;
	[Export] public float dashSpeed = 1000f;
	[Export] public float dashTime = 0.2f;
	[Export] public float dashCooldown = 2f;
	
	//Bool to see if player is dashing or not
	private bool dashing = false;
	//Vector for the direction the player is dashing towards
	private Vector2 dashDir = Vector2.Zero;
	//Floats for dash duration and cooldown
	private float dashTimeLeft = 0f;
	private float dashCooldownTime = 0f;
	
	//Eventhandlers which connects the player script and other scripts
	//Connected to the player health script, player hud and the stat modifier scripts
	public event EventHandler<float> DashCooldownUpdatedEventHandler;
	public event EventHandler<(float currentHealth, float maxHealth)> HealthUpdatedEventHandler; // This line defines the event
	public event EventHandler<float> SpeedUpdatedEventHandler;
	public event EventHandler<float> DamageUpdatedEventHandler;
	
	//Holds the info for the layer the player is on
	private uint originalCollisionLayer;
	private uint originalCollisionMask;
	
	//Verifies the player is on the correct layer, (important for dashing)
	public override void _Ready(){
		originalCollisionLayer = CollisionLayer;
		originalCollisionMask = CollisionMask;
		SpeedUpdatedEventHandler?.Invoke(this, speed);
	}

	public override void _PhysicsProcess(double delta){
		//Makes the player always look at where the computer mouse is located
		LookAt(GetGlobalMousePosition());
		//If the player has dashed it then countsdown until the next time the player can dash again
		if (dashCooldownTime > 0){
			dashCooldownTime -= (float)delta;
			if (dashCooldownTime >= 0){
				//Sends info to the player hud
				DashCooldownUpdatedEventHandler?.Invoke(this, dashCooldownTime);
			}
			else{
				DashCooldownUpdatedEventHandler?.Invoke(this, 2);
			}
		}
		//If the player is dashing, make them move very quickly in the direction they are heading
		if (dashing){
			Velocity = dashDir * (dashSpeed + (speed * 1.5f));
			dashTimeLeft -= (float)delta;
			GD.Print(Velocity);
			//Resets the layer the player is on so collision is accurate
			if (dashTimeLeft <= 0){
				dashing = false;
				CollisionLayer = originalCollisionLayer;
				CollisionMask = originalCollisionMask;
			}
		}
		else{
			//Moves the player smoothly in any direction wanted on the X and Y axis
			Vector2 move_input = Input.GetVector("move_left","move_right","move_up","move_down");
			Velocity = move_input * speed;
			//Get's information for the player size and the screen size and makes it so the player can not leave the play area
			Vector2 viewportSize = GetViewportRect().Size;
			Vector2 newPosition = Position + Velocity * (float)delta;
			CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
			Rect2 shapeRect = collisionShape.Shape.GetRect();
			float playerWidth = shapeRect.Size.X;
			float playerHeight = shapeRect.Size.Y;
			newPosition.X = Mathf.Clamp(newPosition.X, 50 + playerWidth / 2, viewportSize.X - 50 - playerWidth / 2);
			newPosition.Y = Mathf.Clamp(newPosition.Y, 50 + playerHeight / 2, viewportSize.Y - 200 - playerHeight / 2);
			Position = newPosition;
			//When the player presses the button for the utility (dash) which is left shift the player dashes
			if (Input.IsActionJustPressed("utility") && dashCooldownTime <= 0 && move_input != Vector2.Zero){
				StartDash(move_input);
			}
		}
		//Built in function that makes the player move smoothly
		MoveAndSlide();
	}
	//Function that makes the player dash and handles the code relevant for the ability to dash
	private void StartDash(Vector2 direction){
		dashing = true;
		dashDir = direction.Normalized();
		dashTimeLeft = dashTime;
		dashCooldownTime = dashCooldown;
		CollisionLayer = 5;
		CollisionMask = 0; // Temporarily disable collision with everything
		DashCooldownUpdatedEventHandler?.Invoke(this, dashCooldownTime);
	}
	//Function that ensures the player takes damage when hit
	public void TakeDamage(float damage){
		var playerHealth = GetNode<PlayerHealth>("Health"); // Adjust path as needed
		playerHealth.Damage(damage);
		HealthUpdatedEventHandler?.Invoke(this, (playerHealth.HealthValue, playerHealth.maxHealth)); // Notify about health change
	}
	//Function that makes the player faster if a speed item is picked up
	public void ModifySpeed(float speedValue){
		speed += speedValue;
		GD.Print(speed);
		
		SpeedUpdatedEventHandler?.Invoke(this, speed + 150);
	}
	//Function that makes the player deal more damage if a damage item is picked up
	public void ModifyDamage(float damageValue){
		var gun = GetNode<Gun>("Gun");
		if (gun != null){
			gun.bulletDamage += damageValue;
			GD.Print("Damage modified. New bullet damage: " + gun.bulletDamage);
			
			DamageUpdatedEventHandler?.Invoke(this, gun.bulletDamage);
		}
	}
	//Function that makes the player go back to full health if a health item is picked up
	public void ModifyHealth(float healthValue){
		var playerHealth = GetNode<PlayerHealth>("Health"); // Adjust path as needed
		playerHealth.Heal(healthValue);  // Use Heal method from PlayerHealth class
		HealthUpdatedEventHandler?.Invoke(this, (playerHealth.HealthValue, playerHealth.maxHealth)); // Notify about health change
	}
}
