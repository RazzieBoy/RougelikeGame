using Godot;
using System;

public partial class Gun : Node2D{
	
	[Export] PackedScene bulletScene;
	[Export] float bulletSpeed = 700f;
	[Export] float bps = 5;
	[Export] float bulletDamage = 2f;
	[Export] float frenzyTime = 3f;
	[Export] float frenzyCooldown = 30f; 
	
	bool isReloading = false;
	bool isFrenzy = false;
	
	float primaryAmmoCount = 10;
	float fireRate;
	float attackCooldown = 0f;
	float ReloadTime = 2f;
	private float frenzyTimeLeft = 0f;
	private float frenzyCooldownLeft = 0f;
	private float storedBps;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		storedBps = bps;
		fireRate = 1 / bps;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		if (isFrenzy){
			frenzyTimeLeft -= (float)delta;
			if (frenzyTimeLeft <= 0){
				isFrenzy = false;
				frenzyCooldownLeft = frenzyCooldown;
				bps = storedBps;
				fireRate = 1 / bps;
			}
		}
		else{
			if (frenzyCooldownLeft > 0){
				frenzyCooldownLeft -= (float)delta;
				GD.Print(frenzyCooldownLeft);
			}
		}
		//Players primary Attack methods used via the Left mouse button
		if (Input.IsActionPressed("primairy") && attackCooldown > fireRate){
			if (!isReloading && (primaryAmmoCount > 0 || isFrenzy)){
				RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
			
				bullet.Rotation = GlobalRotation;
				bullet.GlobalPosition = GlobalPosition;
				bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;
				GetTree().Root.AddChild(bullet);
				attackCooldown = 0f;
				
				if (!isFrenzy){
					primaryAmmoCount--;
				}
			}
		}
		else{
			attackCooldown += (float)delta;
		}
		if (Input.IsActionPressed("reload")){
			primaryAmmoCount = 10;
		}
		if (Input.IsActionPressed("special") && frenzyCooldownLeft <= 0){
			isFrenzy = true;
			frenzyTimeLeft = frenzyTime;
			bps = storedBps * 3;
			fireRate = 1 / bps;
		}
	}
}
