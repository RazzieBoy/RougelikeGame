using Godot;
using System;

public partial class DeathDisplay : Control{
	
	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://UI/main_menu.tscn");
	}
	
	private void _on_retry_pressed(){
		GetTree().ChangeSceneToFile("res://UI/main.tscn");
	}
	
}
