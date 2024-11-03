using Godot;
using System;
using System.Collections.Generic;

public partial class ItemManager : Node2D{
	private List<PackedScene> itemList = new List<PackedScene>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		AddItem(ResourceLoader.Load<PackedScene>("res://Entities/speedItem.tscn"));
		AddItem(ResourceLoader.Load<PackedScene>("res://Entities/damageItem.tscn"));
	}

	public void AddItem(PackedScene itemScene){ // Ensure the method name is consistent
		itemList.Add(itemScene);
	}

	public PackedScene GetRandomItem(float spawnChance){
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		if (rng.Randf() <= spawnChance){
			int randomIndex = (int)(rng.Randi() % itemList.Count);
			return itemList[randomIndex];
		}
		return null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
