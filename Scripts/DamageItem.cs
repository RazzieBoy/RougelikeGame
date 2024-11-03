using Godot;
using System;

public partial class DamageItem : ItemStatModifier{
	public override void _Ready(){
		speedModifier = 0f;
		damageModifier = 1f;
		GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
	}

	public override void ApplyModification(Player player){
		player.ModifyDamage(damageModifier);
		GD.Print("Applied damage modifier of " + damageModifier);
	}

	private void OnBodyEntered(Node body){
		if (body is Player player){
			ApplyModification(player);
			QueueFree(); // Remove the item after it's picked up
		}
	}
}
