namespace SpaceBattle.Lib;

using Hwdtech;

public class RegisterGameDepsStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		object currScope = (object)args[0];

		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currScope).Execute();
		
		return new ActionCommand(() => {
				Dictionary<string, IUObject> ships = new Dictionary<string, IUObject>() {};
				Queue<SpaceBattle.Lib.ICommand> queue = new Queue<SpaceBattle.Lib.ICommand>();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Sessions.TimeSpan", (object[] args) => (object)new TimeSpan(100)).Execute();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Add.Object", (object[] args) => new ActionCommand(() => {ships[(string)args[0]] = (IUObject)args[1];})).Execute();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Object", (object[] args) => ships[(string)args[0]]).Execute();	
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Delete.Object", (object[] args) => new ActionCommand(() => {ships.Remove((string)args[0]);})).Execute();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Enqueue.Command", (object[] args) => new ActionCommand(() => queue.Enqueue((SpaceBattle.Lib.ICommand)args[0]))).Execute();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Dequeue.Command", (object[] args) => queue.Dequeue()).Execute();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Dict", (object[] args) => ships).Execute();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Queue", (object[] args) => queue).Execute();
				IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Sessions.GetReciever", (object[] args) => new QueueRecieverAdapter(queue)).Execute();
		});
	}
}
