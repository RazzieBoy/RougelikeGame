using Godot;
using System;

public partial class Gun : Node2D{
	
	[Export] PackedScene bulletScene;
	[Export] float bulletSpeed = 800f;
	[Export] float bps = 5;
	[Export] public float bulletDamage = 2f;
	[Export] float frenzyTime = 3f;
	[Export] public float frenzyCooldown = 30f; 
	
	bool isReloading = false;
	bool isFrenzy = false;
	public float primaryAmmoCount = 10;
	float fireRate;
	float attackCooldown = 0f;
	float reloadTime;
	private float frenzyTimeLeft = 0f;
	private float frenzyCooldownLeft = 0f;
	private float storedBps;
	
	private bool piercingEnabled = false;
	private int piercingCount = 1;
	
	public event EventHandler<int> AmmoUpdatedEventHandler;
	public event EventHandler<float> CooldownUpdatedEventHandler;
	
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
			if (frenzyCooldownLeft >= 0){
				frenzyCooldownLeft -= (float)delta;
				CooldownUpdatedEventHandler?.Invoke(this, frenzyCooldownLeft);
				//GD.Print(frenzyCooldownLeft);dw
			}
			else{
				CooldownUpdatedEventHandler?.Invoke(this, 30);
			}
		}
		//Players primary Attack methods used via the Left mouse button
		if (Input.IsActionPressed("primairy") && !Input.IsActionPressed("secondary") && attackCooldown > fireRate && reloadTime <= 0){
			if (!isReloading && (primaryAmmoCount > 0 || isFrenzy)){
				RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
				bullet.Rotation = GlobalRotation;
				bullet.GlobalPosition = GlobalPosition;
				bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;
				// Set bullet's damage based on gun's bulletDamage
				Bullet bulletScript = bullet as Bullet; // Assuming Bullet script is attached to the bullet scene
				if (bulletScript != null){
					bulletScript.damage = bulletDamage;
					bulletScript.piercingCount = piercingEnabled ? piercingCount : 0; 
					//GD.Print("Bullet damage set to: " + bulletDamage); // For debugging
				}
				GetTree().Root.AddChild(bullet);
				attackCooldown = 0f;
				if (!isFrenzy){
					primaryAmmoCount--;
					UpdateAmmo();
				}
			}
		}
		else{
			attackCooldown += (float)delta;
		}
		if (Input.IsActionPressed("reload") && reloadTime <= 0){
			primaryAmmoCount = 10;
			UpdateAmmo();
			reloadTime = 2;
		}
		else{
			reloadTime -= (float)delta;
		}
		if (Input.IsActionPressed("special") && frenzyCooldownLeft <= 0){
			isFrenzy = true;
			frenzyTimeLeft = frenzyTime;
			bps = storedBps * 3;
			fireRate = 1 / bps;
		}
	}
	
	private void UpdateAmmo(){
		AmmoUpdatedEventHandler?.Invoke(this, (int)primaryAmmoCount);
	}
	
	// Call this method to enable piercing when the player picks up the item
	 // Method to enable piercing when the player picks up the item
	public void EnablePiercing(int count){
		piercingEnabled = true;
		piercingCount = count; // Set the piercing count to the provided value
	}

	public void DisablePiercing(){
		piercingEnabled = false;
	}
}
