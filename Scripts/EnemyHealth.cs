using Godot;
using System;

// Ensure EnemyHealth inherits from Health
public partial class EnemyHealth : Health
{
	public override void Damage(float damage){
		if (GetParent().IsInGroup("enemy")){ // Only takes damage from players{
			GD.Print("Calling base damage on enemy or player"); // Debug
			base.Damage(damage); // This will call the Health class's Damage method
		}
	}
}
