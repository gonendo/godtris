using Godot;
using System.Collections.Generic;
namespace Godtris{
  public class Piece{
    public const string I = "I";
    public const string Z = "Z";
    public const string S = "S";
    public const string J = "J";
    public const string L = "L";
    public const string O = "O";
    public const string T = "T";
    private string _color;
    private List<Block> _blocks;
    private List<Block> _current;
    private string _name;
    public string name{
      get{
        return _name;
      }
    }
    public List<Block> GetBlocks(){
      return _current;
    }

    public Piece(string name, List<Block> blocks){
      _blocks = blocks;
      _current = new List<Block>();
      _name = name;
    }

    public void Render(){
      switch(_name){
        case Piece.I:
          _color = "red";
          /*
            xxx####xxx
            xxxxxxxxxx
            xxxxxxxxxx
          */
          for(int i=3; i < 7; i++){
            AddBlock(i, 19, _color);
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
            AddBlock(i, 19, _color);
          }
          for(int i=4; i < 6; i++){
            AddBlock(i, 18, _color);
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
            AddBlock(i, 19, _color);
          }
          for(int i=3; i < 5; i++){
            AddBlock(i, 18, _color);
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
            AddBlock(i, 19, _color);
          }
          AddBlock(5, 18, _color);
          break;
        case Piece.L:
          _color = "orange";
          /*
            xxx###xxxx
            xxx#xxxxxx
            xxxxxxxxxx
          */
          for(int i=3; i < 6; i++){
            AddBlock(i, 19, _color);
          }
          AddBlock(3, 18, _color);
          break;
        case Piece.O:
          _color = "yellow";
          /*
            xxxx##xxxx
            xxxx##xxxx
            xxxxxxxxxx
          */
          for(int i=4; i < 6; i++){
            AddBlock(i, 19, _color);
            AddBlock(i, 18, _color);
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
            AddBlock(i, 19, _color);
          }
          AddBlock(4, 18, _color);
          break;
      }
    }

    public bool MoveDown(){
      foreach(Block block in _current){
        Block nb = _blocks.Find(b => b.x == block.x && b.y == block.y-1);
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
        Block nb = _blocks.Find(b => b.x == block.x && b.y == block.y-1);
        if (nb!=null){
          nb.color = _color;
          nb.empty = false;
          newBlocks.Add(nb);
        }
      }
      _current = newBlocks;
      return true;
    }

    public override string ToString(){
      string blocks = "";
      foreach(Block block in _current){
        blocks += " ("+block.x+","+block.y+","+block.color+")";
      }
      return "Piece "+blocks;
    }

    private void AddBlock(int x, int y, string color){
      Block b = _blocks.Find(block => block.x == x && block.y == y);
      b.color = color;
      b.empty = false;
      _current.Add(b);
    }
  }
}