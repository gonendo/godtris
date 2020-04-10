using Godot;
namespace Godtris{
  public class Controls{
    public const string LEFT_ACTION_ID = "move_left";
    public const string RIGHT_ACTION_ID = "move_right";
    public const string ROTATE_LEFT_ACTION_ID = "rotate_left";
    public const string ROTATE_RIGHT_ACTION_ID = "rotate_right";
    public const string SOFT_DROP_ACTION_ID = "soft_drop";
    public const string HARD_DROP_ACTION_ID = "hard_drop";
    
    private Mode _mode;

    public Controls(Mode mode){
      _mode = mode;
    }

    public void Update(){
      if (Input.IsActionPressed(LEFT_ACTION_ID)){
        _mode.MovePiece(LEFT_ACTION_ID);
      }
      else if(Input.IsActionPressed(RIGHT_ACTION_ID)){
        _mode.MovePiece(RIGHT_ACTION_ID);
      }
      else if(Input.IsActionPressed(SOFT_DROP_ACTION_ID)){
        _mode.MovePiece(SOFT_DROP_ACTION_ID);
      }
      else if(Input.IsActionJustPressed(HARD_DROP_ACTION_ID)){
        _mode.MovePiece(HARD_DROP_ACTION_ID);
      }
    }
  }
}