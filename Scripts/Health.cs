using Godot;
using System;

public partial class Health : Node2D{
	
	[Export] public float maxHealth = 10f;
	float health;
	
	public override void _Ready(){
		health = maxHealth;
	}
	
	public void Damage(float damage){
		health -= damage;
		
		if (health <- 0){
			GetParent().QueueFree();
		}
	}
	
}
