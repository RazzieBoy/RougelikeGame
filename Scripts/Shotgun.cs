using Godot;
using System;

public partial class Shotgun : Node2D{
	[Export] PackedScene bulletScene;
	[Export] float bulletSpeed = 700f;
	[Export] float bps = 5;
	[Export] float bulletDamage = 2f;
	
	public float secondaryAmmoCount = 3;
	bool isReloading = false;
	float fireRate;
	float attackCooldown = 0f;
	float ReloadTime = 2f;
	
	public event EventHandler<int> AmmoUpdatedEventHandler;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		fireRate = 1 / bps;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		//Players secondary Attack methods used via the Left mouse button
		if (Input.IsActionPressed("secondary") && attackCooldown > fireRate){
			if (!isReloading && secondaryAmmoCount > 0){
				RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
			
				bullet.Rotation = GlobalRotation;
				bullet.GlobalPosition = GlobalPosition;
				bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;
				GetTree().Root.AddChild(bullet);
				attackCooldown = 0f;
				secondaryAmmoCount--;
				UpdateAmmo();
			}
			if	(secondaryAmmoCount <= 0){
			}
		}
		else{
			attackCooldown += (float)delta;
		}
		if (Input.IsActionPressed("reload")){
			secondaryAmmoCount = 3;
			UpdateAmmo();
		}
	}
	
	private void UpdateAmmo(){
		AmmoUpdatedEventHandler?.Invoke(this, (int)secondaryAmmoCount);
	}
}
