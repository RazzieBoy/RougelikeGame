using Godot;
using System;

public partial class PlayerHud : Control{
	private Label primaryAmmoLabel;
	private Label secondaryAmmoLabel;
	private Label dashCooldownLabel;
	private Label primaryCooldownLabel;
	private ColorRect healthBar;

	public override void _Ready(){
		primaryAmmoLabel = GetNode<Label>("PrimaryBackground/Primary");
		secondaryAmmoLabel = GetNode<Label>("SecondaryBackground/Secondary");
		dashCooldownLabel = GetNode<Label>("UtilityBackground/Utility");
		primaryCooldownLabel = GetNode<Label>("SpecialBackground/Special");
		healthBar = GetNode<ColorRect>("HealthBar");
	}
	
	public void UpdateHealthBar(float currentHealth, float maxHealth){
		// Calculate the health percentage and update the ColorRect size
		float healthPercentage = currentHealth / maxHealth;
		healthBar.Scale = new Vector2(healthPercentage, 1); // Adjust size based on health percentage
		// Optionally, change color based on health
		Color healthColor = new Color(223f / 255f, 149f / 255f, 255f / 255f); // #df95ff
		healthBar.Color = healthColor; // Set to the specified color
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
	
	public override void _Process(double delta){
	}
}
