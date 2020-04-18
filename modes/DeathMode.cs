using System.Collections.Generic;
namespace Godtris{
  public class DeathMode : TGM2Mode{
    public DeathMode(Game game, List<Block> blocks, int level) : base(game, blocks, level){
    }

    protected override void SetTimings(){
      _timings.Add(new int[2] {0, 99}, new int[5] {16,12,10,30,12});
      _timings.Add(new int[2] {100, 199}, new int[5] {12,6,10,26,6});
      _timings.Add(new int[2] {200, 299}, new int[5] {12,6,9,22,6});
      _timings.Add(new int[2] {300, 399}, new int[5] {6,6,8,18,6});
      _timings.Add(new int[2] {400, 499}, new int[5] {5,5,6,15,5});
      _timings.Add(new int[2] {500, 599}, new int[5] {4,4,6,15,4});
      _gravities.Add(0, 5120); // 20G
    }

    public override string GetTetrionColor(){
      return "ff0000";
    }
  }
}
