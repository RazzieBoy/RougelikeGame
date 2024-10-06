using Godot;
using System;

public partial class Gun : Node2D{
	
	[Export] PackedScene bulletScene;
	[Export] float bulletSpeed = 700f;
	[Export] float bps = 5;
	[Export] float bulletDamage = 2f;
	
	float fireRate;
	float attackCooldown = 0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		fireRate = 1 / bps;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		if (Input.IsActionPressed("shoot") && attackCooldown > fireRate){
			RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
			
			bullet.Rotation = GlobalRotation;
			bullet.GlobalPosition = GlobalPosition;
			bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;
			GetTree().Root.AddChild(bullet);
			attackCooldown = 0f;
		}
		else{
			attackCooldown += (float)delta;
		}
	}
}
