using Godot;
using System;

public partial class EnemyBullet : RigidBody2D{
	
	[Export] public float damage = 2f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		//Destroys the bullet after it has lived existed for x amount of time.
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += () => QueueFree();
		}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
	}
	//Checks if the bullets collide with the enemies, if so they send the damage value to the enemy which was hit
	//and then destroys the bullet
	public void OnBodyEntered(Node2D body){
		if (body.IsInGroup("player")){
			body.GetNode<Health>("Health").Damage(damage);
		}
		QueueFree();
	}
}
