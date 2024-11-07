namespace SpaceBattle.Lib;

using Hwdtech;

public class SendCommandCommand : SpaceBattle.Lib.ICommand
{
    string id;
    SpaceBattle.Lib.ICommand cmd;

    public SendCommandCommand(string id, SpaceBattle.Lib.ICommand cmd)
    {
        this.id = id;
        this.cmd = cmd;
    }

    public void Execute()
    {
        var sn = IoC.Resolve<ISender>("Game.Threads.GetSender", id);
        sn.Send(cmd);
    }
}

