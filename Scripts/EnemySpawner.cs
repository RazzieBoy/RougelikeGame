using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D{
	//Get's enemy and item entities for later use
	[Export] public PackedScene meleeEnemyScene;
	[Export] public PackedScene rangedEnemyScene;
	[Export] public PackedScene speedItemScene;
	[Export] public PackedScene damageItemScene;
	//Floats that hold how quick the enemies can spawn and hoow big the limit is.
	[Export] public float spawnRate = 2f;
	[Export] public float maxSpawnCount;
	//Vectors that determine the size of the area where enemies and items can spawn
	private Vector2 spawnAreaMin;
	private Vector2 spawnAreaMax;
	//Get's the player node so it can be referenced easily
	[Export] public Node2D player;
	//Floats for how many enemies have spawned and sets the cooldown to 0
	private float spawnCooldown = 0f;
	private float totalSpawnedEnemies = 0;
	//Bools that checks is enemies have spawned
	private bool hasEnemiesSpawned = false;
	//List for enemies that are alive
	private List<Node2D> activeEnemies = new List<Node2D>();
	private List<Node2D> activeItems = new List<Node2D>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		//Sets the max enemy spawn count to 5 at the start
		maxSpawnCount = 5;
		if (player == null) {
			//GD.PrintErr("Player node is not assigned!");
		}
		//Sets the spawn cooldown to the spawnrate
		spawnCooldown = spawnRate;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		 UpdateSpawnArea();
		
		
		
		if (activeItems.Count == 0 && spawnCooldown <= 0 && totalSpawnedEnemies < maxSpawnCount && !hasEnemiesSpawned){
			hasEnemiesSpawned = true;
		}
		else if(activeItems.Count == 0){
			spawnCooldown -= (float)delta;
		}
		if (hasEnemiesSpawned && spawnCooldown <= 0){
			SpawnEnemy();
			hasEnemiesSpawned = false;
		}
		
		activeEnemies.RemoveAll(enemy => !IsInstanceValid(enemy));
		if (activeEnemies.Count == 0 && totalSpawnedEnemies == maxSpawnCount){
			SpawnItem();
			spawnCooldown = 5f;
			totalSpawnedEnemies = 0f;
			maxSpawnCount = maxSpawnCount + 1;
		}
		activeItems.RemoveAll(item => !IsInstanceValid(item));
	}
	
	private void UpdateSpawnArea(){
		Rect2 visibleRect = GetViewport().GetVisibleRect();
		Vector2 screenSize = visibleRect.Size;
		
		spawnAreaMin = new Vector2(50, 50);
		spawnAreaMax = new Vector2(screenSize.X - 50, screenSize.Y - 200);
		spawnAreaMax.X = Mathf.Min(spawnAreaMax.X, screenSize.X - 50);
	}
	
	private void SpawnEnemy(){
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		
		PackedScene enemyScene = rng.RandiRange(0, 1) == 0 ? meleeEnemyScene : rangedEnemyScene;
		
		//Vector2 spawnPosition = new Vector2(rng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), rng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
		Vector2 spawnPosition;
		do {
			spawnPosition = new Vector2(rng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), rng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
		} while (player != null && player.Position.DistanceTo(spawnPosition) < 200);
		
		Node2D enemyInstance = enemyScene.Instantiate<Node2D>();
		enemyInstance.Position = spawnPosition;
		AddChild(enemyInstance);
		activeEnemies.Add(enemyInstance);
		
		totalSpawnedEnemies++; // Increment the total spawn count
	}
	
	private void SpawnItem(){
		RandomNumberGenerator itemRng = new RandomNumberGenerator();
		itemRng.Randomize();
		
		PackedScene itemScene = itemRng.RandiRange(0,1) == 0 ? speedItemScene : damageItemScene;
		
		//Vector2 itemSpawnPosition = new Vector2(itemRng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), itemRng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
		Vector2 itemSpawnPosition;
		do{
			itemSpawnPosition = new Vector2(itemRng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), itemRng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
		} while (player != null && player.Position.DistanceTo(itemSpawnPosition) < 50);
		
		Node2D itemInstance = itemScene.Instantiate<Node2D>();
		itemInstance.Position = itemSpawnPosition;
		AddChild(itemInstance);
		activeItems.Add(itemInstance);
	}
}
