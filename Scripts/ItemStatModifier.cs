using Godot;
using System;

public partial class ItemStatModifier : Node2D{
	[Export] public float speedModifier = 0f;
	[Export] public float damageModifier = 0f;
	
	public virtual void ApplyModification(Player player){
		player.ModifySpeed(speedModifier);
		player.ModifyDamage(damageModifier);
	}
}
