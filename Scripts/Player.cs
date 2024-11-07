using Godot;
using System;

public partial class Player : CharacterBody2D{
	//Allows for simple debbuing to change the speed in the inspector
	[Export] public float speed = 150f;
	[Export] public float dashSpeed = 1000f;
	[Export] public float dashTime = 0.2f;
	[Export] public float dashCooldown = 2f;
	
	private bool dashing = false;
	private Vector2 dashDir = Vector2.Zero;
	private float dashTimeLeft = 0f;
	private float dashCooldownTime = 0f;
	
	public event EventHandler<float> DashCooldownUpdatedEventHandler;
	public event EventHandler<(float currentHealth, float maxHealth)> HealthUpdatedEventHandler; // This line defines the event

	private uint originalCollisionLayer;
	private uint originalCollisionMask;
	
	public override void _Ready(){
		originalCollisionLayer = CollisionLayer;
		originalCollisionMask = CollisionMask;
	}

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
				CollisionLayer = originalCollisionLayer;
				CollisionMask = originalCollisionMask;
			}
		}
		else{
			//Moves the player smoothly in any direction wanted on the X and Y axis
			Vector2 move_input = Input.GetVector("move_left","move_right","move_up","move_down");
			Velocity = move_input * speed;
			
			Vector2 viewportSize = GetViewportRect().Size;
			Vector2 newPosition = Position + Velocity * (float)delta;
			CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D"); // Adjust the path as necessary
			Rect2 shapeRect = collisionShape.Shape.GetRect(); // Assumes it's a RectangleShape2D
			float playerWidth = shapeRect.Size.X;
			float playerHeight = shapeRect.Size.Y;
			newPosition.X = Mathf.Clamp(newPosition.X, 50 + playerWidth / 2, viewportSize.X - 50 - playerWidth / 2);
			newPosition.Y = Mathf.Clamp(newPosition.Y, 50 + playerHeight / 2, viewportSize.Y - 200 - playerHeight / 2);

			Position = newPosition;
			
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
		CollisionLayer = 5;
		CollisionMask = 0; // Temporarily disable collision with everything
		DashCooldownUpdatedEventHandler?.Invoke(this, dashCooldownTime);
	}
	
	public void TakeDamage(float damage){
		var playerHealth = GetNode<PlayerHealth>("Health"); // Adjust path as needed
		playerHealth.Damage(damage);
		HealthUpdatedEventHandler?.Invoke(this, (playerHealth.HealthValue, playerHealth.maxHealth)); // Notify about health change
	}

	public void Heal(float amount){
		var playerHealth = GetNode<PlayerHealth>("Health"); // Adjust path as needed
		playerHealth.Heal(amount);
		HealthUpdatedEventHandler?.Invoke(this, (playerHealth.HealthValue, playerHealth.maxHealth)); // Notify about health change
	}

	public void ModifySpeed(float speedValue){
		speed += speedValue;
	}
	
	public void ModifyDamage(float damageValue){
		var gun = GetNode<Gun>("Gun");
		if (gun != null){
			gun.bulletDamage += damageValue;
			//GD.Print("Damage modified. New bullet damage: " + gun.bulletDamage);
		}
	}
}
