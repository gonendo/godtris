using Godot;
using System.Collections.Generic;
namespace Godtris{
  public class TGMRandomizer{
    private List<Piece> _history;
    private Dictionary<int,string> _pieces;
    private RandomNumberGenerator _rng;
    private List<Block> _blocks;

    public TGMRandomizer(List<Block> blocks){
      _blocks = blocks;
      _pieces = new Dictionary<int, string>();
      _pieces.Add(0, Piece.I);
      _pieces.Add(1, Piece.Z);
      _pieces.Add(2, Piece.S);
      _pieces.Add(3, Piece.J);
      _pieces.Add(4, Piece.L);
      _pieces.Add(5, Piece.O);
      _pieces.Add(6, Piece.T);

      _history = new List<Piece>();
      _history.Add(new Piece(Piece.Z, _blocks));
      _history.Add(new Piece(Piece.Z, _blocks));
      _history.Add(new Piece(Piece.S, _blocks));
      _history.Add(new Piece(Piece.S, _blocks));
      
      _rng = new RandomNumberGenerator();
      _rng.Randomize();
    }

    public Piece GetNextPiece(){
      int r = 0;
      for(int i=0; i < 5; i++){
        r = _rng.RandiRange(0, 6);
        if(_history.Find(p => p.name == _pieces[r])==null){
          break;
        }
      }
      for(int i=_history.Count-1; i > 0; i--){
        _history[i] = _history[i-1];
      }
      _history.RemoveAt(0);
      string pieceName;
      _pieces.TryGetValue(r, out pieceName);
      _history.Insert(0, new Piece(pieceName, _blocks));

      return _history[0];
    }
  }
}