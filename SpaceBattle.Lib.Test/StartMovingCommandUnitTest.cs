namespace SpaceBattle.Lib.Test;

using Moq;
using Hwdtech;
using Hwdtech.Ioc;

class QueueRealization<T> : SpaceBattle.Lib.IQueue<T>
{
    private List<T> coms = new List<T>();
    public void Push(T com)
    {
        this.coms.Add(com);
    }
    public T Pop()
    {
        var ret = coms[0];
        coms.RemoveAt(0);
        return ret;
    }
}

class SetupPropertyStrategy : IStrategy
{
    public object run_strategy(params object[] args)
    {
        string prop = (string)args[0];
        IUObject obj = (IUObject)args[1];
        var val = args[2];
        var ret = new Mock<SpaceBattle.Lib.ICommand>();
        ret.Setup(com => com.Execute()).Callback(new Action(() => obj.set_property(prop, val)));
        return (ret.Object);
    }
}

class MovableAdapterStrategy : IStrategy
{
    public object run_strategy(params object[] args)
    {
        var obj = new Mock<IMovable>();
        return (obj.Object);
    }
}

class MoveCommandStrategy : IStrategy
{
    public object run_strategy(params object[] args)
    {
        var obj = new Mock<SpaceBattle.Lib.ICommand>();
        return (obj.Object);
    }
}

class PushStrategy : IStrategy
{
    public object run_strategy(params object[] args)
    {
        var queue = (SpaceBattle.Lib.IQueue<SpaceBattle.Lib.ICommand>)args[0];
        var com = (SpaceBattle.Lib.ICommand)args[1];
        var ret = new Mock<SpaceBattle.Lib.ICommand>();
        ret.Setup(com => com.Execute()).Callback(new Action(() => queue.Push(com)));
        return ret.Object;
    }
}

public class StartMovingCommandsUnitTests
{
    static StartMovingCommandsUnitTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var SetPropertyStrategy = new SetupPropertyStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.SetProperty", (object[] args) => SetPropertyStrategy.run_strategy(args)).Execute();

        var ToMovingAdapterStrategy = new MovableAdapterStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapters.IMovable", (object[] args) => ToMovingAdapterStrategy.run_strategy(args)).Execute();

        var GetMoveCommandStrategy = new MoveCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.MoveCommand", (object[] args) => GetMoveCommandStrategy.run_strategy(args)).Execute();

        var QueuePushStrategy = new PushStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => QueuePushStrategy.run_strategy(args)).Execute();
    }

    [Fact]
    public void StartMoveCommandGood()
    {
        var mcs_obj = new Mock<IMoveCommandStartable>();
        var uobj = new Mock<IUObject>();
        Vector vel = new Vector(1);
        QueueRealization<SpaceBattle.Lib.ICommand> q = new QueueRealization<SpaceBattle.Lib.ICommand>();
        mcs_obj.SetupGet(com => com.uobj).Returns(uobj.Object);
        mcs_obj.SetupGet(com => com.velocity).Returns(vel);
        mcs_obj.SetupGet(com => com.queue).Returns(q);
        var smc = new StartMoveCommand(mcs_obj.Object);

        smc.Execute();

        Assert.NotNull(mcs_obj.Object.queue.Pop());
    }
}

