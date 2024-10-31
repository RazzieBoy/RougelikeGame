using Godot;
using System;

public partial class Health : Node2D
{
	[Export] public float maxHealth = 10f;
	protected float health;

	public override void _Ready(){
		health = maxHealth;
		GD.Print($"{GetParent().Name} initialized with {health} health.");
	}

	public virtual void Damage(float damage){
		health -= damage;
		GD.Print($"{GetParent().Name} took {damage} damage. Remaining health: {health}");

		if (health <= 0){
			GD.Print($"{GetParent().Name} has died.");
			GetParent().QueueFree();
		}
	}

	public void Heal(float amount){
		health += amount;
		if (health > maxHealth) health = maxHealth;
	}
	// Create a property for current health value
	public float HealthValue => health; 
}
