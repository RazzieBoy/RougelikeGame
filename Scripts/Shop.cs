using Godot;
using System;

public partial class Shop : Area2D
{
	private string choiceType; // Variable to identify which choice was clicked (Health, Speed, Damage)
	private bool isPlayerNearby = false; // Track if the player is near

	public override void _Ready()
	{
		// Connect the area entered and exited signals
		this.BodyEntered += OnBodyEntered;
		this.BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			isPlayerNearby = true; // Player is near this choice
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			isPlayerNearby = false; // Player is no longer near this choice
		}
	}

	public override void _Process(double delta)
	{
		if (isPlayerNearby && Input.IsActionJustPressed("ui_accept")) // "T" key
		{
			Interact(); // Call interaction logic
		}
	}

	private void Interact()
	{
		GD.Print($"Choice interacted with: {choiceType}");

		// Perform the appropriate action based on the choice
		switch (choiceType)
		{
			case "Health":
				GD.Print("Interacted with Health choice!");
				((Player)GetParent().GetNode("Player")).Heal(10); // Heal the player
				break;
			case "Speed":
				GD.Print("Interacted with Speed choice!");
				((Player)GetParent().GetNode("Player")).ModifySpeed(50); // Modify player speed
				break;
			case "Damage":
				GD.Print("Interacted with Damage choice!");
				((Player)GetParent().GetNode("Player")).ModifyDamage(10); // Modify player damage
				break;
		}

		DestroyAllChoices(); // Destroy all choices after one is clicked
	}

	private void DestroyAllChoices()
	{
		var parent = GetParent(); // Assuming the parent is Node2D containing Health, Speed, Damage
		foreach (Node child in parent.GetChildren())
		{
			if (child is Area2D area && area != this) // Ensure not to destroy the Shop itself
			{
				area.QueueFree(); // Destroy each choice (Health, Speed, Damage)
			}
		}
	}

	public void SetChoiceType(string type)
	{
		choiceType = type;
	}
}
