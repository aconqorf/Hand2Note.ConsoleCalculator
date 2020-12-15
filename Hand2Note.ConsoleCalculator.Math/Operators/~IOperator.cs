using System;


namespace Hand2Note.ConsoleCalculator.Math.Operators
{
    public interface IOperator
    {
        OperatorPriority Priority { get; }

        string Operator { get; }
        Func<decimal, decimal, decimal> Function { get; }
    }
}
