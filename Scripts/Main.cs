using Godot;
using System;

public partial class Main : Node2D{
	private Player player;
	private PlayerHud playerHud;
	private DeathDisplay deathDisplay;

	public override void _Ready(){
		// Reference the player, PlayerHud and Death Display nodes
		player = GetNode<Player>("Player"); // Adjust this path as needed
		playerHud = GetNode<PlayerHud>("PlayerHUD"); // Adjust this path as needed
		deathDisplay = GetNode<DeathDisplay>("DeathDisplay");

		//Connects the primary's ammo update event to the PlayerHud display
		var gun = player.GetNode<Gun>("Gun");
		gun.AmmoUpdatedEventHandler += playerHud.UpdateAmmoDisplayPrimary;
		gun.CooldownUpdatedEventHandler += playerHud.UpdateCooldownDisplayPrimary;
		
		//Connects the secondary's ammo update event to the PlayerHud display
		var shotgun = player.GetNode<Shotgun>("Shotgun/Barrel1");
		shotgun.AmmoUpdatedEventHandler += playerHud.UpdateAmmoDisplaySecondary;
		
		//Connects the dashcooldown from the Player script to the PlayerHud display
		player.DashCooldownUpdatedEventHandler += playerHud.UpdateDashCooldownDisplay;
		
		//Connects the players health to the Playerhud display
		var playerHealth = player.GetNode<PlayerHealth>("Health"); // Adjust path as needed
		playerHealth.HealthUpdatedEventHandler += OnHealthUpdated;
		
		//Player information event updates
		player.HealthUpdatedEventHandler += playerHud.UpdateHealthBar;
		player.SpeedUpdatedEventHandler += playerHud.UpdateSpeedDisplay;
		player.DamageUpdatedEventHandler += playerHud.UpdateDamageDisplay;

		// Initialize ammo display's and ability cooldowns on start
		playerHud.UpdateAmmoDisplayPrimary(this, (int)gun.primaryAmmoCount);
		playerHud.UpdateCooldownDisplayPrimary(this, gun.frenzyCooldown);
		playerHud.UpdateAmmoDisplaySecondary(this, (int)shotgun.secondaryAmmoCount);
		playerHud.UpdateDashCooldownDisplay(this, player.dashCooldown);
	}
	
	private void OnHealthUpdated(object sender, (float currentHealth, float maxHealth) healthInfo){
		playerHud.UpdateHealthBar(sender, healthInfo);
		if (healthInfo.currentHealth <= 0 && deathDisplay != null){
			deathDisplay.Show();
		}
	}
}
