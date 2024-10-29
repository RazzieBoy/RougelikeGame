using Godot;
using System;

public partial class Main : Node2D
{
	private Player player;
	private PlayerHud playerHud;

	public override void _Ready()
	{
		// Reference the player and PlayerHud nodes
		player = GetNode<Player>("Player"); // Adjust this path as needed
		playerHud = GetNode<PlayerHud>("PlayerHUD"); // Adjust this path as needed

		// Connect the gun's ammo update event to PlayerHud's display method
		var gun = player.GetNode<Gun>("Gun");
		gun.AmmoUpdatedEventHandler += playerHud.UpdateAmmoDisplayPrimary;
		gun.CooldownUpdatedEventHandler += playerHud.UpdateCooldownDisplayPrimary;
		
		var shotgun = player.GetNode<Shotgun>("Shotgun/Barrel1");
		shotgun.AmmoUpdatedEventHandler += playerHud.UpdateAmmoDisplaySecondary;
		
		player.DashCooldownUpdatedEventHandler += playerHud.UpdateDashCooldownDisplay;

		// Initialize ammo display on start
		playerHud.UpdateAmmoDisplayPrimary(this, (int)gun.primaryAmmoCount);
		playerHud.UpdateCooldownDisplayPrimary(this, gun.frenzyCooldown);
		playerHud.UpdateAmmoDisplaySecondary(this, (int)shotgun.secondaryAmmoCount);
		playerHud.UpdateDashCooldownDisplay(this, player.dashCooldown);
	}
}
