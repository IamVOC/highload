namespace SpaceBattle.Lib.Test;
using Moq;
public class MovementUnitTests
{
    [Fact]
    public void GoodMoveTest()
    {
        var moving_obj = new Mock<IMovable>();
        moving_obj.Setup(a => a.position).Returns(new Vector(12, 5)).Verifiable();
        moving_obj.Setup(a => a.velocity).Returns(new Vector(-7, 3)).Verifiable();
        var move_command = new MoveCommand(moving_obj.Object);

        move_command.Execute();

        moving_obj.VerifySet(a => a.position = new Vector(5, 8), Times.Once);
        moving_obj.VerifyAll();
    }

    [Fact]
    public void ReadPositionError()
    {
        Mock<IMovable> m = new Mock<IMovable>();
        m.SetupProperty(m => m.position, new Vector(12, 5));
        m.SetupGet<Vector>(m => m.velocity).Returns(new Vector(-7, 3));
        m.SetupSet(m => m.position = It.IsAny<Vector>()).Throws<Exception>();
        var c = new MoveCommand(m.Object);


        Assert.Throws<Exception>(() => c.Execute());
    }

    [Fact]
    public void GetSpeedErr()
    {
        Mock<IMovable> m = new Mock<IMovable>();
        m.SetupProperty(m => m.position, new Vector(12, 5));
        m.SetupGet<Vector>(m => m.velocity).Throws<Exception>();
        var c = new MoveCommand(m.Object);


        Assert.Throws<Exception>(() => c.Execute());
    }

    [Fact]
    public void GetPosErr()
    {
        Mock<IMovable> m = new Mock<IMovable>();
        m.SetupProperty(m => m.position, new Vector(12, 5));
        m.SetupGet<Vector>(m => m.velocity).Returns(new Vector(-7, 3));
        m.SetupGet<Vector>(m => m.position).Throws<Exception>();
        var c = new MoveCommand(m.Object);


        Assert.Throws<Exception>(() => c.Execute());
    }
}

