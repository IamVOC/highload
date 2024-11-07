namespace SpaceBattle.Lib.Test;
using Moq;


public class RotateObjectsUnitTests
{
    [Fact]
    public void GoodRotateTest()
    {
        var rotate_obj = new Mock<IRotatable>();
        rotate_obj.Setup(rotate_obj => rotate_obj.angle).Returns(new Angle(45, 1)).Verifiable();
        rotate_obj.Setup(rotate_obj => rotate_obj.angle_speed).Returns(new Angle(90, 1)).Verifiable();
        var rotate_command = new RotateCommand(rotate_obj.Object);

        rotate_command.Execute();

        rotate_obj.VerifySet(rotate_obj => rotate_obj.angle = new Angle(135, 1), Times.Once);
        rotate_obj.VerifyAll();
    }

    [Fact]
    public void BadReadAngle()
    {
        Mock<IRotatable> rotate_obj = new Mock<IRotatable>();
        rotate_obj.SetupProperty(rotate_obj => rotate_obj.angle, new Angle(45, 1));
        rotate_obj.SetupGet<Angle>(rotate_obj => rotate_obj.angle_speed).Returns(new Angle(90, 1));
        rotate_obj.SetupSet(rotate_obj => rotate_obj.angle = It.IsAny<Angle>()).Throws<Exception>();
        var rotate_command = new RotateCommand(rotate_obj.Object);

        Assert.Throws<Exception>(() => rotate_command.Execute());
    }

    [Fact]
    public void BadReadAngleSpeed()
    {
        Mock<IRotatable> rotate_obj = new Mock<IRotatable>();
        rotate_obj.SetupProperty(rotate_obj => rotate_obj.angle, new Angle(45, 1));
        rotate_obj.SetupGet<Angle>(rotate_obj => rotate_obj.angle_speed).Throws<Exception>();
        var rotate_command = new RotateCommand(rotate_obj.Object);

        Assert.Throws<Exception>(() => rotate_command.Execute());
    }

    [Fact]
    public void BadGetAngle()
    {
        Mock<IRotatable> rotate_obj = new Mock<IRotatable>();
        rotate_obj.SetupProperty(rotate_obj => rotate_obj.angle, new Angle(45, 1));
        rotate_obj.SetupGet<Angle>(rotate_obj => rotate_obj.angle_speed).Returns(new Angle(90, 1));
        rotate_obj.SetupGet<Angle>(rotate_obj => rotate_obj.angle).Throws<Exception>();
        var rotate_command = new RotateCommand(rotate_obj.Object);

        Assert.Throws<Exception>(() => rotate_command.Execute());
    }
}
