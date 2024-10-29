using Godot;
using System;

public partial class PlayerHud : Control{
	private Label primaryAmmoLabel;
	private Label secondaryAmmoLabel;
	private Label dashCooldownLabel;
	private Label primaryCooldownLabel;

	public override void _Ready(){
		primaryAmmoLabel = GetNode<Label>("Primary");
		secondaryAmmoLabel = GetNode<Label>("Secondary");
		dashCooldownLabel = GetNode<Label>("Utility");
		primaryCooldownLabel = GetNode<Label>("Special");
	}

	// Method to update ammo display, called by Main.cs
	public void UpdateAmmoDisplayPrimary(object sender, int primaryAmmoCount){
		primaryAmmoLabel.Text = $"{primaryAmmoCount}";
	}
	
	public void UpdateAmmoDisplaySecondary(object sender, int secondaryAmmoCount){
		secondaryAmmoLabel.Text = $"{secondaryAmmoCount}";
	}
	
	 public void UpdateDashCooldownDisplay(object sender, float cooldownTime){
		dashCooldownLabel.Text = $"{cooldownTime:F1}s";  // Dash cooldown
	}
	
	public void UpdateCooldownDisplayPrimary(object sender, float cooldownTime){
		primaryCooldownLabel.Text = $"{cooldownTime:F1}s";  // Primary cooldown only
	}
	
	public override void _Process(double delta){
	}
}
