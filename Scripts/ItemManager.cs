using Godot;
using System;
using System.Collections.Generic;

public partial class ItemManager : Node2D{
	
	[Export] public PackedScene SpeedItemScene {get; set;}
	[Export] public PackedScene DamageItemScene {get; set;}
	
	private List<PackedScene> itemList;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		itemList = new List<PackedScene> { SpeedItemScene, DamageItemScene };
	}

	//public void AddItem(PackedScene itemScene){ // Ensure the method name is consistent
		//itemList.Add(itemScene);
	//}

	public PackedScene GetRandomItem(float spawnChance)
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		if (rng.Randf() <= spawnChance && itemList.Count > 0)
		{
			int randomIndex = rng.RandiRange(0, itemList.Count - 1);
			GD.Print("Selected item index: " + randomIndex);
			return itemList[randomIndex];
		}

		GD.Print("No item selected due to probability check.");
		return null; // No item selected
}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
