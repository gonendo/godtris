using Godot;
using System.Collections.Generic;
namespace Godtris{
  public class Piece{
    public const string EMPTY = "EMPTY";
    public const string I = "I";
    public const string Z = "Z";
    public const string S = "S";
    public const string J = "J";
    public const string L = "L";
    public const string O = "O";
    public const string T = "T";
    private string _color;
    private bool _locked;
    private List<Block> _blocks;
    private List<Block> _current;
    private string _name;
    private bool _rendered=false;

    public string name{
      get{
        return _name;
      }
    }

    public string color{
      get{
        return _color;
      }
    }

    public int rotation_state = 0;

    public List<Block> GetBlocks(){
      return _current;
    }

    public void SetBlocks(List<Block> blocks){
      _current = blocks;
    }

    public bool locked{
      get{
        return _locked;
      }
      set{
        _locked = value;
        foreach(Block block in _current){
          block.locked = _locked;
        }
      }
    }

    public bool rendered{
      get{
        return _rendered;
      }
    }

    public Piece(string name, List<Block> blocks){
      _blocks = blocks;
      _current = new List<Block>();
      _name = name;
    }

    public void Render(bool visible=true){
      switch(_name){
        case Piece.I:
          _color = "red";
          /*
            xxx####xxx
            xxxxxxxxxx
            xxxxxxxxxx
          */
          for(int i=3; i < 7; i++){
            AddBlock(i, Game.GRID_HEIGHT-1, _color, visible);
          }
          break;
        case Piece.Z:
          _color = "green";
          /*
            xxx##xxxxx
            xxxx##xxxx
            xxxxxxxxxx
          */
          for(int i=3; i < 5; i++){
            AddBlock(i, Game.GRID_HEIGHT-1, _color, visible);
          }
          for(int i=4; i < 6; i++){
            AddBlock(i, Game.GRID_HEIGHT-2, _color, visible);
          }
          break;
        case Piece.S:
          _color = "magenta";
          /*
            xxxx##xxxx
            xxx##xxxxx
            xxxxxxxxxx
          */
          for(int i=4; i < 6; i++){
            AddBlock(i, Game.GRID_HEIGHT-1, _color, visible);
          }
          for(int i=3; i < 5; i++){
            AddBlock(i, Game.GRID_HEIGHT-2, _color, visible);
          }
          break;
        case Piece.J:
          _color = "blue";
          /*
            xxx###xxxx
            xxxxx#xxxx
            xxxxxxxxxx
          */
          for(int i=3; i < 6; i++){
            AddBlock(i, Game.GRID_HEIGHT-1, _color, visible);
          }
          AddBlock(5, Game.GRID_HEIGHT-2, _color, visible);
          break;
        case Piece.L:
          _color = "orange";
          /*
            xxx###xxxx
            xxx#xxxxxx
            xxxxxxxxxx
          */
          for(int i=3; i < 6; i++){
            AddBlock(i, Game.GRID_HEIGHT-1, _color, visible);
          }
          AddBlock(3, Game.GRID_HEIGHT-2, _color, visible);
          break;
        case Piece.O:
          _color = "yellow";
          /*
            xxxx##xxxx
            xxxx##xxxx
            xxxxxxxxxx
          */
          for(int i=4; i < 6; i++){
            AddBlock(i, Game.GRID_HEIGHT-1, _color, visible);
            AddBlock(i, Game.GRID_HEIGHT-2, _color, visible);
          }
          break;
        case Piece.T:
          _color = "cyan";
          /*
            xxx###xxxx
            xxxx#xxxxx
            xxxxxxxxxx
          */
          for(int i=3; i < 6; i++){
            AddBlock(i, Game.GRID_HEIGHT-1, _color, visible);
          }
          AddBlock(4, Game.GRID_HEIGHT-2, _color, visible);
          break;
      }

      _rendered = true;
    }

    private bool Move(int xOffset, int yOffset){
      foreach(Block block in _current){
        Block nb = _blocks.Find(b => b.x == block.x+xOffset && b.y == block.y+yOffset);
        if(_current.IndexOf(nb)==-1){
          if (nb!=null){
            if(!nb.empty){
              return false;
            }
          }
          else{
            return false;
          }
        }
      }
      List<Block> newBlocks = new List<Block>();
      foreach(Block block in _current){
        if(newBlocks.IndexOf(block)==-1){
          block.empty = true;
        }
        Block nb = _blocks.Find(b => b.x == block.x+xOffset && b.y == block.y+yOffset);
        if (nb!=null){
          nb.color = _color;
          nb.empty = false;
          newBlocks.Add(nb);
        }
      }
      _current = newBlocks;
      return true;
    }

    public bool MoveDown(){
      return Move(0, -1);
    }
    public bool MoveLeft(){
      return Move(-1, 0);
    }
    public bool MoveRight(){
      return Move(1, 0);
    }

    public override string ToString(){
      string blocks = "";
      foreach(Block block in _current){
        blocks += " ("+block.x+","+block.y+","+block.color+")";
      }
      return "Piece "+blocks;
    }

    private void AddBlock(int x, int y, string color, bool visible){
      Block b = _blocks.Find(block => block.x == x && block.y == y);
      b.color = color;
      b.empty = false;
      b.visible = visible;
      _current.Add(b);
    }
  }
}