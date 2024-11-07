namespace SpaceBattle.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class SagaCommandUnitTest {

	Mock<SpaceBattle.Lib.ICommand> mckcmd;
	Mock<SpaceBattle.Lib.ICommand> fkmckcmd;
	Mock<SpaceBattle.Lib.ICommand> mckcompcmd;

	public class AdaptStrategy : IStrategy {
		public object run_strategy(params object[] args)
		{
			return new Mock<object>().Object;
		}
	}

	public SagaCommandUnitTest() {
		new InitScopeBasedIoCImplementationCommand().Execute();
		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();
		
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
				"Game.Commands.SagaCommand",
				(object[] args) => new CreateSagaCommand().run_strategy(args)).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
				"Game.Adapter.AdaptForCmd",
				(object[] args) => new AdaptStrategy().run_strategy(args)).Execute();

		this.mckcmd = new Mock<SpaceBattle.Lib.ICommand>();
		mckcmd.Setup(cmd => cmd.Execute()).Verifiable();
		this.fkmckcmd = new Mock<SpaceBattle.Lib.ICommand>();
		fkmckcmd.Setup(cmd => cmd.Execute()).Throws(new Exception()).Verifiable();
		this.mckcompcmd = new Mock<SpaceBattle.Lib.ICommand>();
		mckcompcmd.Setup(cmd => cmd.Execute()).Verifiable();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
				"Game.Commands.CreateCommand",
				(object[] args) =>
				{return (string)args[0] == "ExceptionCommand" ? fkmckcmd.Object : mckcmd.Object;}).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
				"Game.Saga.CreateCompensatingCommand",
				(object[] args) => mckcompcmd.Object).Execute();
	}

	[Fact]
    public void SuccessSagaCommand()
    {
		var uobj = new Mock<IUObject>();
		SpaceBattle.Lib.ICommand scmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Commands.SagaCommand",
				"SuccessCommand", "AnotherSuccessCommand", uobj.Object);

		scmd.Execute();

		this.mckcmd.Verify();
	}
}
