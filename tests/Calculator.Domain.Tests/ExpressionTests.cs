using Calculator.Domain.Models;

namespace Calculator.Domain.Tests;

public class ExpressionTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Empty_expression_throws_argument_exception(string? expression)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new Expression(expression!);
        });
    }
    
    [Fact]
    public void Expression_exceeding_max_string_length()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new Expression("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                           "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        });
    }
    
    [Fact]
    public void Valid_expression_is_created()
    {
        var expression = new Expression("2 + 3 * 5");

        Assert.Equal("2 + 3 * 5", expression.Value);
    }
}