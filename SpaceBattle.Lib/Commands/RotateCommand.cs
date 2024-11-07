namespace SpaceBattle.Lib;

public class RotateCommand : ICommand {
  private readonly IRotatable rotate_obj;

  public RotateCommand(IRotatable obj) {
    rotate_obj = obj; 
  }

  public void Execute() {
    rotate_obj.angle += rotate_obj.angle_speed; 
  }
}
