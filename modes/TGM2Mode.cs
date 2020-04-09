using System.Collections.Generic;
//Info from https://tetris.fandom.com/wiki/Tetris_The_Absolute_The_Grand_Master_2
namespace Godtris{
  public class TGM2Mode : Mode
  {
    public class Level{
      private int _level;
      private int _gravity;
      public int level{
        get{
          return _level;
        }
      }
      public int gravity{
        get{
          return _gravity;
        }
      }
      public Level(int level, int gravity){
        _level = level;
        _gravity = gravity;
      }
    }

    private List<Level> _levels = new List<Level>();
    private bool _init = false;
    public const int ARE_FRAMES = 25;
    public const int LOCK_FRAMES = 30;
    public const int DAS_FRAMES = 16;
    public const int LINE_CLEAR_FRAMES = 40;

    public TGM2Mode(List<Block> blocks, int level) : base(blocks){
      _rowsPerFrame = GetGravity(level);
      _are = ARE_FRAMES;
      _lockDelay = LOCK_FRAMES;
    }

    public override float GetGravity(int level){
      if(!_init){
        _levels.Add(new Level(0, 4));
        _levels.Add(new Level(8, 5));
        _levels.Add(new Level(19, 6));
        _levels.Add(new Level(35, 8));
        _levels.Add(new Level(40, 10));
        _levels.Add(new Level(50, 12));
        _levels.Add(new Level(60, 16));
        _levels.Add(new Level(70, 32));
        _levels.Add(new Level(80, 48));
        _levels.Add(new Level(90, 64));
        _levels.Add(new Level(100, 4));
        _levels.Add(new Level(108, 5));
        _levels.Add(new Level(119, 6));
        _levels.Add(new Level(125, 8));
        _levels.Add(new Level(131, 12));
        _levels.Add(new Level(139, 32));
        _levels.Add(new Level(149, 48));
        _levels.Add(new Level(156, 80));
        _levels.Add(new Level(164, 112));
        _levels.Add(new Level(174, 128));
        _levels.Add(new Level(180, 144));
        _levels.Add(new Level(200, 16));
        _levels.Add(new Level(212, 48));
        _levels.Add(new Level(221, 80));
        _levels.Add(new Level(232, 112));
        _levels.Add(new Level(244, 144));
        _levels.Add(new Level(256, 176));
        _levels.Add(new Level(267, 192));
        _levels.Add(new Level(277, 208));
        _levels.Add(new Level(287, 224));
        _levels.Add(new Level(295, 240));
        _levels.Add(new Level(300, 5120));
        _init = true;
      }

      return _levels.Find(lvl => lvl.level == level).gravity / 256.0f;
    }
  }
}