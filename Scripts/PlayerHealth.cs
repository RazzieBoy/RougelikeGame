using Godot;
using System;

public partial class PlayerHealth : Health
{
	public new float HealthValue => health;

	// Ensure you have using System; at the top for EventHandler
	public event EventHandler<(float currentHealth, float maxHealth)> HealthUpdatedEventHandler;

	public override void Damage(float damage){
		if (GetParent().IsInGroup("player")){ // Only takes damage from enemies
			base.Damage(damage);
			GD.Print("PlayerDamage");
			// Notify listeners about health update after taking damage
			HealthUpdatedEventHandler?.Invoke(this, (health, maxHealth));
		}
	}

	public new void Heal(float amount){
		health += amount;
		if (health > maxHealth) health = maxHealth;
		// Notify listeners about health update after healing
		HealthUpdatedEventHandler?.Invoke(this, (health, maxHealth));
	}
}
