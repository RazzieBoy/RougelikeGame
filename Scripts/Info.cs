using Godot;
using System;

public partial class Info : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
	}

	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://UI/main_menu.tscn");
	}
}
