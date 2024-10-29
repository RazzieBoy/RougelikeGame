using Godot;
using System;

public partial class PlayerHud : Control{
	private Label primaryAmmoLabel;
	private Label secondaryAmmoLabel;

	public override void _Ready(){
		primaryAmmoLabel = GetNode<Label>("Primary");
		secondaryAmmoLabel = GetNode<Label>("Secondary");
	}

	// Method to update ammo display, called by Main.cs
	public void UpdateAmmoDisplayPrimary(object sender, int primaryAmmoCount){
		primaryAmmoLabel.Text = $"{primaryAmmoCount}";
	}
	
	public void UpdateAmmoDisplaySecondary(object sender, int secondaryAmmoCount){
		secondaryAmmoLabel.Text = $"{secondaryAmmoCount}";
	}
	
	public override void _Process(double delta){
	}
}
