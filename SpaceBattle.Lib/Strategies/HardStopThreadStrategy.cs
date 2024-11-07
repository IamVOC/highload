namespace SpaceBattle.Lib;

using Hwdtech;

public class HardStopThreadStrategy
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
        var cmd = new HardStopThreadCommand(id);
        var inp = new object[] { actingcmd, cmd };
        var mc = new MacroCommand(inp);
        var ret = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Commands.SendCommand", id, mc);
        return ret;
    }
}

