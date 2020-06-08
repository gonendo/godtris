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
		private Mode _mode;
		private Controls _controls;
		private bool _started = false;
		private bool _gameover = false;
		private bool _starting = false;
		private int _modeId;
		private uint _time;
		private bool _firstStart=true;

		public override void _Ready()
		{
			GD.Print("Game ready");
			tetrionBottomLeft = GetNode("Tetrion/BottomLeft") as Position3D;
			tetrionBottomLeftPosition = tetrionBottomLeft.ToGlobal(tetrionBottomLeft.Transform.origin);

			previewPosition = GetNode("PreviewPosition") as Position3D;

			ShowHUD(false);
			StartMode();
		}

		public override void _PhysicsProcess(float delta)
		{
			if(!_gameover && !_starting && Input.IsActionJustPressed(Controls.RESTART_ACTION_ID)){
				StartMode();
			}
			if(_started && !_gameover){
				TimeSpan t = TimeSpan.FromMilliseconds(OS.GetTicksMsec() - _time);
				SetTime(string.Format(@"{0:mm\:ss\:ff}", t));
				_controls.Update();
				_mode.RenderPreview(this);
				_mode.Update();
				foreach(Block block in _blocks){
					block.Render();
				}
			}
		}

		public void SetMode(int gameMode){
			_modeId = gameMode;
		}

		public async void StartMode(){
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
			if(_mode!=null){
				_mode.DestroyPreview(this);
			}

			switch(_modeId){
				case MASTER_MODE:
					_mode = new TGM2Mode(this, _blocks, 0);
					break;
				case DEATH_MODE:
					_mode = new DeathMode(this, _blocks, 0);
					break;
			}
			
			SetTetrionColor(_mode.GetTetrionColor());
			SetLevel(0);
			SetLines(0);
			SetTime("00:00:00");
			_controls = new Controls(_mode);

			Timer t = new Timer();
			t.WaitTime = 1.1f;
			t.Autostart = true;
			AddChild(t);

			if(_firstStart){
				await ToSignal(t, "timeout");
				ShowHUD(true);
				_firstStart = false;
			}

			t.WaitTime = 0.8f;
			RichTextLabel readyLabel = GetNode("ReadyText") as RichTextLabel;
			readyLabel.Show();
			readyLabel.BbcodeText = "[center]READY[/center]";
			PlaySound(Sounds.READY);
			t.Stop();
			t.Start();
			await ToSignal(t, "timeout");
			readyLabel.BbcodeText = "[center]GO ![/center]";
			PlaySound(Sounds.GO);
			t.Stop();
			t.Start();

			await ToSignal(t, "timeout");
			readyLabel.Hide();
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

		private void ShowHUD(bool visible){
			for(int i=0; i < GetChildCount(); i++){
				if((GetChild(i) as RichTextLabel)!=null){
					if(visible){
						(GetChild(i) as RichTextLabel).Show();
					}
					else{
						(GetChild(i) as RichTextLabel).Hide();
					}
				}
			}
			if(visible){
				(GetNode("Line2D") as Line2D).Show();
			}
			else{
				(GetNode("Line2D") as Line2D).Hide();
			}
		}

		public void SetLevel(int level){
			RichTextLabel label = GetNode("LevelValue") as RichTextLabel;
			int lvl = Mathf.Min(level, _mode.maxLevel);
			label.BbcodeText = string.Format("{0}\n{1}", string.Format("{0:D3}", lvl), string.Format("{0:D3}", _mode.maxLevel));
		}

		public void SetLines(int lines){
			RichTextLabel label = GetNode("LinesValue") as RichTextLabel;
			label.BbcodeText = string.Format("{0}", string.Format("{0:D3}", lines));			
		}

		public void SetTime(string time){
			RichTextLabel timeLabel = GetNode("Time") as RichTextLabel;
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
			if(sound!=null){
				sound.Loop = false;
			}
			asp.Play();
		}

	}
}
