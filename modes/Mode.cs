using Godot;
using System.Collections.Generic;

namespace Godtris{
  public class Mode
  {
    protected List<Block> _blocks;
    protected float _count = 0; //counter to know the number of rows to go down
    protected float _count2 = 0; //counter for entry delay
    protected float _count3 = 0; //counter for lock delay

    protected Level _level;
    protected LinkedList<Piece> _history;

    protected bool _waitForARE = false;
    protected bool _waitForLockDelay = false;

    public Mode(List<Block> blocks){
      this._blocks = blocks;
      this._history = new LinkedList<Piece>();

      //DEBUG CODE
      for(int i=0; i < 20; i++)
      _history.AddLast(new Piece(Piece.I, this._blocks));
    }

    /*
    TODO : ARE first piece, ARS + IRS, Piece lock (effect)
    BUG : first line not visible
    */
    public void update(){
      _count+=_level.gravity;
      _count2++;
      _count3++;

      if(_waitForLockDelay && (_count3 >= _level.lockDelay)){
        #if (DEBUG)
          GD.Print("Wait For ARE");
        #endif
        _waitForARE = true;
        _count2 = 0;
        _waitForLockDelay = false;
      }
      if(_waitForARE && _count2 >= _level.are){
        if(_history.Count > 0){
          _history.RemoveFirst();
        }
        _waitForARE = false;
        _waitForLockDelay = false;
      }
      if(_count >= 1){
        for(int i=0; i < _count; i++){
          Piece piece = GetCurrentPiece();
          if(piece!=null){
            if(!piece.MoveDown()){
              if(!_waitForLockDelay){
                #if (DEBUG)
                  GD.Print("Wait For Lock Delay");
                #endif
                _count3 = 0;
              }
              _waitForLockDelay = true;
            }
            else{
              _waitForLockDelay = false;
            }
          }
        }
        _count=0;
      }
    }

    public Piece GetCurrentPiece(){
      return _history.First!=null ? _history.First.Value as Piece : null;
    }

    public virtual Level GetLevel(int level){
      return null;
    }

    public virtual void SetLevel(int level){

    }
  }
}