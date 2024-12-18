namespace SpaceBattle.Lib;

using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.ObjectModel;
using Moq;

public class TransitDepsTest
{
    public TransitDepsTest()
    {
		new InitScopeBasedIoCImplementationCommand().Execute();
		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();

		SpaceBattle.Lib.ICommand strat = (SpaceBattle.Lib.ICommand)new RegisterThreadDepsStrategy().run_strategy();
		strat.Execute();
		SpaceBattle.Lib.ICommand tds = (SpaceBattle.Lib.ICommand)new RegisterTransitDepsStrategy().run_strategy();
		tds.Execute();
	}

	[Fact]
	public void NullObjTest()
	{
		IUObject obj = IoC.Resolve<IUObject>("Game.Object.Deserialize");

		obj.set_property("velocity", 10);
		object v = obj.get_property("velocity");

		Assert.True((int)v == 10);
	}

	[Fact]
	public void SerializeTest()
	{
		string gid = "1";	
		object ic = IoC.Resolve<object>("Game.Session.NewScope", gid);
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Scope.Register.Dependencies", ic).Execute();
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Sessions.Set.TimeSpan", new TimeSpan(200)).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Add.Object", "s1", new UObject()).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Add.Object", "s2", new UObject()).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Add.Object", "s3", new UObject()).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command", new ActionCommand(() => {})).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command", new ActionCommand(() => {})).Execute();

		string serGame = IoC.Resolve<string>("Game.Serialize", gid);

		Console.Write(serGame);
		Assert.Equal(serGame, "{\"objects\":{\"s1\":\"object\",\"s2\":\"object\",\"s3\":\"object\"},\"commands\":[\"command\",\"command\"],\"timeSpan\":\"00:00:00.0000200\"}");
	}

	[Fact]
	public void DeserializeTest()
	{
		var objects=new Dictionary<string, string>();
		var commands=new string[1];
		objects.Add("s1", "object");
		objects.Add("s2", "object");
		commands.Append("command");
		var sgs = new SerializedGameSchema{
			objects=objects,
			commands=commands,
			timeSpan="00:00:00.0000150"
		};

		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Deserialize", sgs);
		var sc = IoC.Resolve<ReadOnlyCollection<string>>("Game.Scopes");
		var gid = sc[0];

		var ic = IoC.Resolve<object>("Game.Sessions.Scope", gid);
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic);
		Dictionary<string, IUObject> d = IoC.Resolve<Dictionary<string, IUObject>>("Game.Get.Dict");
		Queue<ICommand> q = IoC.Resolve<Queue<ICommand>>("Game.Get.Queue");

		Assert.Equal(q.ToArray().Length, 1);
		Assert.Equal(d.Values.ToArray().Length, 2);
	}

	[Fact]
	public void CatchGameCommandTest()
	{
		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();

		var objects=new Dictionary<string, string>();
		var commands=new string[1];
		objects.Add("s1", "object");
		objects.Add("s2", "object");
		commands.Append("command");
		var sgs = new SerializedGameSchema{
			objects=objects,
			commands=commands,
			timeSpan="00:00:00.0000150"
		};
		var mcksd = new Mock<ISender>();
		mcksd.Setup(o => o.Send(It.IsAny<ICommand>())).Verifiable();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Deserialize", (object[] args) => new ActionCommand(() => {})).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Inner.GetSender", (object[] args) => mcksd.Object).Execute();

		new CatchGameCommand(sgs, "1").Execute();

		mcksd.Verify();
	}
}
