using Godot;
using System;

public partial class Bullet : RigidBody2D{
	//Float that holds the value of the bullets damage
	[Export] public float damage = 2f;
	public int piercingCount = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		//Destroys the bullet after it has lived existed for x amount of time.
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += () => QueueFree();
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
	}
	//Checks if the bullets collide with the enemies, if so they send the damage value to the enemy which was hit
	//and then destroys the bullet
	public void OnBodyEntered(Node2D body){
		if (body.IsInGroup("enemy")){
			var health = body.GetNodeOrNull<EnemyHealth>("Health");
			if (health != null){
				health.Damage(damage);
				piercingCount--;  // Decrease the piercing count
				if (piercingCount <= 0) {
					QueueFree();  // Destroy the bullet when piercing count reaches 0
				}
			}
		}
	}
}
