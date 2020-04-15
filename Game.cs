using Godot;
using System.Collections.Generic;

namespace Godtris{
	public class Game : Spatial
	{
		public const int GRID_WIDTH = 10;
		public const int GRID_HEIGHT = 20;
		public Position3D tetrionBottomLeft;
		public Vector3 tetrionBottomLeftPosition;
		public Position3D previewPosition;
		private List<Block> _blocks;
		public Mode mode;
		public Controls controls;
		private bool _started = false;

		public async override void _Ready()
		{
			GD.Print("Game ready");
			tetrionBottomLeft = GetNode("Tetrion/BottomLeft") as Position3D;
			tetrionBottomLeftPosition = tetrionBottomLeft.ToGlobal(tetrionBottomLeft.Transform.origin);

			previewPosition = GetNode("PreviewPosition") as Position3D;

			_blocks = new List<Block>();
			for(int i=0; i < GRID_WIDTH; i++){
				for(int j=0; j < GRID_HEIGHT+1; j++){
					_blocks.Add(new Block(i, j, this));
				}
			}

			Timer t = new Timer();
			t.OneShot = true;
			t.WaitTime = 1;
			t.Autostart = true;
			AddChild(t);
			await ToSignal(t, "timeout");
			t.QueueFree();
			StartTGM2();
		}
		public override void _PhysicsProcess(float delta)
		{
			if(_started){
				controls.Update();
				mode.RenderPreview(this);
				mode.Update();
				foreach(Block block in _blocks){
					block.Render();
				}
			}
		}

		private void StartTGM2(){
			mode = new TGM2Mode(_blocks, 300);
			controls = new Controls(mode);
			_started = true;
		}
	}
}
