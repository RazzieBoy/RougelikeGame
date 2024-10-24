using Godot;
using System;

public partial class EnemyGun : Node2D{
	
	[Export] PackedScene bulletScene;
	[Export] float bulletSpeed = 700f;
	[Export] float shootCooldown = 1f;
	
	private Node2D player;
	private float attackCooldown = 0f;

	public override void _Ready(){
		// Assuming the player node is named "Player" and is a sibling or child of the enemy node
		player = (Player)GetTree().Root.GetNode("Main").GetNode("Player");
	}

	public override void _Process(double delta){
		if (player != null){
			// Calculate direction to the player
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Rotation = direction.Angle();
			// Shoot at intervals
			attackCooldown -= (float)delta;
			if (attackCooldown <= 0f){
				Shoot(direction);
				attackCooldown = shootCooldown;
			}
		}
	}

	private void Shoot(Vector2 direction){
		RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
		// Set the bullet's initial position and direction
		bullet.GlobalPosition = GlobalPosition;
		bullet.LinearVelocity = direction * bulletSpeed;
		// Add the bullet to the scene
		GetTree().Root.AddChild(bullet);
	}
}
