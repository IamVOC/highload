namespace SpaceBattle.Lib.Test;

using Moq;

public class MacroCommandUnitTest
{
    [Fact]
    public void MacroCommandCheck()
    {
        var first = new Mock<ICommand>();
        first.Setup(obj => obj.Execute()).Verifiable();
        var second = new Mock<ICommand>();
        second.Setup(obj => obj.Execute()).Verifiable();
        object[] targets = new object[] { first.Object, first.Object };
        MacroCommand macro = new MacroCommand(targets);

        macro.Execute();

        first.Verify(obj => obj.Execute());
    }
}
