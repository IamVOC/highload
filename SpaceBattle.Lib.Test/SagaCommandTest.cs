namespace SpaceBattle.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class SagaCommandUnitTest {

	public class AdaptStrategy : IStrategy {
		public object run_strategy(params object[] args)
		{
			return new Mock<object>();
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

		var mckcmd = new Mock<ICommand>();
		mckcmd.Setup(cmd => cmd.Execute()).Verifiable();
		var fkmckcmd = new Mock<ICommand>();
		fkmckcmd.Setup(cmd => cmd.Execute()).Throws(new Exception()).Verifiable();
		var mckcompcmd = new Mock<ICommand>();
		mckcompcmd.Setup(cmd => cmd.Execute()).Verifiable();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
				"Game.Commands.CreateCommand",
				(object[] args) => {return (string)args[1] == "ExceptionCommand" ? fkmckcmd : mckcmd;}).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
				"Game.Commands.CreateCompensatingCommand",
				(object[] args) => mckcompcmd).Execute();

	}
}
