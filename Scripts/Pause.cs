using Godot;
using System;

public partial class Pause : Control{
	
	public override void _Ready(){
		Hide();
	}
	
	public override void _Process(double delta){
		if (Input.IsActionJustPressed("home")){
			GetTree().Paused = true;
			Show();
		}
	}
	
	private void _Pause(){
		GetTree().Paused = false;
		Hide();
	}
	
	private void _Home(){
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://UI/main_menu.tscn");
	}
}
