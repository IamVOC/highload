namespace SpaceBattle.Lib;

public class NullCommandStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		return new ActionCommand(() => { });
	}
}
