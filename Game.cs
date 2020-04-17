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
		private bool _gameover = false;
		private bool _starting = false;

		public override void _Ready()
		{
			GD.Print("Game ready");
			tetrionBottomLeft = GetNode("Tetrion/BottomLeft") as Position3D;
			tetrionBottomLeftPosition = tetrionBottomLeft.ToGlobal(tetrionBottomLeft.Transform.origin);

			previewPosition = GetNode("PreviewPosition") as Position3D;

			StartMode();
		}

		public override void _PhysicsProcess(float delta)
		{
			if(!_gameover && !_starting && Input.IsActionJustPressed(Controls.RESTART_ACTION_ID)){
				StartMode();
			}
			if(_started && !_gameover){
				controls.Update();
				mode.RenderPreview(this);
				mode.Update();
				foreach(Block block in _blocks){
					block.Render();
				}
			}
		}

		private async void StartMode(){
			_starting = true;
			_started = false;
			_gameover = false;
			if(_blocks!=null){
				foreach(Block block in _blocks){
					block.Destroy();
				}
			}
			_blocks = new List<Block>();
			for(int i=0; i < GRID_WIDTH; i++){
				for(int j=0; j < GRID_HEIGHT+4; j++){
					_blocks.Add(new Block(i, j, this));
				}
			}
			if(mode!=null){
				mode.DestroyPreview(this);
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

		public async void GameOver(){
			GD.Print("Game Over");
			_gameover = true;
			Timer t = new Timer();
			t.WaitTime = 0.2f;
			AddChild(t);
			for(int i=0; i <= GRID_HEIGHT+1; i++){
				t.Stop();
				t.Start();
				await ToSignal(t, "timeout");
				for(int j=0; j < GRID_WIDTH; j++){
					Block blockBelow = _blocks.Find(b => b.x == j && b.y == i-1);
					if(blockBelow!=null){
						if(blockBelow.y < GRID_HEIGHT){
							blockBelow.empty = true;
						}
						else{
							blockBelow.visible = false;
						}
					}
					Block block = _blocks.Find(b => b.x == j && b.y == i);
					if(!block.empty){
						block.Lighten();
					}
				}
			}
			t.QueueFree();
			_started = false;
			_gameover = false;
		}

		public void SetLevel(int level){
			RichTextLabel label = GetNode("Viewport/Level") as RichTextLabel;
			int lvl = Mathf.Min(level, mode.maxLevel);
			label.BbcodeText = string.Format("[color=#ffff00]LEVEL[/color]\n [u]{0}[/u]\n {1}", string.Format("{0:D3}", lvl), mode.maxLevel);
		}

		public void PlaySound(string id){
			AudioStreamPlayer asp = GetNode(id) as AudioStreamPlayer;
			AudioStreamOGGVorbis sound = asp.Stream as AudioStreamOGGVorbis;
			sound.Loop = false;
			asp.Play();
		}

		private void StartTGM2(){
			mode = new TGM2Mode(this, _blocks, 0);
			controls = new Controls(mode);
			_started = true;
			_starting = false;
		}
	}
}
