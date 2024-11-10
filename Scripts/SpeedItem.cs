using Godot;
using System;

public partial class SpeedItem : ItemStatModifier{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		speedModifier = 50f;
		damageModifier = 0f;
		healthModifier = 0f;
		GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body){
		if (body is Player player){
			ApplyModification(player);
			QueueFree(); // Remove the item after it's picked up
		}
	}
}
