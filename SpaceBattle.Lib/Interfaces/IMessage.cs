namespace SpaceBattle.Lib;

public interface IMessage
{
	public string cmd {get;}
	public string gameId {get;}
	public string objId {get;}
	public IDictionary<string, object> properties {get;}
}

