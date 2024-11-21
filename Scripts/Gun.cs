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
		fireRate = 2 / bps;
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
		
		//Checks if the player is only clicking the eft mouse button to shoot and if it has enough ammo
		if (Input.IsActionPressed("primairy") && !Input.IsActionPressed("secondary") && attackCooldown > fireRate && reloadTime <= 0){
			//Check to see if the player isn't reloading
			if (!isReloading && (primaryAmmoCount > 0 || isFrenzy)){
				//Sets the orientation of the bullet and it's position to be accurate for the player, so it doesn't shoot from the wrong location
				RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();
				bullet.Rotation = GlobalRotation;
				bullet.GlobalPosition = GlobalPosition;
				bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;
				// Set bullet's damage based on gun's bulletDamage
				Bullet bulletScript = bullet as Bullet;
				if (bulletScript != null){
					bulletScript.damage = bulletDamage;
				}
				//Makes the fired bullets to children nodes until removed
				GetTree().Root.AddChild(bullet);
				attackCooldown = 0f;
				//Check to see if the player isn't using their special
				if (!isFrenzy){
					primaryAmmoCount--;
					UpdateAmmo();
				}
			}
		}
		else{
			attackCooldown += (float)delta;
		}
		//Check to see if the player wants to reload and sets the ammo to maximum capacity
		if (Input.IsActionPressed("reload") && reloadTime <= 0){
			primaryAmmoCount = 10;
			UpdateAmmo();
			reloadTime = 2;
		}
		else{
			reloadTime -= (float)delta;
		}
		//Check to see if the player can use their special ability
		if (Input.IsActionPressed("special") && frenzyCooldownLeft <= 0){
			isFrenzy = true;
			frenzyTimeLeft = frenzyTime;
			bps = storedBps * 3;
			fireRate = 1 / bps;
		}
	}
	
	//Function that sends the current ammo count to the playerhud
	private void UpdateAmmo(){
		AmmoUpdatedEventHandler?.Invoke(this, (int)primaryAmmoCount);
	}
	
	public void EnablePiercing(int count){
		piercingEnabled = true;
		piercingCount = count; 
	}

	public void DisablePiercing(){
		piercingEnabled = false;
	}
}
