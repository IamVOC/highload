namespace SpaceBattle.Lib.Test;

using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class ThreadShardingDepsTest
{
    public ThreadShardingDepsTest()
    {
		new InitScopeBasedIoCImplementationCommand().Execute();
		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();

		SpaceBattle.Lib.ICommand tds = (SpaceBattle.Lib.ICommand)new RegisterThreadDepsStrategy().run_strategy();
		tds.Execute();
		SpaceBattle.Lib.ICommand gds = (SpaceBattle.Lib.ICommand)new RegisterGameDepsStrategy().run_strategy(ic);
		gds.Execute();
	}

	[Fact]
	public void GetObjectDepTest()
	{
		var fkship = new Mock<IUObject>();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Add.Object", "ship1", fkship.Object).Execute();

		var ship = IoC.Resolve<IUObject>("Game.Get.Object", "ship1");

		Assert.True(fkship.Object == ship);
	}

	[Fact]
	public void DeleteObjectDepTest()
	{
		var fkship = new Mock<IUObject>();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Add.Object", "ship1", fkship.Object).Execute();

		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Delete.Object", "ship1").Execute();

		var d = IoC.Resolve<IDictionary<string, IUObject>>("Game.Get.Dict");
		Assert.False(d.ContainsKey("ship1"));
	}

	[Fact]
	public void EnqDeqDepTest()
	{
		var fkcmd = new Mock<SpaceBattle.Lib.ICommand>();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command", fkcmd.Object).Execute();

		var cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Dequeue.Command");

		Assert.True(fkcmd.Object == cmd);
	}

	[Fact]
	public void GetQueueDepTest()
	{
		var fkcmd = new Mock<SpaceBattle.Lib.ICommand>();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command", fkcmd.Object).Execute();

		var cmd = IoC.Resolve<Queue<SpaceBattle.Lib.ICommand>>("Game.Get.Queue").Dequeue();

		Assert.True(fkcmd.Object == cmd);
	}

	[Fact]
	public void GetRecieverDepTest()
	{
		var fkcmd = new Mock<SpaceBattle.Lib.ICommand>();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command", fkcmd.Object).Execute();

		var cmd = IoC.Resolve<IReciever>("Game.Sessions.GetReciever").Recieve();

		Assert.True(fkcmd.Object == cmd);
	}

	[Fact]
	public void FillThreadCollectionTest()
	{
		var fki = new BlockingCollection<SpaceBattle.Lib.ICommand>();
		var fko = new BlockingCollection<SpaceBattle.Lib.ICommand>();
		var tr = new MyThread(new RecieverAdapter(fki), new RecieverAdapter(fko));
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.SetThread", "t1", tr).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Inner.SetReciever", "t1", fki).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Inner.SetSender", "t1", fki).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Outer.SetReciever", "t1", fki).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Outer.SetSender", "t1", fki).Execute();

		var ntr = IoC.Resolve<MyThread>("Game.Threads.GetThread", "t1");
		var fkicmd = new Mock<SpaceBattle.Lib.ICommand>();
		var nisd = IoC.Resolve<ISender>("Game.Threads.Inner.GetSender", "t1");
		nisd.Send(fkicmd.Object);
		var nirc = IoC.Resolve<IReciever>("Game.Threads.Inner.GetReciever", "t1");
		var fkocmd = new Mock<SpaceBattle.Lib.ICommand>();
		var nosd = IoC.Resolve<ISender>("Game.Threads.Outer.GetSender", "t1");
		nosd.Send(fkocmd.Object);
		var norc = IoC.Resolve<IReciever>("Game.Threads.Outer.GetReciever", "t1");

		Assert.True(tr == ntr);
		var nicmd = nirc.Recieve();
		var nocmd = norc.Recieve();
		Assert.True(fkicmd.Object == nicmd);
		Assert.True(fkocmd.Object == nocmd);
	}

	[Fact]
	public void ScopeCheckDepTest()
	{

		var sc = IoC.Resolve<object>("Game.Session.NewScope", "sc1");

		var nsc = IoC.Resolve<object>("Game.Sessions.Scope", "sc1");
		Assert.True(sc == nsc);
	}
}
