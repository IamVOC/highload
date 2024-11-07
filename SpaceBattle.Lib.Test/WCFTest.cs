namespace SpaceBattle.Lib;

using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class WCFTest
{
	[Fact]
	public void ApiSendJson()
	{
        new InitScopeBasedIoCImplementationCommand().Execute();
		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();
		
		var fs = new Mock<IStrategy>();
		fs.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns(new Dictionary<string, object>());

		var cmd = new Mock<SpaceBattle.Lib.ICommand>();
		cmd.Setup(o => o.Execute()).Verifiable();

		var ss = new Mock<IStrategy>();
		ss.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns(cmd.Object);

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Api.Serialize", (object[] args) => fs.Object.run_strategy()).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Api.SendToGame", (object[] args) => ss.Object.run_strategy()).Execute();

		var cc = new CommandContract();
		var wa = new WebApi();


		wa.BodyQuery(cc);


		cmd.Verify();
	}
}
