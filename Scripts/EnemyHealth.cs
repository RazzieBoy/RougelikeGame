using Godot;
using System;

// Ensure EnemyHealth inherits from Health
public partial class EnemyHealth : Health{
	public override void Damage(float damage){
		//Makes sure to check to only take damage from the player.
		if (GetParent().IsInGroup("enemy")){ 
			//GD.Print("Calling base damage on enemy or player");
			//This will call the Health class's Damage method
			base.Damage(damage);
		}
	}
}
