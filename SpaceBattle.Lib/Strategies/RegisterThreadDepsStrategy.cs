namespace SpaceBattle.Lib;

using Hwdtech;
using System.Collections.Concurrent;

public class RegisterThreadDepsStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		ConcurrentDictionary<string, ThreadCollection> threads = new ConcurrentDictionary<string, ThreadCollection>();
		ConcurrentDictionary<string, object> scopes = new ConcurrentDictionary<string, object>(); 
		return new ActionCommand(() => {
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Start", (object[] args) => new CreateAndStartThreadStrategy().run_strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.AllThreads", (object[] args) => threads).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.SetThread", (object[] args) => new ActionCommand(() => {threads.GetOrAdd((string)args[0], new ThreadCollection());threads[(string)args[0]].tr = (MyThread)args[1];})).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Inner.SetReciever", (object[] args) => new ActionCommand(() => {threads.GetOrAdd((string)args[0], new ThreadCollection());threads[(string)args[0]].irc = new RecieverAdapter((BlockingCollection<SpaceBattle.Lib.ICommand>)args[1]);})).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Inner.SetSender", (object[] args) => new ActionCommand(() => {threads.GetOrAdd((string)args[0], new ThreadCollection());threads[(string)args[0]].isd = new SenderAdapter((BlockingCollection<SpaceBattle.Lib.ICommand>)args[1]);})).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Outer.SetReciever", (object[] args) => new ActionCommand(() => {threads.GetOrAdd((string)args[0], new ThreadCollection());threads[(string)args[0]].orc =  new RecieverAdapter((BlockingCollection<SpaceBattle.Lib.ICommand>)args[1]);})).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Outer.SetSender", (object[] args) => new ActionCommand(() => {threads.GetOrAdd((string)args[0], new ThreadCollection());threads[(string)args[0]].osd = new SenderAdapter((BlockingCollection<SpaceBattle.Lib.ICommand>)args[1]);})).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.GetThread", (object[] args) => threads[(string)args[0]].tr).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Inner.GetReciever", (object[] args) => threads[(string)args[0]].irc).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Inner.GetSender", (object[] args) => threads[(string)args[0]].isd).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Outer.GetReciever", (object[] args) => threads[(string)args[0]].orc).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Outer.GetSender", (object[] args) => threads[(string)args[0]].osd).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapters.CommandAdapter", (object[] args) => new ActionCommand((Action)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exceptions.WrongThread", (object[] args) => new Exception()).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.CreateThread", (object[] args) => new CreateAndStartThreadStrategy().run_strategy(args)).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Session.NewScope", (object[] args) => {
				scopes.TryAdd((string)args[0],
						IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));
				return scopes[(string)args[0]];
			}).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Sessions.Scope", (object[] args) => scopes[(string)args[0]]).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Session.Create", (object[] args) => new GameCreatorStrategy().run_strategy(args)).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scopes", (object[] args) => scopes.Keys).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Register.Dependencies", (object[] args) => new RegisterGameDepsStrategy().run_strategy(args)).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Deserialize", (object[] args) => new NullCommandStrategy().run_strategy(args)).Execute();
		});
	}
}
