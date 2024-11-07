namespace SpaceBattle.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class CreateAndDeleteGameUnitTest
{
	Dictionary<string, object> scopes = new Dictionary<string, object>();
    public CreateAndDeleteGameUnitTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",  IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

		var GetScope = new Mock<IStrategy>();
		GetScope.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => scopes[(string)args[0]]);
		var NewScopeToDict = new Mock<IStrategy>();
		NewScopeToDict.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => {
				scopes.Add((string)args[0], IoC.Resolve<object>("Scopes.New", (object)args[1]));
				return scopes[(string)args[0]];
				});
		var DeleteScopeFromDict = new Mock<IStrategy>();
		DeleteScopeFromDict.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => scopes.Remove((string)args[0]));
		
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Session.GetScope", (object[] args) => GetScope.Object.run_strategy(args)).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Session.NewScope", (object[] args) => NewScopeToDict.Object.run_strategy(args)).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Delete.New", (object[] args) => DeleteScopeFromDict.Object.run_strategy(args)).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create.New", (object[] args) => new CreateScopeAndGameStrategy().run_strategy(args)).Execute();
		
    }

	[Fact]
	public void GoodCreateTowGamesAndDeleteOneTest()
	{
		var gameCommand = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Create.New", "Game1", "Scope1");
		var gameCommand2 = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Create.New", "Game1", "Scope2");

		Assert.True(scopes.Count == 2);
		Assert.True(scopes["Scope1"] != scopes["Scope2"]);

		IoC.Resolve<bool>("Game.Delete.New", "Scope1");

		Assert.True(scopes.Count == 1);

		IoC.Resolve<bool>("Game.Delete.New", "Scope2");

		Assert.True(scopes.Count == 0);
	}

	[Fact]
	public void TestRegisterTimeSpanInGameScope()
	{
		var gameCommand = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Create.New", "Game1", "Scope1");

		var ts = IoC.Resolve<TimeSpan>("Game.Get.TimeSpan");
		Assert.True(ts == new TimeSpan(100));
	}

	[Fact]
	public void TestRegisterDictOfObjectsInGameScope()
	{
		var gameCommand = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Create.New", "Game1", "Scope1");

		var obj = IoC.Resolve<IUObject>("Game.Get.Object", "Object1");
		var objDict = IoC.Resolve<Dictionary<string, IUObject>>("Game.Get.Dict");
		var testobj = new Mock<IUObject>().Object;
		Assert.True(objDict.Count == 1);
		Assert.True(obj.GetType() == testobj.GetType());

		var delFlag = IoC.Resolve<bool>("Game.Delete.Object", "Object1");
		Assert.True(delFlag == true);
		Assert.True(objDict.Count == 0);
		Assert.Throws<KeyNotFoundException>(() => IoC.Resolve<IUObject>("Game.Get.Object", "Object1"));
	}
	
	[Fact]
	public void TestRegisterQueueInGameScope()
	{
		var gameCommand = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Create.New", "Game1", "Scope1");

		var fireCommand = new Mock<SpaceBattle.Lib.ICommand>().Object;
		
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command", fireCommand).Execute();
		var cmdQueue = IoC.Resolve<Queue<SpaceBattle.Lib.ICommand>>("Game.Get.Queue");
		Assert.True(cmdQueue.Count == 1);

		SpaceBattle.Lib.ICommand fireCommandE = IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Dequeue.Command");
		Assert.True(fireCommand == fireCommandE);
		Assert.True(cmdQueue.Count == 0);
	}
}

