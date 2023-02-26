namespace Core.Test;

public class UnitTest1
{
    private TestingClass _testingClass = new TestingClass();
    
    [Theory]
    [InlineData(3,2)]
    [InlineData(5,2)]
    [InlineData(0,2)]
    public void SumTest(int one,int two)
    {
        int result = one + two;
        Assert.True(_testingClass.Sum(one,two)==result);
    }
}