using Godot;
using System;

public partial class DamageItem : ItemStatModifier{
	//Sets the modifier values
	public override void _Ready(){
		speedModifier = 0f;
		damageModifier = 1f;
		GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
	}
	//Checks if the player has touched the item, if so it is deleted from view
	private void OnBodyEntered(Node body){
		if (body is Player player){
			ApplyModification(player);
			QueueFree(); // Remove the item after it's picked up
		}
	}
}
