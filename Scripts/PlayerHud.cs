using Godot;
using System;

public partial class PlayerHud : Control{
	private Label primaryAmmoLabel;
	private Label secondaryAmmoLabel;
	private Label dashCooldownLabel;
	private Label primaryCooldownLabel;
	private ColorRect healthBar;
	private Label healthLabel;
	private Label speedLabel;
	private Label damageLabel;

	public override void _Ready(){
		primaryAmmoLabel = GetNode<Label>("PrimaryBackground/Primary");
		secondaryAmmoLabel = GetNode<Label>("SecondaryBackground/Secondary");
		dashCooldownLabel = GetNode<Label>("UtilityBackground/Utility");
		primaryCooldownLabel = GetNode<Label>("SpecialBackground/Special");
		healthBar = GetNode<ColorRect>("HealthBar");
		healthLabel = GetNode<Label>("HealthLabel");
		speedLabel = GetNode<Label>("SpeedLabel");
		damageLabel = GetNode<Label>("DamageLabel");
	}
	
	public void UpdateHealthBar(object sender, (float currentHealth, float maxHealth) healthInfo){
		// Calculate the health percentage and update the ColorRect size
		float healthPercentage = healthInfo.currentHealth / healthInfo.maxHealth;
		healthBar.Scale = new Vector2(healthPercentage, 1); // Adjust size based on health percentage
		// Optionally, change color based on health
		Color healthColor = new Color(223f / 255f, 149f / 255f, 255f / 255f); // #df95ff
		healthBar.Color = healthColor; // Set to the specified color
		healthLabel.Text = $"HEALTH: {healthInfo.currentHealth:F0} / {healthInfo.maxHealth:F0}";
	}

	// Method to update ammo display, called by Main.cs
	public void UpdateAmmoDisplayPrimary(object sender, int primaryAmmoCount){
		primaryAmmoLabel.Text = $"{primaryAmmoCount}";
	}
	
	public void UpdateAmmoDisplaySecondary(object sender, int secondaryAmmoCount){
		secondaryAmmoLabel.Text = $"{secondaryAmmoCount}";
	}
	
	 public void UpdateDashCooldownDisplay(object sender, float cooldownTime){
		dashCooldownLabel.Text = $"{cooldownTime:F0}";  // Dash cooldown
	}
	
	public void UpdateCooldownDisplayPrimary(object sender, float cooldownTime){
		primaryCooldownLabel.Text = $"{cooldownTime:F0}";  // Primary cooldown only
	}
	
	public void UpdateSpeedDisplay(object sender, float newSpeed){
		speedLabel.Text = $"SPEED: {newSpeed}";
	}
	
	public void UpdateDamageDisplay(object sender, float newDamage){
		damageLabel.Text = $"DAMAGE: {newDamage}";
	}
	
	public override void _Process(double delta){
	}
}
