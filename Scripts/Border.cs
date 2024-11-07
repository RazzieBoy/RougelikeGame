using Godot;
using System;

public partial class Border : Control
{
	 // Define the margin size for the border
	private const float Margin = 200f;

	public override void _Draw() {
		// Get the current size of the viewport
		Vector2 viewportSize = GetViewportRect().Size;

		// Define the rectangle for the border
		Rect2 borderRect = new Rect2(Margin, Margin, viewportSize.X - 2 * Margin, viewportSize.Y - 2 * Margin);

		// Draw the border with a transparent fill and a line thickness of 2 pixels
		DrawRect(borderRect, Colors.Transparent, false, 2.0f);
	}

	public override void _Process(double delta) {
		// Call the Update() method to redraw the border if the window size changes
	   QueueRedraw();
	}
}
