using Godot;
using System;
using System.Collections.Generic;

namespace Godtris{
	public class Game : Spatial
	{
		public const int GRID_WIDTH = 10;
		public const int GRID_HEIGHT = 20;
		public const int MASTER_MODE = 1;
		public const int DEATH_MODE = 2;
		public Position3D tetrionBottomLeft;
		public Vector3 tetrionBottomLeftPosition;
		public Position3D previewPosition;
		private List<Block> _blocks;
		public Mode mode;
		public Controls controls;
		private bool _started = false;
		private bool _gameover = false;
		private bool _starting = false;
		private int _mode;
		private uint _time;

		public override void _Ready()
		{
			GD.Print("Game ready");
			tetrionBottomLeft = GetNode("Tetrion/BottomLeft") as Position3D;
			tetrionBottomLeftPosition = tetrionBottomLeft.ToGlobal(tetrionBottomLeft.Transform.origin);

			previewPosition = GetNode("PreviewPosition") as Position3D;
		}

		public override void _PhysicsProcess(float delta)
		{
			if(!_gameover && !_starting && Input.IsActionJustPressed(Controls.RESTART_ACTION_ID)){
				StartMode(_mode);
			}
			if(_started && !_gameover){
				TimeSpan t = TimeSpan.FromMilliseconds(OS.GetTicksMsec() - _time);
				SetTime(string.Format(@"{0:mm\:ss\:ff}", t));
				controls.Update();
				mode.RenderPreview(this);
				mode.Update();
				foreach(Block block in _blocks){
					block.Render();
				}
			}
		}

		public async void StartMode(int gameMode){
			_mode = gameMode;
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

			switch(_mode){
				case MASTER_MODE:
					mode = new TGM2Mode(this, _blocks, 0);
					break;
				case DEATH_MODE:
					mode = new DeathMode(this, _blocks, 0);
					break;
			}
			
			SetTetrionColor(mode.GetTetrionColor());
			SetLevel(0);
			SetLines(0);
			SetTime("00:00:00");
			controls = new Controls(mode);
			RichTextLabel readyLabel = GetNode("Viewport2/Ready") as RichTextLabel;
			MeshInstance mesh = GetNode("Ready") as MeshInstance;
			mesh.Show();
			readyLabel.BbcodeText = "[center]READY[/center]";
			Timer t = new Timer();
			t.WaitTime = 0.5f;
			t.Autostart = true;
			AddChild(t);
			await ToSignal(t, "timeout");
			readyLabel.BbcodeText = "[center]GO ![/center]";
			t.Stop();
			t.Start();

			await ToSignal(t, "timeout");
			mesh.Hide();
			t.QueueFree();

			_time = OS.GetTicksMsec();
			_started = true;
			_starting = false;
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
			label.BbcodeText = string.Format("[color=#ffff00]LEVEL[/color]\n [u]{0}[/u]\n {1}", string.Format("{0:D3}", lvl), string.Format("{0:D3}", mode.maxLevel));
		}

		public void SetLines(int lines){
			RichTextLabel label = GetNode("Viewport/Lines") as RichTextLabel;
			label.BbcodeText = string.Format("[color=#00ffff]LINES[/color]\n {0}", string.Format("{0:D3}", lines));			
		}

		public void SetTime(string time){
			RichTextLabel timeLabel = GetNode("Viewport3/Time") as RichTextLabel;
			timeLabel.BbcodeText = string.Format("[center]{0}[/center]", time);
		}

		private void SetTetrionColor(string color){
			Spatial tetrion = GetNode("Tetrion") as Spatial;
			for(int i=0; i < tetrion.GetChildCount(); i++){
				MeshInstance mesh = tetrion.GetChild(i) as MeshInstance;
				if(mesh != null){
					SpatialMaterial material = mesh.GetSurfaceMaterial(0) as SpatialMaterial;
					if(material != null){
						material.AlbedoColor = new Color(color);
					}
				}
			}
		}

		public void PlaySound(string id){
			AudioStreamPlayer asp = GetNode(id) as AudioStreamPlayer;
			AudioStreamOGGVorbis sound = asp.Stream as AudioStreamOGGVorbis;
			sound.Loop = false;
			asp.Play();
		}

	}
}
