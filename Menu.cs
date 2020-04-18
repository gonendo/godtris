using Godot;
using System;

namespace Godtris{
	public class Menu : Node2D
	{
		private Action<int> _StartGame;
		public Action<int> StartGame{
			set{
				_StartGame = value;
			}
		}

		public override void _Ready()
		{
			GetNode("VBoxContainer/Master").Connect("pressed", this, nameof(StartMaster));
			GetNode("VBoxContainer/TA Death").Connect("pressed", this, nameof(StartDeath));
		}

		public void StartMaster(){
			_StartGame(Game.MASTER_MODE);
		}

		public void StartDeath(){
			_StartGame(Game.DEATH_MODE);
		}
	}
}
