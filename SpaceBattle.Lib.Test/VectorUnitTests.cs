namespace SpaceBattle.Lib.Test;

public class VectorTests
{
    [Fact]
    public void VectorEqualGood()
    {
        Vector firstVector = new Vector(10, 7);
        Vector secondVector = new Vector(10, 7);

        bool result = firstVector == secondVector;

        Assert.True(result);
    }
    [Fact]
    public void VectorEqualBad()
    {
        Vector firstVector = new Vector(11, 8);
        Vector secondVector = new Vector(10, 7);

        bool result = firstVector == secondVector;

        Assert.False(result);
    }
    [Fact]
    public void DifferentSizeVectorEqual()
    {
        Vector firstVector = new Vector(10, 7, 1);
        Vector secondVector = new Vector(10, 7);

        bool result = firstVector == secondVector;

        Assert.False(result);
    }
    [Fact]
    public void VectorNotEqual()
    {
        Vector firstVector = new Vector(11, 8);
        Vector secondVector = new Vector(10, 7);

        bool result = firstVector != secondVector;

        Assert.True(result);
    }
    [Fact]
    public void VectorEqualWithNotVector()
    {
        Object notVector = new Object();
        Vector firstVector = new Vector(1, 1);

        bool result = firstVector.Equals(notVector);

        Assert.False(result);
    }
    [Fact]
    public void GetVectorCoordinateGood()
    {
        Vector firstVector = new Vector(5, 4, 2, 1, 5);

        int data = firstVector[2];

        Assert.Equal(2, data);
    }
    [Fact]
    public void GetVectorCoordinateBad()
    {
        Vector firstVector = new Vector(5, 4, 2, 1, 5);


        Assert.Throws<IndexOutOfRangeException>(() => firstVector[6]);
    }
    [Fact]
    public void SetVectorCoordinateGood()
    {
        Vector firstVector = new Vector(5, 4, 2, 1, 5);

        firstVector[2] = 5;

        Assert.Equal(5, firstVector[2]);
    }
    [Fact]
    public void SetVectorCoordinateBad()
    {
        Vector firstVector = new Vector(5, 4, 2, 1, 5);


        Assert.Throws<IndexOutOfRangeException>(() => firstVector[6] = 3);
    }
    [Fact]
    public void VectorSum()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(5, -3);

        Vector outVector = firstVector + secondVector;

        Assert.True(outVector == new Vector(17, 4));
    }
    [Fact]
    public void DifferentSizeVectorSum()
    {
        Vector firstVector = new Vector(12, 7, 1);
        Vector secondVector = new Vector(5, -3);


        Assert.Throws<ArgumentException>(() => firstVector + secondVector);
    }
    [Fact]
    public void VectorNeg()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(5, -3);

        Vector outVector = firstVector - secondVector;

        Assert.True(outVector == new Vector(7, 10));
    }
    [Fact]
    public void DifferentSizeVectorNeg()
    {
        Vector firstVector = new Vector(12, 7, 1);
        Vector secondVector = new Vector(5, -3);


        Assert.Throws<ArgumentException>(() => firstVector - secondVector);
    }
    [Fact]
    public void VectorMult()
    {
        Vector firstVector = new Vector(3, 2);
        int argument = 2;

        Vector outVector = argument * firstVector;

        Assert.True(outVector == new Vector(6, 4));
    }
    [Fact]
    public void VectorLesserBad()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(5, -3);

        bool result = firstVector < secondVector;

        Assert.False(result);
    }
    [Fact]
    public void VectorLesserGood()
    {
        Vector firstVector = new Vector(5, -3);
        Vector secondVector = new Vector(12, 7);

        bool result = firstVector < secondVector;

        Assert.True(result);
    }
    [Fact]
    public void VectorLesserSizeBad()
    {
        Vector firstVector = new Vector(12, 7, 0);
        Vector secondVector = new Vector(12, 7);

        bool result = firstVector < secondVector;

        Assert.False(result);
    }
    [Fact]
    public void VectorLesserSizeGood()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(12, 7, 0);

        bool result = firstVector < secondVector;

        Assert.True(result);
    }
    [Fact]
    public void CheckLesserEqualVectors()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(12, 7);

        bool result = firstVector < secondVector;

        Assert.False(result);
    }
    [Fact]
    public void VectorBiggerGood()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(5, -3);

        bool result = firstVector > secondVector;

        Assert.True(result);
    }
    [Fact]
    public void VectorBiggerBad()
    {
        Vector firstVector = new Vector(5, -3);
        Vector secondVector = new Vector(12, 7);

        bool result = firstVector > secondVector;

        Assert.False(result);
    }
    [Fact]
    public void VectorBiggerSizeGood()
    {
        Vector firstVector = new Vector(12, 7, 0);
        Vector secondVector = new Vector(12, 7);

        bool result = firstVector > secondVector;

        Assert.True(result);
    }
    [Fact]
    public void VectorBiggerSizeBad()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(12, 7, 0);

        bool result = firstVector > secondVector;

        Assert.False(result);
    }
    [Fact]
    public void CheckBiggerEqualVectors()
    {
        Vector firstVector = new Vector(12, 7);
        Vector secondVector = new Vector(12, 7);

        bool result = firstVector > secondVector;

        Assert.False(result);
    }
    [Fact]
    public void VectorToStringTest()
    {
        Vector firstVector = new Vector(12, 7);

        String result = firstVector.ToString();

        Assert.Equal("Vector(12,7)", result);
    }
    [Fact]
    public void EmptyVectorToStringTest()
    {
        Vector firstVector = new Vector();

        String result = firstVector.ToString();

        Assert.Equal("", result);
    }
    [Fact]
    public void GetHashCodeGood()
    {
        Vector firstVector = new Vector(1, 10);
        Vector secondVector = new Vector(1, 10);

        int firstHash = firstVector.GetHashCode();
        int secondHash = secondVector.GetHashCode();

        Assert.Equal(firstHash, secondHash);
    }
}

