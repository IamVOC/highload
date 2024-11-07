namespace SpaceBattle.Lib;

public class UpdateBehaviorCommand : ICommand
{
    MyThread thread;
    Action behavior;

    public UpdateBehaviorCommand(MyThread thread, Action behavior)
    {
        this.thread = thread;
        this.behavior = behavior;
    }

    public void Execute()
    {
        thread.UpdateBehavior(behavior);
    }
}

