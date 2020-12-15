using System;


namespace Hand2Note.ConsoleCalculator.Math.Functions
{
    public interface IFunction
    {
        string FunctionName { get; }
        Func<decimal, decimal> Function { get; }
    }
}
