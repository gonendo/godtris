using Godot;
namespace Godtris{
  public class Block{
    public int x; // 0 to 9
    public int y; // 0 to 19
    private string _color;
    private Game _game;
    private Spatial _block;
    private bool _empty;
    private bool _locked;
    private bool _offgrid;

    public Block(int x, int y, Game game){
      this.x = x;
      this.y = y;
      this._game = game;
      this._block = Godtris.Utils.GetBlock(Color.ColorN("black"));
      if(y >= Game.GRID_HEIGHT){
        this._block.Hide();
        _offgrid = true;
        _empty = true;
      }
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
        if(_block!=null){
          MeshInstance mesh = _block.GetNode("MeshInstance") as MeshInstance;
          SpatialMaterial material = mesh.GetSurfaceMaterial(0) as SpatialMaterial;
          if(_offgrid){
            if(!empty){
              this._block.Show();
            }
            else{
              this._block.Hide();
            }
          }
          _color = value;
          material.AlbedoColor = Color.ColorN(_color);
          if(_locked){
            material.AlbedoColor = material.AlbedoColor.Darkened(0.5f);
          }
        }
      }
    }

    public bool locked{
      get{
        return _locked;
      }
      set{
        _locked = value;
      }
    }

    public bool visible{
      set{
        if(value){
          _block.Show();
        }
        else{
          _block.Hide();
        }
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
