namespace SpaceBattle.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

class GetPropertyStrategy : IStrategy
{
    public object run_strategy(params object[] args)
    {
        string prop = (string)args[0];
        IUObject obj = (IUObject)args[1];
        return (obj.get_property(prop));
    }
}

public class CollisionCheckUnitTest
{
    public CollisionCheckUnitTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var GetProperty = new GetPropertyStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.GetProperty", (object[] args) => GetProperty.run_strategy(args)).Execute();

        var NodeT = new TrueNode<int>();
        var NodeF = new FalseNode<int>();
        var DictLeft = new Dictionary<int, INode<int>>()
        {
            {0, NodeF},
            {1, NodeT}
        };
        var NodeL = new CollisionNode<int>(DictLeft, 1);
        var DictRight = new Dictionary<int, INode<int>>()
        {
            {0, NodeF},
            {1, NodeT}
        };
        var NodeR = new CollisionNode<int>(DictRight, 1);
        var DictRoot = new Dictionary<int, INode<int>>()
        {
            {0, NodeL},
            {1, NodeR}
        };
        var Root = new CollisionNode<int>(DictRoot, 0);
        var GetTreeStrategy = new Mock<IStrategy>();
        GetTreeStrategy.Setup(o => o.run_strategy()).Returns(Root);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Trees.Collision", (object[] args) => GetTreeStrategy.Object.run_strategy(args)).Execute();

        var GetDecisionStrategy = new CollisionCheckStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DecisionTree.Collision", (object[] args) => GetDecisionStrategy.run_strategy(args)).Execute();
    }
    [Fact]
    public void CollisionCheckFalse()
    {
        var first = new Mock<IUObject>();
        first.Setup(obj => obj.get_property("position")).Returns(new Vector(1));
        first.Setup(obj => obj.get_property("velocity")).Returns(new Vector(2));
        var second = new Mock<IUObject>();
        second.Setup(obj => obj.get_property("position")).Returns(new Vector(1));
        second.Setup(obj => obj.get_property("velocity")).Returns(new Vector(2));
        CollisionCheckCommand ccc = new CollisionCheckCommand(first.Object, second.Object);

        ccc.Execute();

    }
    [Fact]
    public void CollisionCheckTrue()
    {
        var first = new Mock<IUObject>();
        first.Setup(obj => obj.get_property("position")).Returns(new Vector(1));
        first.Setup(obj => obj.get_property("velocity")).Returns(new Vector(3));
        var second = new Mock<IUObject>();
        second.Setup(obj => obj.get_property("position")).Returns(new Vector(1));
        second.Setup(obj => obj.get_property("velocity")).Returns(new Vector(2));
        CollisionCheckCommand ccc = new CollisionCheckCommand(first.Object, second.Object);


        Assert.Throws<Exception>(() => ccc.Execute());
    }
}

