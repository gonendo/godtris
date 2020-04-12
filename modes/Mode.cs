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

    protected Level _level;
    protected List<Piece> _history;

    protected bool _waitForARE = false;
    protected bool _waitForLockDelay = false;
    protected bool _waitForDAS = false;
    
    protected bool _autoShift = false;

    public Mode(List<Block> blocks){
      this._blocks = blocks;
      this._history = new List<Piece>();
    }

    public void Update(){
      _count+=_level.gravity;
      _count2++;
      _count3++;
      _count4++;

      if(Input.IsActionJustReleased(Controls.LEFT_ACTION_ID) || 
        Input.IsActionJustReleased(Controls.RIGHT_ACTION_ID) || 
        Input.IsActionJustReleased(Controls.SOFT_DROP_ACTION_ID)){
        _waitForDAS = false;
        _autoShift = false;
      }

      Piece piece = GetCurrentPiece();
      if(piece!=null && piece.name == Piece.EMPTY && !_waitForARE){
        StartARE();
      }

      if(_waitForDAS && (_count4 >= _level.das)){
        _waitForDAS = false;
        _autoShift= true;
      }

      if(_waitForLockDelay && (_count3 >= _level.lockDelay)){
        _waitForLockDelay = false;
        GetCurrentPiece().locked = true;
      }

      if(_waitForARE && _count2 >= _level.are){
        _waitForARE = false;
        _waitForLockDelay = false;
        _waitForDAS = false;
        RenderNextPiece();
      }

      if(_count >= 1){
        for(int i=0; i < _count; i++){
          piece = GetCurrentPiece();
          if(piece!=null){
            if(!piece.MoveDown()){
              if(!piece.locked){
                if(!_waitForLockDelay){
                  _count3 = 0;
                }
                _waitForLockDelay = true;
              }
              else if(!_waitForARE){
                StartARE();
              }
            }
            else{
              _waitForLockDelay = false;
            }
          }
        }
        _count=0;
      }
    }

    private void StartDAS(){
      if(_level.das > 0){
        _waitForDAS = true;
        _count4 = 0;
      }
    }

    private void StartARE(){
      if(_level.are > 0){
        _waitForARE = true;
        _count2 = 0;
      }
    }

    protected void RenderNextPiece(bool first=false){
      if(_history.Count > 0){
        if(!first){
          _history.RemoveAt(0);
        }
        Piece next = GetCurrentPiece();
        if(next!=null){
          next.Render();
        }
      }   
    }

    protected Piece GetCurrentPiece(){
      return _history.Count > 0 ? _history[0] : null;
    }

    private Piece GetNextPiece(){
      return _history.Count > 1 ? _history[1] : null;
    }

    public void MovePiece(string actionId){
      if(_waitForDAS){
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
              _waitForLockDelay = false;
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
  }
}