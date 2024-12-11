namespace SpaceBattle.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class ThreadShardingServerTest
{
    public ThreadShardingServerTest()
    {
		new InitScopeBasedIoCImplementationCommand().Execute();
		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();
	}

	[Fact]
	public void EmptyQueueRecieverAdapterTest()
	{
		var fkq = new Queue<SpaceBattle.Lib.ICommand>();
	
		var fkrc = new QueueRecieverAdapter(fkq);

		Assert.True(fkrc.isEmpty());
	}

	[Fact]
	public void NotEmptyQueueRecieverAdapterTest()
	{
		var fkq = new Queue<SpaceBattle.Lib.ICommand>();
		var fkcmd = new Mock<SpaceBattle.Lib.ICommand>();
		fkq.Enqueue(fkcmd.Object);
	
		var fkrc = new QueueRecieverAdapter(fkq);

		Assert.False(fkrc.isEmpty());
	}

	[Fact]
	public void NullCommandIsCommandTest()
	{

		SpaceBattle.Lib.ICommand nullcmd = (SpaceBattle.Lib.ICommand)new NullCommandStrategy().run_strategy();

		Assert.True(nullcmd is ActionCommand);
		nullcmd.Execute();
	}

	[Fact]
	public void SendCommandTest()
	{
		var tdeps = (SpaceBattle.Lib.ICommand)new RegisterThreadDepsStrategy().run_strategy();
		tdeps.Execute();
		var sc = IoC.Resolve<object>("Game.Session.NewScope", "sc1");
		var deps = (SpaceBattle.Lib.ICommand)new RegisterGameDepsStrategy().run_strategy(sc);
		deps.Execute();
		var fkcmd = new Mock<SpaceBattle.Lib.ICommand>();
		fkcmd.Setup(o => o.Execute()).Verifiable();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Deserialize", (object[] args) => fkcmd.Object).Execute();

		new SendToGameCommand("fake_schema", "sc1").Execute();

		IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Dequeue.Command").Execute();
		fkcmd.Verify();
	}

	[Fact]
	public void GameCreationTest()
	{
		var tdeps = (SpaceBattle.Lib.ICommand)new RegisterThreadDepsStrategy().run_strategy();
		tdeps.Execute();
		var sc = IoC.Resolve<object>("Game.Session.NewScope", "sc1");
		var deps = (SpaceBattle.Lib.ICommand)new RegisterGameDepsStrategy().run_strategy(sc);
		deps.Execute();
		var fkcmd = new Mock<SpaceBattle.Lib.ICommand>();
		fkcmd.Setup(o => o.Execute()).Verifiable();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Register.Dependencies", (object[] args) => fkcmd.Object).Execute();

		var gcmd = (SpaceBattle.Lib.ICommand)new GameCreatorStrategy().run_strategy("sc1", "sc1");

		fkcmd.Verify();
		Assert.True(gcmd is GameCommand);
	}
}
