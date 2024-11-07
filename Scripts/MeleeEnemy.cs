using Godot;
using System;

public partial class MeleeEnemy : CharacterBody2D{
	
	private Player player;
	private ItemManager itemManager;
	
	[Export] float speed = 250f;
	[Export] float damage = 2f;
	[Export] float aps = 2f;
	
	float attackRate;
	float attackCooldown;
	bool inRange = false;
	
	public override void _Ready(){
		var mainScene = (Main)GetTree().Root.GetNode("Main");
		player = mainScene.GetNode<Player>("Player");
		itemManager = mainScene.GetItemManager();
		
		attackRate = 1 / aps;
		attackCooldown = attackRate;
	}
	
	public void DropItem()
	{
		if (itemManager == null){
			//GD.PrintErr("ItemManager Not Found!");
			return;
		}
		
		PackedScene itemToDrop = itemManager.GetRandomItem(0.2f);
		if (itemToDrop != null){
			Node2D itemInstance = itemToDrop.Instantiate<Node2D>();
			itemInstance.GlobalPosition = GlobalPosition;
			GetTree().Root.AddChild(itemInstance);
		}
	}

	public override void _Process(double delta){
		if (inRange && attackCooldown <= 0){
			Attack();
			attackCooldown = attackRate;
		}
		else{
			attackCooldown -= (float)delta;
		}
	}
	
	public override void _PhysicsProcess(double delta){
		if (player != null){
			LookAt(player.GlobalPosition);
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * speed;
		}
		else{
			Velocity = Vector2.Zero;
		}
		MoveAndSlide();
	}
	
	public void Attack(){
		//GD.Print("Attack player");
		player.GetNode<Health>("Health").Damage(damage);
	}
	
	public void OnAttackRangeBodyEnter(Node2D body){
		if (body.IsInGroup("player")){
			inRange = true;
		}
	}
	
	public void OnAttackRangeBodyExit(Node2D body){
		if (body.IsInGroup("player")){
			inRange = false;
			attackCooldown = attackRate;
		}
	}
	
	public void Die(){
		DropItem();
		QueueFree();
	}
}
