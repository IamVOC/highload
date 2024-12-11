namespace SpaceBattle.Lib;

using Hwdtech;

public class ThreadManager
{
	private Dictionary<string, ISender> _threadSenders = new Dictionary<string, ISender>();

	public ThreadManager()
	{
		int threadsNum = Convert.ToInt32(Environment.GetEnvironmentVariable("THREADS_NUM"));
		int gameNum = Convert.ToInt32(Environment.GetEnvironmentVariable("THREADS_GAME_NUM"));
		for(int i = 0; i < threadsNum; i++) {
			String id = i.ToString();
			Action delact = () =>
			{
				ISender gameSender = IoC.Resolve<ISender>("Game.Threads.Inner.GetSender", id);
				for(int j = 1; j <= gameNum; j++) {
					IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
					String gid = (Convert.ToInt32(id) * gameNum + j).ToString();				
					SpaceBattle.Lib.ICommand gcmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Session.Create", gid, gid);
					gameSender.Send(gcmd);
				};
			};
			IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Start", id, delact).Execute();
			ISender sender = IoC.Resolve<ISender>("Game.Threads.Outer.GetSender", id);
			_threadSenders[id] = sender;
		};
	}

	public ISender GetSender(String id)
	{
		return _threadSenders[id];
	}
}
