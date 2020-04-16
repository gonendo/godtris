using Godot;
using System.Collections.Generic;

namespace Godtris{
  public class Mode
  {
    protected List<Block> _blocks;
    protected float _count = 0; //counter to know the number of rows to go down
    protected float _count2 = 0; //counter for entry delay
    protected float _count3 = 0; //counter for lock delay
    protected float _count4 = 0; //counter for das
    protected float _count5 = 0; //counter for line clear

    protected Level _level;
    protected List<Piece> _history;

    protected bool _waitForARE = false;
    protected bool _waitForLockDelay = false;
    protected bool _waitForDAS = false;
    protected bool _waitForLineClear = false;
    
    protected bool _autoShift = false;

    protected List<Spatial> previewBlocks;
    protected string previewPiece;

    protected bool _firstPiece = true;
    protected List<int> _clearedLines;

    protected Game _game;

    public Mode(Game game, List<Block> blocks){
      this._game = game;
      this._blocks = blocks;
      this._history = new List<Piece>();
      _clearedLines = new List<int>();
    }

    public void Update(){
      _count+=_level.gravity;
      _count2++;
      _count3++;
      _count4++;
      _count5++;

      if(Input.IsActionJustReleased(Controls.LEFT_ACTION_ID) || 
        Input.IsActionJustReleased(Controls.RIGHT_ACTION_ID) || 
        Input.IsActionJustReleased(Controls.SOFT_DROP_ACTION_ID)){
        _waitForDAS = false;
        _autoShift = false;
      }

      Piece piece = GetCurrentPiece();
      if(piece!=null && piece.name == Piece.EMPTY && !_waitForARE){
        RenderNextPiece();
      }

      if(_waitForDAS && (_count4 >= _level.das)){
        _waitForDAS = false;
        _autoShift= true;
      }
      
      if(_waitForLockDelay && (_count3 >= _level.lockDelay)){
        _waitForLockDelay = false;
        GetCurrentPiece().locked = true;
        if(CheckLines()){
          StartLineClear();
        }
      }

      if(_waitForARE && _count2 >= _level.are){
        _waitForARE = false;
        RenderNextPiece();
      }

      if(_waitForLineClear && _count5 >= _level.lineClear){
        _waitForLineClear = false;
        StartARE();
      }

      if(_count >= 1){
        for(int i=0; i < _count; i++){
          piece = GetCurrentPiece();
          if(piece!=null && !_waitForLineClear && !_waitForARE){
            if(!piece.MoveDown()){
              if(!piece.locked){
                if(!_waitForLockDelay){
                  StartLockDelay();
                }
              }
              else if(!_waitForLockDelay){
                StartARE();
              }
            }
            else if(piece.locked){
              for(int j=0; j < 20; j++){
                piece.MoveDown();
              }
              if(CheckLines()){
                StartLineClear();
              }
              break;
            }
          }
        }
        _count=0;
      }
    }

    private void StartDAS(){
      if(_level.das > 0){
        _count4 = 0;
        _waitForDAS = true;
      }
    }

    protected void StartARE(){
      if(_level.are > 0){
        _count2 = 0;
        _waitForARE = true;
      }
    }

    protected void StartLineClear(){
      if(_level.lineClear > 0){
        _count5 = 0;
        _waitForLineClear = true;
      }
    }

    protected void StartLockDelay(){
      if(_level.lockDelay > 0){
        _count3 = 0;
        _waitForLockDelay = true;
      }
    }

    protected void RenderNextPiece(){
      if(_history.Count > 0){
        if(!_firstPiece){
          _history.RemoveAt(0);
        }
        Piece next = GetCurrentPiece();
        if(next!=null){
          List<Block> topRowBlocks = new List<Block>();
          foreach(Block block in _blocks){
            if(block.y == Game.GRID_HEIGHT-1 && !block.empty){
              topRowBlocks.Add(new Block(block.x, block.y, false));
            }
          }
          if(!next.rendered){
            next.Render();
          }
          else{
            List<Block> newBlocks = new List<Block>();
            foreach(Block block in next.GetBlocks()){
              Block blockBelow = _blocks.Find(b => b.x == block.x && b.y == block.y - 3);
              blockBelow.empty = false;
              blockBelow.color = next.color;
              blockBelow.visible = true;
              blockBelow.locked = false;
              newBlocks.Add(blockBelow);
              block.empty = true;
            }
            next.SetBlocks(newBlocks);
            next.visible = true;
          }

          //checking game over
          foreach(Block block in next.GetBlocks()){
            foreach(Block topRowBlock in topRowBlocks){
              if((block.x == topRowBlock.x) && (block.y == topRowBlock.y)){
                next.locked = true;
                _game.GameOver();
                return;
              }
            }
          }
        }
        _firstPiece = false;
      }
    }

    protected Piece GetCurrentPiece(){
      return _history.Count > 0 ? _history[0] : null;
    }

