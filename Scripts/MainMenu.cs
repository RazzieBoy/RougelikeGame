using Godot;
using System;

public partial class MainMenu : Control{
	//Let's the player start the game
	private void _on_start_button_pressed(){
		GD.Print("Play button");
		GetTree().ChangeSceneToFile("res://UI/main.tscn");
	}
	//Button to close the game
	private void _on_quit_button_pressed(){
		GetTree().Quit();
	}
	//Button which takes the player to a scene that shows all the keybinds and what they do
	private void _on_help_button_pressed(){
		GetTree().ChangeSceneToFile("res://Entities/info.tscn");
	}
}
