using Calculator.Domain.Models;

namespace Calculator.Domain.Tests;

public class MathExpressionTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Empty_expression_throws_argument_exception(string? expression)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new MathExpression(expression!);
        });
    }
    
    [Fact]
    public void Valid_expression_is_created()
    {
        var expression = new MathExpression("2 + 3 * 5");

        Assert.Equal("2 + 3 * 5", expression.Value);
    }
}