    protected Piece GetNextPiece(){
      return _history.Count > 1 ? _history[1] : null;
    }

    public void MovePiece(string actionId){
      if(_waitForDAS || _waitForLineClear){
        return;
      }

      Piece p = !_waitForARE ? GetCurrentPiece() : GetNextPiece();

      if(p!=null && !p.locked){
        switch(actionId){
          case Controls.LEFT_ACTION_ID:
            p.MoveLeft();
            break;
          case Controls.RIGHT_ACTION_ID:
            p.MoveRight();
            break;
          case Controls.SOFT_DROP_ACTION_ID:
            if(!p.MoveDown()){
              p.locked = true;
            }
            break;
          case Controls.HARD_DROP_ACTION_ID:
            _count = 20;
            break;
        }

        if(actionId!=Controls.HARD_DROP_ACTION_ID && !_autoShift){
          StartDAS();
        }
      }
    }

    public virtual void RotatePiece(string actionId){

    }

    public virtual Level GetLevel(int level){
      return null;
    }

    public virtual void SetLevel(int level){

    }

    protected void AddPreviewBlock(Game game, int x, int y, string color){
      if(previewBlocks==null){
        previewBlocks = new List<Spatial>();
      }
      Spatial block = Godtris.Utils.GetBlock(Color.ColorN(color));
      block.Translate(new Vector3(
      game.previewPosition.Transform.origin.x + x * Godtris.Utils.BLOCK_SIZE, 
      game.previewPosition.Transform.origin.y + y * Godtris.Utils.BLOCK_SIZE, 
      0));
      game.AddChild(block);
      previewBlocks.Add(block);
    }

    public virtual void RenderPreview(Game game){
      Piece piece = GetNextPiece();
      if(piece!=null && previewPiece!=piece.name){
        if(previewBlocks!=null){
          foreach(Spatial block in previewBlocks){
            block.QueueFree();
          }
          previewBlocks.Clear();
        }
        switch(piece.name){
          case Piece.I:
            for(int i=3; i < 7; i++){
              AddPreviewBlock(game, i, 0, "red");
            }
            break;
          case Piece.Z:
            for(int i=3; i < 5; i++){
              AddPreviewBlock(game, i, 0, "green");
            }
            for(int i=4; i < 6; i++){
              AddPreviewBlock(game, i, -1, "green");
            }
            break;
          case Piece.S:
            for(int i=4; i < 6; i++){
              AddPreviewBlock(game, i, 0, "magenta");
            }
            for(int i=3; i < 5; i++){
              AddPreviewBlock(game, i, -1, "magenta");
            }
            break;
          case Piece.J:
            for(int i=3; i < 6; i++){
              AddPreviewBlock(game, i, 0, "blue");
            }
            AddPreviewBlock(game, 5, -1, "blue");
            break;
          case Piece.L:
            for(int i=3; i < 6; i++){
              AddPreviewBlock(game, i, 0, "orange");
            }
            AddPreviewBlock(game, 3, -1, "orange");
            break;
          case Piece.O:
            for(int i=4; i < 6; i++){
              AddPreviewBlock(game, i, 0, "yellow");
              AddPreviewBlock(game, i, -1, "yellow");
            }
            break;
          case Piece.T:
            for(int i=3; i < 6; i++){
              AddPreviewBlock(game, i, 0, "cyan");
            }
            AddPreviewBlock(game, 4, -1, "cyan");
            break;
        }

        previewPiece = piece.name;
      }
    }

    protected bool CheckLines(){
      _clearedLines.Clear();
      int minClearedLineIndex = -1;
      for(int i=0; i < Game.GRID_HEIGHT; i++){
        int filled = 0;
        foreach(Block block in _blocks){
          if(block.y==i && !block.empty){
            filled++;
          }
        }
        if(filled == Game.GRID_WIDTH){
          _clearedLines.Add(i);
          if(minClearedLineIndex == -1){
            minClearedLineIndex = i;
          }
          else{
            minClearedLineIndex = Mathf.Min(i, minClearedLineIndex);
          }
        }
      }
      foreach(int lineIndex in _clearedLines){
        foreach(Block block in _blocks){
          if(block.y == lineIndex){
            block.Clear();
            block.empty = true;
          }
        }
      }

      if(_clearedLines.Count > 0){
        for(int i=0; i < _clearedLines.Count; i++){
          for(int j=1; j < Game.GRID_HEIGHT+4; j++){
            if(j > minClearedLineIndex){
              foreach(Block block in _blocks){
                if(!block.empty && block.y == j){
                  Block blockBelow = _blocks.Find(b => b.y == j-1 && b.x == block.x);
                  blockBelow.color = block.color;
                  blockBelow.empty = false;
                  blockBelow.locked = true;
                  block.empty = true;
                  block.locked = false;
                }
              }
            }
          }
        }

        return true;
      }
      
      return false;
    }
  }
}