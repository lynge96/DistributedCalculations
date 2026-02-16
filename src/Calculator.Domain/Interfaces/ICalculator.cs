using Calculator.Domain.Models;

namespace Calculator.Domain.Interfaces;

public interface ICalculator
{
    decimal Calculate(Expression expression);
}