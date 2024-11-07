namespace SpaceBattle.Lib;

using Hwdtech;

public class MessageToCommand : IStrategy
{
	public object run_strategy(params object[] args)
	{
		var message = (IMessage)args[0];
		var gameObj = IoC.Resolve<IUObject>("Message.Get.IUObject", message.objId);

		message.properties.ToList().ForEach(p => gameObj.set_property(p.Key, p.Value));
		return IoC.Resolve<ICommand>("Game.Get." + message.cmd + "Command", gameObj);
	}
}

