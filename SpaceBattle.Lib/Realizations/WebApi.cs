namespace SpaceBattle.Lib;
using CoreWCF;
using Hwdtech;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class WebApi : IWebApi
{
	public void BodyQuery(CommandContract cc)
	{
		var obj = IoC.Resolve<Dictionary<string, object>>("Game.Api.Serialize", cc);
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Api.SendToGame", obj).Execute();
	}
}

