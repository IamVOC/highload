namespace SpaceBattle.Lib;

using Hwdtech;

public class CollisionCheckCommand : ICommand
{
    private IUObject firstTarget;
    private IUObject secondTarget;

    public CollisionCheckCommand(IUObject firstObject, IUObject secondObject)
    {
        this.firstTarget = firstObject;
        this.secondTarget = secondObject;
    }

    public void Execute()
    {
        Vector firstPosition = (Vector)IoC.Resolve<Vector>("Game.UObject.GetProperty", "position", firstTarget);
        Vector secondPosition = (Vector)IoC.Resolve<Vector>("Game.UObject.GetProperty", "position", secondTarget);
        Vector firstVelocity = (Vector)IoC.Resolve<Vector>("Game.UObject.GetProperty", "velocity", firstTarget);
        Vector secondVelocity = (Vector)IoC.Resolve<Vector>("Game.UObject.GetProperty", "velocity", secondTarget);

        Vector deltaPosition = firstPosition - secondPosition;
        Vector deltaVelocity = firstVelocity - secondVelocity;

        bool areCollided = IoC.Resolve<bool>("Game.DecisionTree.Collision", deltaPosition, deltaVelocity);

        if (areCollided)
        {
            throw (new Exception("Objects are collided"));
        }
    }
}

