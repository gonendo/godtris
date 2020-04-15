using System.Collections.Generic;
using Godot;
namespace Godtris{
  public class TGMRotation{
    private static int[] I_state_0 = {-2, -1, -1, 0, 0, 1, 1, 2};
    private static int[] I_state_1 = {2, 1, 1, 0, 0, -1, -1, -2};
    private static int[] Z_state_0 = {-2, -1, -1, 0, 0, -1, 1, 0};
    private static int[] Z_state_1 = {2, 1, 1, 0, 0, 1, -1, 0};
    private static int[] S_state_0 = {1, 0, 2, -1, -1, 0, 0, -1};
    private static int[] S_state_1 = {-1, 0, -2, 1, 1, 0, 0, 1};
    private static int[] T_state_1 = {1, -1, 0, 0, -1, 1, 1, 1};
    private static int[] T_state_2 = {1, 0, 0, -1, -1, -2, -1, 0};
    private static int[] T_state_3 = {-1, 2, 0, 1, 1, 0, -1, 0};
    private static int[] T_state_4 = {-1, -1, 0, 0, 1, 1, 1, -1};
    private static int[] T_state_4_R = {1, 1, 0, 0, -1, -1, -1, 1};
    private static int[] T_state_3_R = {1, -2, 0, -1, -1, 0, 1, 0};
    private static int[] T_state_2_R = {-1, 0, 0, 1, 1, 2, 1, 0};
    private static int[] T_state_1_R = {-1, 1, 0, 0, 1, -1, -1, -1};
    private static int[] J_state_1 = {1, -1, 0, 0, -1, 1, 0, 2};
    private static int[] J_state_2 = {1, 0, 0, -1, -1, -2, -2, -1};
    private static int[] J_state_3 = {-1, 2, 0, 1, 1, 0, 0, -1};
    private static int[] J_state_4 = {-1, -1, 0, 0, 1, 1, 2, 0};
    private static int[] J_state_4_R = {1, 1, 0, 0, -1, -1, -2, 0};
    private static int[] J_state_3_R = {1, -2, 0, -1, -1, 0, 0, 1};
    private static int[] J_state_2_R = {-1, 0, 0, 1, 1, 2, 2, 1};
    private static int[] J_state_1_R = {-1, 1, 0, 0, 1, -1, 0, -2};
    private static int[] L_state_1 = {1, -1, 0, 0, -1, 1, 2, 0};
    private static int[] L_state_2 = {1, 0, 0, -1, -1, -2, 0, 1};
    private static int[] L_state_3 = {-1, 2, 0, 1, 1, 0, -2, 1};
    private static int[] L_state_4 = {-1, -1, 0, 0, 1, 1, 0,-2};
    private static int[] L_state_4_R = {1, 1, 0, 0, -1, -1, 0, 2};
    private static int[] L_state_3_R = {1, -2, 0, -1, -1, 0, 2, -1};
    private static int[] L_state_2_R = {-1, 0, 0, 1, 1, 2, 0, -1};
    private static int[] L_state_1_R = {-1, 1, 0, 0, 1, -1, -2, 0};

    public static List<Block> RotateLeft(List<Block> blocks, Piece piece){
      switch(piece.name){
        case Piece.I:
          return ChangeState(blocks, piece, piece.rotation_state == 0 ? I_state_1 : I_state_0, piece.rotation_state == 0 ? 1 : 0);
        case Piece.Z:
          return ChangeState(blocks, piece, piece.rotation_state == 0 ? Z_state_1 : Z_state_0, piece.rotation_state == 0 ? 1 : 0);
        case Piece.S:
          return ChangeState(blocks, piece, piece.rotation_state == 0 ? S_state_1 : S_state_0, piece.rotation_state == 0 ? 1 : 0);
        case Piece.T:
          int T_stateId = piece.rotation_state-1;
          int[] T_state = null;
          switch(T_stateId){
            case -1:
              T_state = T_state_1;
              break;
            case 2:
            case -2:
              T_state = T_state_2;
              break;
            case 1:
            case -3:
              T_state = T_state_3;
              break;
            case 0:
            case -4:
              T_state = T_state_4;
              T_stateId = 0;
              break;
          }
          return ChangeState(blocks, piece, T_state, T_stateId);
        case Piece.J:
          int J_stateId = piece.rotation_state-1;
          int[] J_state =null;
          switch(J_stateId){
            case -1:
              J_state = J_state_1;
              break;
            case 2:
            case -2:
              J_state = J_state_2;
              break;
            case 1:
            case -3:
              J_state = J_state_3;
              break;
            case 0:
            case -4:
              J_state = J_state_4;
              J_stateId = 0;
              break;
          }
          return ChangeState(blocks, piece, J_state, J_stateId);
        case Piece.L:
          int L_stateId = piece.rotation_state-1;
          int[] L_state =null;
          switch(L_stateId){
            case -1:
              L_state = L_state_1;
              break;
            case 2:
            case -2:
              L_state = L_state_2;
              break;
            case 1:
            case -3:
              L_state = L_state_3;
              break;
            case 0:
            case -4:
              L_state = L_state_4;
              L_stateId = 0;
              break;
          }
          return ChangeState(blocks, piece, L_state, L_stateId);
      }

      return null;
    }

