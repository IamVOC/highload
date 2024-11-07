namespace SpaceBattle.Lib;

using Hwdtech;

public class CreateAndStartThreadStrategy
{
    public object run_strategy(params object[] args)
    {
        string id = (string)args[0];
        SpaceBattle.Lib.ICommand castc = new CreateAndStartThreadCommand(id);
        Action act = () => { };
        if (args.Length > 1)
        {
            act = (Action)args[1];
        }
        Action delact = () =>
        {
            IoC.Resolve<ISender>("Game.Threads.GetSender", id).Send(IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Adapters.CommandAdapter", act));
        };
        var regact = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Adapters.CommandAdapter", delact);
        object[] mcinp = new object[] { castc, regact };
        return new MacroCommand(mcinp);
    }
}

