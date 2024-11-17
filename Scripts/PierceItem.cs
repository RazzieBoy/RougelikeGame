using Godot;
using System;

public partial class PierceItem : Node2D{
	[Export] public float piercingDuration = 10f;
	[Export] public int piercingCount = 1; // Number of enemies the bullet can pierce

	public override void _Ready(){
		// Connect the signal to detect the player
		this.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node body){
		if (body.IsInGroup("player")){
			// Assuming you have a reference to the player's Gun script
			Gun playerGun = body.GetNode<Gun>("Gun");
			playerGun.EnablePiercing(3);  // Set the piercing count when the player picks up the item
			QueueFree();  // Remove the item after pickup
		}
	}
}
