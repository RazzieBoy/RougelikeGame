using Godot;
using System;

public partial class MainMenu : Control
{
	public override void _Ready(){
		
	}

	private void _on_start_button_pressed(){
		// Replace with function body.
		GD.Print("Play button");
		GetTree().ChangeSceneToFile("res://main.tscn");
	}
	
	private void _on_quit_button_pressed(){
		GetTree().Quit();
	}
}
