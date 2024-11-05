using Godot;
using System;

public partial class EnemySpawner : Node2D{
	[Export] public PackedScene meleeEnemyScene;
	[Export] public PackedScene rangedEnemyScene;
	[Export] public float spawnRate = 2f;
	[Export] public Vector2 spawnAreaMin;
	[Export] public Vector2 spawnAreaMax;
	
	private float spawnCooldown = 0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		if (spawnCooldown <= 0){
			SpawnEnemy();
			spawnCooldown = spawnRate;
		}
		else
		{
			spawnCooldown -= (float)delta;
		}
	}
	
	private void SpawnEnemy(){
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		
		PackedScene enemyScene = rng.RandiRange(0, 1) == 0 ? meleeEnemyScene : rangedEnemyScene;
		
		Vector2 spawnPosition = new Vector2(rng.RandfRange(spawnAreaMin.X, spawnAreaMax.X), rng.RandfRange(spawnAreaMin.Y, spawnAreaMax.Y));
		
		Node2D enemyInstance = enemyScene.Instantiate<Node2D>();
		enemyInstance.Position = spawnPosition;
		AddChild(enemyInstance);
	}
}
