namespace SpaceBattle.Lib;

using Hwdtech;

public class SoftStopThreadStrategy
{
    public object run_strategy(params object[] args)
    {
        string id = (string)args[0];

        var act = () => { };
        if (args.Length > 1)
        {
            act = (Action)args[1];
        }

        SpaceBattle.Lib.ICommand actingcmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Adapters.CommandAdapter", act);
        var cmd = new SoftStopThreadCommand(id, act);
        var ret = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Commands.SendCommand", id, cmd);
        return ret;
    }
}

