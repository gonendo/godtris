using Godot;
using System;
namespace Godtris{
	public class Path : Godot.Path
	{
		private PathFollow _pathFollow;
		public override void _Ready()
		{
			Curve.AddPoint(new Vector3(0,0,0));
			Curve.AddPoint(new Vector3(0,0,-5f));
			_pathFollow = GetNode("PathFollow") as PathFollow;
		}

		public override void _Process(float delta)
		{
			if((Owner as Main).gameStarted){
				_pathFollow.Offset += 6 * delta;
			}
		}
	}
}