    public static List<Block> RotateRight(List<Block> blocks, Piece piece){
      switch(piece.name){
        case Piece.I:
          return ChangeState(blocks, piece, piece.rotation_state == 0 ? I_state_1 : I_state_0, piece.rotation_state == 0 ? 1 : 0);
        case Piece.Z:
          return ChangeState(blocks, piece, piece.rotation_state == 0 ? Z_state_1 : Z_state_0, piece.rotation_state == 0 ? 1 : 0);
        case Piece.S:
          return ChangeState(blocks, piece, piece.rotation_state == 0 ? S_state_1 : S_state_0, piece.rotation_state == 0 ? 1 : 0);
        case Piece.T:
          int stateId = piece.rotation_state+1;
          int[] state = null;
          switch(stateId){
            case -1:
            case 3:
              state = T_state_2_R;
              break;
            case 1:
              state = T_state_4_R;
              break;
            case 2:
            case -2:
              state = T_state_3_R;
              break;
            case 0:
            case 4:
              state = T_state_1_R;
              stateId = 0;
              break;
          }
          return ChangeState(blocks, piece, state, stateId);
        case Piece.J:
          int J_stateId = piece.rotation_state+1;
          int[] J_state =null;
          switch(J_stateId){
            case -1:
            case 3:
              J_state = J_state_2_R;
              break;
            case 1:
              J_state = J_state_4_R;
              break;
            case 2:
            case -2:
              J_state = J_state_3_R;
              break;
            case 0:
            case 4:
              J_state = J_state_1_R;
              J_stateId = 0;
              break;
          }
          return ChangeState(blocks, piece, J_state, J_stateId);
        case Piece.L:
          int L_stateId = piece.rotation_state+1;
          int[] L_state =null;
          switch(L_stateId){
            case -1:
            case 3:
              L_state = L_state_2_R;
              break;
            case 1:
              L_state = L_state_4_R;
              break;
            case 2:
            case -2:
              L_state = L_state_3_R;
              break;
            case 0:
            case 4:
              L_state = L_state_1_R;
              L_stateId = 0;
              break;
          }
          return ChangeState(blocks, piece, L_state, L_stateId);
      }

      return null;
    }

    private static List<Block> ChangeState(List<Block> blocks, Piece piece, int[] state, int stateId){
      List<Block> newblocks = new List<Block>();
      int i=0;
      bool fail = false;

      foreach(Block block in piece.GetBlocks()){
        int block_x = block.x + state[i];
        int block_y = block.y + state[i+1];

        Block newBlock = blocks.Find(b => b.x == block_x && b.y == block_y);
        if(newBlock==null || (!newBlock.empty && piece.GetBlocks().IndexOf(newBlock)==-1)){
          fail = true;
          break;
        }
        newblocks.Add(newBlock);
        i+=2;
      }

      //Wall kicks
      if(fail){
        i=0;
        fail = false;
        newblocks.Clear();
        foreach(Block block in piece.GetBlocks()){
          int block_x = block.x + state[i] + 1;
          int block_y = block.y + state[i+1];

          Block newBlock = blocks.Find(b => b.x == block_x && b.y == block_y);
          if(newBlock==null || (!newBlock.empty && piece.GetBlocks().IndexOf(newBlock)==-1)){
            fail = true;
            break;
          }
          newblocks.Add(newBlock);
          i+=2;
        }
      }

      if(fail){
        i=0;
        fail = false;
        newblocks.Clear();
        foreach(Block block in piece.GetBlocks()){
          int block_x = block.x + state[i] - 1;
          int block_y = block.y + state[i+1];

          Block newBlock = blocks.Find(b => b.x == block_x && b.y == block_y);
          if(newBlock==null || (!newBlock.empty && piece.GetBlocks().IndexOf(newBlock)==-1)){
            fail = true;
            break;
          }
          newblocks.Add(newBlock);
          i+=2;
        }
      }

      if(fail){
        return null;
      }

      piece.rotation_state = stateId;
      return newblocks;
    }

  }
}