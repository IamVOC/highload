namespace SpaceBattle.Lib.Test;
using Moq;

public class AngleTests
{

    [Fact]
    public void GoodSum()
    {
        Angle a = new Angle(100, 1);
        Angle b = new Angle(45, 1);
        Assert.Equal(new Angle(145, 1), a + b);

    }

    [Fact]
    public void GoodEquals()
    {
        Angle a = new Angle(-10, 1);
        Angle b = new Angle(50, -5);
        Assert.True(a == b);
    }

    [Fact]
    public void GoodInequality()
    {
        Angle a = new Angle(10, 1);
        Angle b = new Angle(11, 1);
        Assert.True(a != b);
    }

    [Fact]
    public void GoodHashGood()
    {
        Angle a = new Angle(-10, -1);
        Angle b = new Angle(50, 5);
        Assert.True(a.GetHashCode() == b.GetHashCode());
    }

    [Fact]
    public void BadEquals()
    {
        Angle a = new Angle(10, 1);
        Angle b = new Angle(11, 1);
        Assert.False(a == b);
    }

    [Fact]
    public void BadEqualsMethod()
    {
        Angle a = new Angle(100, -1);
        int b = 999;
        Assert.False(a.Equals(b));
    }

    [Fact]
    public void ZeroDenominator()
    {
        Assert.Throws<ArgumentException>(()=> new Angle(1,0));
    }
}
