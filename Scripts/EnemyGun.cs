using Godot;
using System;

public partial class EnemyGun : Node2D{
	
	//Gets the bullet entity
	[Export] PackedScene bulletScene;
	//Floats that hold value of the bullet speed and how quick the gun can be fired
	[Export] float bulletSpeed = 700f;
	[Export] float shootCooldown = 1f;
	//Get's the player node so it can be referenced easily
	private Node2D player;
	//How quickly the enemy can attck
	private float attackCooldown = 0f;

	public override void _Ready(){
		//Stores information of the player Node in player
		player = (Player)GetTree().Root.GetNode("Main").GetNode("Player");
	}

	public override void _Process(double delta){
		if (player != null){
			// Calculate direction to the player
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Rotation = direction.Angle();
			// Shoots towards the at intervals
			attackCooldown -= (float)delta;
			if (attackCooldown <= 0f){
				Shoot(direction);
				attackCooldown = shootCooldown;
			}
		}
	}
	//Get's the bullet entity and gives it a speed and position that updates
	private void Shoot(Vector2 direction){
		RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
		// Set the bullet's initial position and direction
		bullet.GlobalPosition = GlobalPosition;
		bullet.LinearVelocity = direction * bulletSpeed;
		// Add the bullet to the scene
		GetTree().Root.AddChild(bullet);
	}
}
