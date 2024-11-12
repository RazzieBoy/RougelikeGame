using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D{
	//Get's enemy and item entities for later use
	[Export] public PackedScene meleeEnemyScene;
	[Export] public PackedScene rangedEnemyScene;
	[Export] public PackedScene miniBossEnemyScene;
	[Export] public PackedScene speedItemScene;
	[Export] public PackedScene damageItemScene;
	[Export] public PackedScene healthItemScene;
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
	public float currentLevel;
	//Labels that are specific to the enemy spawner and it's actions
	private Label levelLabel;
	private Label nextWave;
	//Bools that checks is enemies have spawned
	private bool hasEnemiesSpawned = false;
	private bool hasMiniBossSpawned = false;
	//List for enemies that are alive
	private List<Node2D> activeEnemies = new List<Node2D>();
	private List<Node2D> activeItems = new List<Node2D>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		//Function that sets where items and enemies can spawn.
		UpdateSpawnArea();
		currentLevel = 1;
		//Sets the max enemy spawn count to 5 at the start
		maxSpawnCount = 5;
		//Sets the spawn cooldown to the spawnrate
		spawnCooldown = spawnRate;
		levelLabel = GetNode<Label>("LevelLabel");
		nextWave = GetNode<Label>("NextWave");
		//Updates the label informations
		UpdateLevelLabel();
		UpdateNextWave();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		//Checks that all requirements are met for spawning enemies. 
		//This makes sure so the all enemies are spawned
		if (activeItems.Count == 0 && spawnCooldown <= 0 && totalSpawnedEnemies < maxSpawnCount && !hasEnemiesSpawned){
			hasEnemiesSpawned = true;
		}
		//If there is no item to pickup, countdown for spawning enemies
		else if(activeItems.Count == 0){
			spawnCooldown -= (float)delta;
			UpdateNextWave();
		}
		//Spawn the enemies
		if (hasEnemiesSpawned && spawnCooldown <= 0){
			if (currentLevel % 5 == 0){
				if (!hasMiniBossSpawned){
					SpawnMiniBoss();
					hasMiniBossSpawned = true;
					totalSpawnedEnemies = maxSpawnCount;
				}
			}
			else{
				SpawnEnemy();
			}
			hasEnemiesSpawned = false;
		}
		
		activeEnemies.RemoveAll(enemy => !IsInstanceValid(enemy));
		if (activeEnemies.Count == 0 && totalSpawnedEnemies == maxSpawnCount){
			SpawnItem();
			spawnCooldown = 5f;
			totalSpawnedEnemies = 0f;
			maxSpawnCount = maxSpawnCount + 1;
			currentLevel += 1;
			hasMiniBossSpawned = false;
			UpdateLevelLabel(); // Ensure this call is made
		}
		activeItems.RemoveAll(item => !IsInstanceValid(item));
	}
	
	private void UpdateSpawnArea(){
		Rect2 visibleRect = GetViewport().GetVisibleRect();
		Vector2 screenSize = visibleRect.Size;
		
		float offset = 100f;
		spawnAreaMin = new Vector2(50, 50);
		spawnAreaMax = new Vector2(screenSize.X - 50, screenSize.Y - 200);
		spawnAreaMax.X = Mathf.Min(spawnAreaMax.X, screenSize.X - 50);
	}
	
	private void SpawnEnemy(){
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		
		PackedScene enemyScene = rng.RandiRange(0, 1) == 0 ? meleeEnemyScene : rangedEnemyScene;
		
		Vector2 spawnPosition = Vector2.Zero;
		Rect2 visibleRect = GetViewport().GetVisibleRect();
		Vector2 screenSize = visibleRect.Size;
		do {
			float edge = rng.RandiRange(0,3);
			switch(edge){
				case 0:
					spawnPosition = new Vector2(rng.RandfRange(-50, screenSize.X + 50), -50);
					break;
				case 1:
					spawnPosition = new Vector2(rng.RandfRange(-50, screenSize.X + 50), screenSize.Y + 50);
					break;
				case 2:
					spawnPosition = new Vector2(-50, rng.RandfRange(-50, screenSize.Y + 50));
					break;
				case 3:
					spawnPosition = new Vector2(screenSize.X + 50, rng.RandfRange(-50, screenSize.Y + 50));
					break;
			}
			//spawnPosition = new Vector2(rng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), rng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
		} while (player != null && player.Position.DistanceTo(spawnPosition) < 200);
		
		Node2D enemyInstance = enemyScene.Instantiate<Node2D>();
		enemyInstance.Position = spawnPosition;
		AddChild(enemyInstance);
		activeEnemies.Add(enemyInstance);
		totalSpawnedEnemies++; // Increment the total spawn count
	}
	
	private void SpawnMiniBoss(){
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		PackedScene enemyScene = miniBossEnemyScene;
		Vector2 miniBossSpawnPosition = Vector2.Zero;
		Rect2 visibleRect = GetViewport().GetVisibleRect();
		Vector2 screenSize = visibleRect.Size;
		do{
			float edge = rng.RandiRange(0,3);
			switch(edge){
				case 0:
					miniBossSpawnPosition = new Vector2(rng.RandfRange(-50, screenSize.X + 50), -50);
					break;
				case 1:
					miniBossSpawnPosition = new Vector2(rng.RandfRange(-50, screenSize.X + 50), screenSize.Y + 50);
					break;
				case 2:
					miniBossSpawnPosition = new Vector2(-50, rng.RandfRange(-50, screenSize.Y + 50));
					break;
				case 3:
					miniBossSpawnPosition = new Vector2(screenSize.X + 50, rng.RandfRange(-50, screenSize.Y + 50));
					break;
			}
		}  while (player != null && player.Position.DistanceTo(miniBossSpawnPosition) < 200);
		Node2D enemyInstance = enemyScene.Instantiate<Node2D>();
		enemyInstance.Position = miniBossSpawnPosition;
		AddChild(enemyInstance);
		activeEnemies.Add(enemyInstance);
	}
	
	private void SpawnItem(){
		RandomNumberGenerator itemRng = new RandomNumberGenerator();
		itemRng.Randomize();
		int itemChoice = itemRng.RandiRange(0,2);
		GD.Print($"Item: {itemChoice} has spawned");
		
		PackedScene itemScene = itemChoice switch{
			0 => speedItemScene,
			1 => damageItemScene,
			2 => healthItemScene,
			_ => speedItemScene
		};
		
		Vector2 itemSpawnPosition;
		do{
			itemSpawnPosition = new Vector2(itemRng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), itemRng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
		} while (player != null && player.Position.DistanceTo(itemSpawnPosition) < 50);
		
		Node2D itemInstance = itemScene.Instantiate<Node2D>();
		itemInstance.Position = itemSpawnPosition;
		AddChild(itemInstance);
		activeItems.Add(itemInstance);
		
		if (itemRng.Randf() <= 0.1f){
			List<int> remainingChoices = new List<int> {0, 1, 2};
			remainingChoices.Remove(itemChoice);
			
			int extraItemChoice = remainingChoices[itemRng.RandiRange(0, remainingChoices.Count - 1)];
			PackedScene extraItemScene = extraItemChoice switch{
				0 => speedItemScene,
				1 => damageItemScene,
				2 => healthItemScene,
				_ => speedItemScene
			};
			
			Vector2 extraItemSpawnPosition;
			do{
				extraItemSpawnPosition = new Vector2(itemRng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), itemRng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
			} while (player != null && player.Position.DistanceTo(extraItemSpawnPosition) < 50);
				
			Node2D extraItemInstance = extraItemScene.Instantiate<Node2D>();
			extraItemInstance.Position = extraItemSpawnPosition;
			AddChild(extraItemInstance);
			activeItems.Add(extraItemInstance);
		}
	}
	
	private void UpdateLevelLabel(){
		if (levelLabel != null){
			levelLabel.Text = $"LEVEL: {currentLevel}";
		}
	}
	
	private void UpdateNextWave(){
		if (nextWave != null && spawnCooldown >= 0){
			nextWave.Show();
			nextWave.Text = $"NEXT WAVE IN: {spawnCooldown:F0}";
		}
		else{
			nextWave.Hide();
		}
	}
}
