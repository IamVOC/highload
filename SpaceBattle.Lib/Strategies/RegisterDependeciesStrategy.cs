namespace SpaceBattle.Lib;

using Hwdtech;
using Moq;

public class RegisterDependenciesStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		object currScope = (object)args[0];

		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currScope).Execute();
		
		return new ActionCommand(()=> {
				Dictionary<string, IUObject> ships = new Dictionary<string, IUObject>() {{"Object1", new Mock<IUObject>().Object}};
				Queue<SpaceBattle.Lib.ICommand> queue = new Queue<SpaceBattle.Lib.ICommand>();
				var TimeSpanGame = new Mock<IStrategy>();
				TimeSpanGame.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => new TimeSpan(100));
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.TimeSpan", (object[] args) => TimeSpanGame.Object.run_strategy(args)).Execute();
				var GetObjcet = new Mock<IStrategy>();
				GetObjcet.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => ships[(string)args[0]]);
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Object", (object[] args) => GetObjcet.Object.run_strategy(args)).Execute();		
				var DeleteObject = new Mock<IStrategy>();
				DeleteObject.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => ships.Remove((string)args[0]));
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Delete.Object", (object[] args) => DeleteObject.Object.run_strategy(args)).Execute();
				var EnqueueCommand = new Mock<IStrategy>();
				EnqueueCommand.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => new ActionCommand(() => queue.Enqueue((SpaceBattle.Lib.ICommand)args[0])));
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Queue.Enqueue.Command", (object[] args) => EnqueueCommand.Object.run_strategy(args)).Execute();
				var DequeueCommand = new Mock<IStrategy>();
				DequeueCommand.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => queue.Dequeue());
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Dequeue.Command", (object[] args) => DequeueCommand.Object.run_strategy(args)).Execute();
				var GetDict = new Mock<IStrategy>();
				GetDict.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => ships);
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Dict", (object[] args) => GetDict.Object.run_strategy(args)).Execute();
				var GetQueue = new Mock<IStrategy>();
				GetQueue.Setup(o => o.run_strategy(It.IsAny<object[]>())).Returns((object[] args) => queue);
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Queue", (object[] args) => GetQueue.Object.run_strategy(args)).Execute();	
				});
	}
}
