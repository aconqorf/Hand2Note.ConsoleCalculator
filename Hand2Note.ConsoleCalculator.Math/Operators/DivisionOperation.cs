using System;


namespace Hand2Note.ConsoleCalculator.Math.Operators
{
    // ReSharper disable once UnusedType.Global
    public sealed class DivisionOperation : IOperator
    {
        public OperatorPriority Priority => OperatorPriority.High;

        public string Operator => "/";
        public Func<decimal, decimal, decimal> Function => (a, b) =>
        {
            if (b != 0) return a / b;

            throw new MathEvaluatorException($"Invalid expression: division {a} by zero.");
        };
    }
}
