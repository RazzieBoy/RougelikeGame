using Godot;
using System;

public partial class Gun2 : Node2D{
	
	[Export] PackedScene bulletScene;
	[Export] float bulletSpeed = 700f;
	[Export] float bps = 5;
	[Export] float bulletDamage = 2f;
	
	float primaryAmmoCount = 10;
	bool isReloading = false;
	float fireRate;
	float attackCooldown = 0f;
	float ReloadTime = 2f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		fireRate = 1 / bps;
		//Timer reloadTimer = GetNode<Timer>("ReloadTimer");
		//reloadTimer.Connect("timeout", this, "_on_timer_timeout");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		//Players primary Attack methods used via the Left mouse button
		if (Input.IsActionPressed("secondary") && attackCooldown > fireRate){
			if (!isReloading && primaryAmmoCount > 0){
				RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
			
				bullet.Rotation = GlobalRotation;
				bullet.GlobalPosition = GlobalPosition;
				bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;
				GetTree().Root.AddChild(bullet);
				attackCooldown = 0f;
			}
			primaryAmmoCount--;
			if	(primaryAmmoCount <= 0){
			}
		}
		else{
			attackCooldown += (float)delta;
		}
		if (Input.IsActionPressed("reload")){
			primaryAmmoCount = 10;
		}
	}
}
