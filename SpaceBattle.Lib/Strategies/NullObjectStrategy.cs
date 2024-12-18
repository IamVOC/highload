namespace SpaceBattle.Lib;

public class NullObjectStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		return new UObject();
	}
}
