using Godot;
using System.Collections.Generic;

namespace Godtris{
	public class Game : Spatial
	{
		public Position3D tetrionBottomLeft;
		public Vector3 tetrionBottomLeftPosition;
		private List<Block> _blocks;
		public Mode mode;

		public override void _Ready()
		{
			GD.Print("Game ready");
			tetrionBottomLeft = GetNode("Tetrion/BottomLeft") as Position3D;
			tetrionBottomLeftPosition = tetrionBottomLeft.ToGlobal(tetrionBottomLeft.Transform.origin);
			_blocks = new List<Block>();
			for(int i=0; i < 10; i++){
				for(int j=0; j < 20; j++){
					_blocks.Add(new Block(i, j, this));
				}
			}

			//Default Mode is TGM2
			mode = new TGM2Mode(_blocks, 100);
		}
		public override void _PhysicsProcess(float delta)
		{
			mode.update();
			foreach(Block block in _blocks){
				block.Render();
			}
		}
	}
}
