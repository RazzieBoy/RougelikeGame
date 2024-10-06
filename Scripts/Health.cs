using Godot;
using System;

public partial class Health : Node2D{
	
	//Allows for debuggins and changing hp in the inspector
	[Export] public float maxHealth = 10f;
	float health;
	
	//Set's the health to the max
	public override void _Ready(){
		health = maxHealth;
	}
	
	//Deals damage to user when hit and if they have no hp left they get destroyed
	public void Damage(float damage){
		health -= damage;
		if (health <= 0){
			GetParent().QueueFree();
		}
	}
}
