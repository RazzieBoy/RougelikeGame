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
	
	private void _on_settings_button_pressed(){}
}
