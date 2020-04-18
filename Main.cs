using Godot;

namespace Godtris{
	public class Main : Spatial
	{
		private Game _game;
		private Menu _menu;

		public override void _Ready()
		{
			GD.Print("Godtris");
			LoadMenu();
		}

		private void LoadMenu(){
			PackedScene scene = ResourceLoader.Load<PackedScene>("res://Menu.tscn");
			_menu = scene.Instance() as Menu;
			_menu.StartGame = StartGame;
			AddChild(_menu);
		}

		public void StartGame(int mode){
			_menu.QueueFree();
			PackedScene scene = ResourceLoader.Load<PackedScene>("res://Game.tscn");
			_game = scene.Instance() as Game;
			Position3D pos = GetNode("GamePosition") as Position3D;
			_game.Translate(pos.Transform.origin);
			AddChild(_game);
			_game.StartMode(mode);
		}

		public override void _Process(float delta)
		{

		}
	}
}
