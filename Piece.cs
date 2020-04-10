using Godot;
using System.Collections.Generic;
namespace Godtris{
  public class Piece{
    public const string I = "I";
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
          */
          for(int i=3; i < 7; i++){
            Block b = _blocks.Find(block => block.x == i && block.y == 19);
            b.color = _color;
            b.empty = false;
            _current.Add(b);
          }
          break;
      }
    }

    public bool MoveDown(){
      foreach(Block block in _current){
        Block nb = _blocks.Find(b => b.x == block.x && b.y == block.y-1);
        if (nb!=null){
          if(!nb.empty){
            return false;
          }
        }
        else{
          return false;
        }
      }
      List<Block> tmp = new List<Block>();
      foreach(Block block in _current){
        block.empty = true;
        Block nb = _blocks.Find(b => b.x == block.x && b.y == block.y-1);
        if (nb!=null){
          nb.color = _color;
          nb.empty = false;
          tmp.Add(nb);
        }
      }
      _current = tmp;
      return true;
    }

    public override string ToString(){
      string blocks = "";
      foreach(Block block in _current){
        blocks += " ("+block.x+","+block.y+","+block.color+")";
      }
      return "Piece "+blocks;
    }
  }
}