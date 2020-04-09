using Godot;
using System.Collections.Generic;
namespace Godtris{
  public class Piece{
    public const string I = "I";
    private string _color;
    private List<Block> _blocks;
    private List<Block> _current;
    public List<Block> GetBlocks(){
      return _current;
    }

    public Piece(string name, List<Block> blocks){
      _blocks = blocks;
      _current = new List<Block>();
      switch(name){
        case Piece.I:
          _color = "red";
          /*
            xxx####xxx
          */
          for(int i=3; i < 7; i++){
            Block b = blocks.Find(block => block.x == i && block.y == 19);
            b.color = _color;
            _current.Add(b);
          }
          break;
      }
    }

    public bool MoveDown(){
      foreach(Block block in _current){
        Block nb = _blocks.Find(b => b.x == block.x && b.y == block.y-1);
        if (nb!=null){
          if(!nb.IsEmpty()){
            return false;
          }
        }
        else{
          return false;
        }
      }
      List<Block> tmp = new List<Block>();
      foreach(Block block in _current){
        block.color = "black";
        Block nb = _blocks.Find(b => b.x == block.x && b.y == block.y-1);
        if (nb!=null){
          nb.color = _color;
          tmp.Add(nb);
        }
      }
      _current = tmp;
      return true;
    }
  }
}