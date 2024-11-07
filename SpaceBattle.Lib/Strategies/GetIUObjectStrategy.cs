namespace SpaceBattle.Lib;

using Hwdtech;

public class GetIUObject : IStrategy
{
	public object run_strategy(params object[] args)
	{
		string objId = (string)args[0];

		if (!IoC.Resolve<IDictionary<string, IUObject>>("Game.Get.UObjectsDictionary").TryGetValue(objId, out IUObject? obj))
        {
            throw new Exception();
        }
        else
        {
            return obj;
        }
	}
}

