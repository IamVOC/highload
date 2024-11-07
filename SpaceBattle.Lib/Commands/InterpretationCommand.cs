namespace SpaceBattle.Lib;

using Hwdtech;

public class InterpretationCommand : ICommand
{
	IMessage interprMsg;

	public InterpretationCommand(IMessage msg)
	{
		interprMsg = msg;
	}

	public void Execute()
	{
		var cmd = IoC.Resolve<ICommand>("Game.Create.CommandFromMessage", interprMsg);

		IoC.Resolve<ICommand>("Game.Queue.Push", interprMsg.gameId, cmd).Execute();
	}
}

