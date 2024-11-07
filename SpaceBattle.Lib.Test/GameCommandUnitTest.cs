namespace SpaceBattle.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class GameCommandUnitTest
{
	Dictionary<string, IReciever> regt;

	public class GameCommandReciever : IReciever
	{
		Queue<SpaceBattle.Lib.ICommand> q;
		public GameCommandReciever(Queue<SpaceBattle.Lib.ICommand> q)
		{
			this.q = q;
		}

		public SpaceBattle.Lib.ICommand Recieve()
		{
			if(q.Count == 0)
			{
				var mc = new Mock<SpaceBattle.Lib.ICommand>();
				mc.Setup(o => o.Execute());
				return mc.Object;
			}
			return q.Dequeue();
		}

		public bool isEmpty()
		{
			return q.Count == 0;
		}
	}

	object ic;
	Queue<SpaceBattle.Lib.ICommand> q1;

	public GameCommandUnitTest()
	{
		new InitScopeBasedIoCImplementationCommand().Execute();
		ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();

		q1 = new Queue<SpaceBattle.Lib.ICommand>();
		this.regt = new Dictionary<string, IReciever>();
		regt.Add("1", new GameCommandReciever(q1));

		var GetGameReciever = new Mock<IStrategy>();
		GetGameReciever.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => this.regt[(string)args[0]]);
		var ScopeGame = new Mock<IStrategy>();
		ScopeGame.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => ic);
		var TimeSpanGame = new Mock<IStrategy>();
		TimeSpanGame.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => new TimeSpan(100));

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Sessions.GetReciever", (object[] args) => GetGameReciever.Object.run_strategy(args)).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Sessions.Scope", (object[] args) => ScopeGame.Object.run_strategy(args)).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Sessions.TimeSpan", (object[] args) => TimeSpanGame.Object.run_strategy(args)).Execute();
	}

	[Fact]
	public void GameCommandTest()
	{
		AutoResetEvent waiter = new AutoResetEvent(false);
		var gc = new GameCommand("1");
		var fcmd = new Mock<SpaceBattle.Lib.ICommand>();
		fcmd.Setup(o => o.Execute()).Callback(() => waiter.Set()).Verifiable();

		q1.Enqueue(fcmd.Object);
		gc.Execute();

		waiter.WaitOne();
		fcmd.Verify();
	}

	[Fact]
	public void ExceptionTest()
	{
		var exh = new Mock<SpaceBattle.Lib.ICommand>();
		exh.Setup(o => o.Execute()).Verifiable();
		var ExHandler = new Mock<IStrategy>();
		ExHandler.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => exh.Object);
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.ExceptionHandler", (object[] args) => ExHandler.Object.run_strategy(args)).Execute();
		var gc = new GameCommand("1");
		var fcmd = new Mock<SpaceBattle.Lib.ICommand>();
		fcmd.Setup(o => o.Execute()).Throws(new Exception());

		q1.Enqueue(fcmd.Object);
		gc.Execute();

		exh.Verify();
	}
}

