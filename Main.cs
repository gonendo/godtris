using Godot;

namespace Godtris{
	public class Main : Spatial
	{
		public override void _Ready()
		{
			GD.Print("Godtris");
			PackedScene scene = ResourceLoader.Load<PackedScene>("res://Game.tscn");
			Game game = scene.Instance() as Game;
			Position3D pos = GetNode("ScenePosition") as Position3D;
			game.Translate(pos.Transform.origin);
			AddChild(game);
		}

		public override void _Process(float delta)
		{

		}
	}
}
