namespace SpaceBattle.Lib;

public class CodeGenerationTest
{
	[Fact]
	public void GoodResTest()
	{
		var ag = new AdapterGenerator(typeof(IMovable));
		var a = ag.Generate();
		string r = $"namespace SpaceBattle.Lib;public class IMovableAdapter:IMovable{{IUObject target;public SpaceBattle.Lib.Vector position{{get => target.get_property(\"position\");set => target.set_property(\"position\", value);}}public SpaceBattle.Lib.Vector velocity{{get => target.get_property(\"velocity\");}}public IMovableAdapter(IUObject target){{this.target=target;}}}}";
		Assert.Equal(a, r);
	}
}
