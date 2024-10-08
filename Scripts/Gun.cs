using Godot;
using System;

public partial class Gun : Node2D{
	
	[Export] PackedScene bulletScene;
	[Export] float bulletSpeed = 700f;
	[Export] float bps = 5;
	[Export] float bulletDamage = 2f;
	
	float reload = 10;
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
		if (Input.IsActionPressed("shoot") && attackCooldown > fireRate){
			if (!isReloading && reload > 0){
				RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
			
				bullet.Rotation = GlobalRotation;
				bullet.GlobalPosition = GlobalPosition;
				bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;
				GetTree().Root.AddChild(bullet);
				attackCooldown = 0f;
			}
			reload--;
			if	(reload <= 0){
				StartReload();
			}
		}
		else{
			attackCooldown += (float)delta;
		}
	}
	
	private void StartReload(){
		isReloading = true;
		reload = 10;
		Timer timer = new Timer();
		AddChild(timer);
		timer.WaitTime = ReloadTime;
		timer.OneShot = true;
		timer.Connect("timeout", this, nameof(OnReloadComplete));
		timer.Start();
	}
	
	private void OnReloadComplete(){
		isReloading = false;
	}
}
