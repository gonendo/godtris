//Info from https://tetris.fandom.com/wiki/Tetris_The_Absolute_The_Grand_Master_2
using System.Collections.Generic;
using Godot;

namespace Godtris{
  public class TGM2Mode : Mode
  {
    private Dictionary<int[], int[]> _timings;
    private Dictionary<int, int> _gravities;
    private const int ARE_TIMINGS_INDEX = 0;
    private const int DAS_TIMINGS_INDEX = 1;
    private const int LOCK_TIMINGS_INDEX = 2;
    private const int LINECLEAR_TIMINGS_INDEX = 3;

    public TGM2Mode(Game game, List<Block> blocks, int level) : base(game, blocks){
      _timings = new Dictionary<int[], int[]>();

      //MASTER MODE
      _timings.Add(new int[2] {0, 499}, new int[4] {25,16,30,40});
      _timings.Add(new int[2] {500, 599}, new int[4] {25,10,30,25});
      _timings.Add(new int[2] {600, 699}, new int[4] {25,10,30,16});
      _timings.Add(new int[2] {700, 799}, new int[4] {16,10,30,12});
      _timings.Add(new int[2] {800, 899}, new int[4] {12,16,30,40});
      _timings.Add(new int[2] {900, 999}, new int[4] {12,16,30,40});

      _gravities = new Dictionary<int, int>();
      _gravities.Add(0, 4);
      _gravities.Add(30, 6);
      _gravities.Add(35, 8);
      _gravities.Add(40, 10);
      _gravities.Add(50, 12);
      _gravities.Add(60, 16);
      _gravities.Add(70, 32);
      _gravities.Add(80, 48);
      _gravities.Add(90, 64);
      _gravities.Add(100, 80);
      _gravities.Add(120, 96);
      _gravities.Add(140, 112);
      _gravities.Add(160, 128);
      _gravities.Add(170, 144);
      _gravities.Add(200, 4);
      _gravities.Add(220, 32);
      _gravities.Add(230, 64);
      _gravities.Add(233, 96);
      _gravities.Add(236, 128);
      _gravities.Add(239, 160);
      _gravities.Add(243, 192);
      _gravities.Add(247, 224);
      _gravities.Add(251, 256); // 1G
      _gravities.Add(300, 512); // 2G
      _gravities.Add(330, 768); // 3G
      _gravities.Add(360, 1024); // 4G
      _gravities.Add(400, 1280); // 5G
      _gravities.Add(420, 1024); // 4G
      _gravities.Add(450, 768); // 3G
      _gravities.Add(500, 5120); // 20G

      SetLevel(level);

      //DEBUG CODE
      _history.Add(new Piece(Piece.EMPTY, this._blocks));

      for(int i=0; i < 50; i++){
        _history.Add(new Piece(Piece.I, this._blocks));
        _history.Add(new Piece(Piece.Z, this._blocks));
        _history.Add(new Piece(Piece.S, this._blocks));
        _history.Add(new Piece(Piece.J, this._blocks));
        _history.Add(new Piece(Piece.L, this._blocks));
        _history.Add(new Piece(Piece.O, this._blocks));
        _history.Add(new Piece(Piece.T, this._blocks));
      }

      //START
      StartARE();
    }

    public override Level GetLevel(int level){
      if(_level!=null && _level.level == level){
        return _level;
      }

      float gravity = 0.0f;
      foreach(KeyValuePair<int,int> values in _gravities){
        if(level >= values.Key){
          gravity = values.Value / 256.0f; // Rows Per Frame
        }
      }

      int[] timings = GetTimings(level);

      return new Level(
        level, 
        gravity, 
        timings[ARE_TIMINGS_INDEX], 
        timings[DAS_TIMINGS_INDEX], 
        timings[LOCK_TIMINGS_INDEX], 
        timings[LINECLEAR_TIMINGS_INDEX]
      );
    }

    public override void SetLevel(int level){
      _level = GetLevel(level);
      #if (DEBUG)
        GD.Print("SetLevel "+_level.level+" "+_level.gravity+" "+_level.are+" "+_level.das+" "+_level.lockDelay+" "+_level.lineClear);
      #endif
    }

    public override void RotatePiece(string actionId){
      Piece piece = GetCurrentPiece();
      List<Block> rotatedBlocks = null;
      if(piece!=null && !_waitForLineClear){
        if(piece.locked || piece.name == Piece.EMPTY){
          piece = GetNextPiece();
          if(piece==null){
            return;
          }
          if(!piece.rendered){
            piece.Render(false);
          }
        }

        rotatedBlocks = actionId == Controls.ROTATE_LEFT_ACTION_ID ? TGMRotation.RotateLeft(_blocks, piece) : TGMRotation.RotateRight(_blocks, piece);
        
        if(rotatedBlocks!=null){
          foreach(Block block in piece.GetBlocks()){
            block.empty = true;
            block.visible = true;
          }
          foreach(Block block in rotatedBlocks){
            if(piece == GetCurrentPiece()){
              block.color = piece.color;
              block.empty = false;
            }
          }
          
          piece.SetBlocks(rotatedBlocks);
        }
      }
    }

    private int[] GetTimings(int level){
      int[] timings = {};
      foreach(KeyValuePair<int[],int[]> values in _timings){
        if( (level >= values.Key[values.Key.GetLowerBound(0)]) && (level <= values.Key[values.Key.GetUpperBound(0)]) ){
          timings = values.Value;
          break;
        }
      }
      return timings;
    }
  }
}