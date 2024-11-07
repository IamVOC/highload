namespace SpaceBattle.Lib;
using System.Reflection;

public class AdapterGenerator
{
	Type adaptee;
	public AdapterGenerator(Type adaptee)
	{
		this.adaptee = adaptee;
	}
	public object Generate()
	{
		PropertyInfo[] properties = adaptee.GetProperties();
		string newprops = "";
		foreach(PropertyInfo prop in properties)
		{
			string getter = string.Empty;
			string setter = string.Empty;
			if(prop.CanRead)
			{
				getter = $"get => target.get_property(\"{prop.Name}\");";
			}
			if(prop.CanWrite)
			{
				setter = $"set => target.set_property(\"{prop.Name}\", value);";
			}
			string newprop = $"public {prop.PropertyType} {prop.Name}{{{getter}{setter}}}";
			newprops += newprop;
		}
		string nspace = "namespace SpaceBattle.Lib;";
		string classhead = $"public class {adaptee.Name}Adapter:{adaptee.Name}{{";
		string targetfield = "IUObject target;";
		string constructor = $"public {adaptee.Name}Adapter(IUObject target){{this.target=target;}}";
		string code = nspace + classhead + targetfield + newprops + constructor + "}";
		return code;
	}
}
