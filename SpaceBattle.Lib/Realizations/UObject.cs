namespace SpaceBattle.Lib;

public class UObject : IUObject
{
	private Dictionary<string, object> props = new Dictionary<string, object>();
	
	public void set_property(string key, object value)
	{
		this.props.TryAdd(key, value);
	}
    public object get_property(string key)
	{
		return this.props[key];
	}
}
