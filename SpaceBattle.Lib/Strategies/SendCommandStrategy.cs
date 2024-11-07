namespace SpaceBattle.Lib;

public class SendCommandStrategy : IStrategy
{
    public object run_strategy(params object[] args)
    {
        string id = (string)args[0];
        ICommand cmd = (ICommand)args[1];
        var scc = new SendCommandCommand(id, cmd);
        return scc;
    }
}

