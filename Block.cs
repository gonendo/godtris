using Godot;
namespace Godtris{
  public class Block{
    public int x; // 0 to 9
    public int y; // 0 to 19
    private string _color;
    private Game _game;
    private Spatial _block;
    private bool _empty;

    public Block(int x, int y, Game game){
      this.x = x;
      this.y = y;
      this._game = game;
      this._block = Godtris.Utils.GetBlock(Color.ColorN("black"));
      this.empty = true;
      this._block.Translate(new Vector3(
      this._game.tetrionBottomLeftPosition.x + this.x * Godtris.Utils.BLOCK_SIZE, 
      this._game.tetrionBottomLeft.Transform.origin.y + this.y * Godtris.Utils.BLOCK_SIZE, 
      0));
      this._game.AddChild(this._block);
    }

    public string color{
      get{
        return _color;
      }
      set{
        MeshInstance mesh = _block.GetNode("MeshInstance") as MeshInstance;
        SpatialMaterial material = mesh.GetSurfaceMaterial(0) as SpatialMaterial;
        _color = value;
        material.AlbedoColor = Color.ColorN(_color);
      }
    }

    public void Render(){
      color = _color;
    }

    public bool empty{
      get{
        return _empty;
      }
      set{
        if(value==true){
          color = "black";
        }
        _empty = value;
      }
    }
  }
}
