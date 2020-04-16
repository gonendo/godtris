using Godot;
namespace Godtris{
  public static class Utils{
    public const float BLOCK_SIZE = 0.2f;
    private static PackedScene scene = ResourceLoader.Load<PackedScene>("res://Block.tscn");

    ///<summary>gets a colored block spatial</summary>
    ///<param name="color">Godot Color instance</param>
    public static Spatial GetBlock(Color color, uint flags=7){ 
      Spatial block = scene.Instance() as Spatial;
      MeshInstance mesh = block.GetNode("MeshInstance") as MeshInstance;
      SpatialMaterial material = new SpatialMaterial();
      material.AlbedoColor = color;
      material.Uv1Scale = new Vector3(3, 2, 1);
      Texture tx = GD.Load("res://assets/block.png") as Texture;
      tx.Flags = flags;
      material.AlbedoTexture = tx;
      mesh.SetSurfaceMaterial(0, material);
      return block;
    }
  }
}
