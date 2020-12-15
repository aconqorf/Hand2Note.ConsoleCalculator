using System;


namespace Hand2Note.ConsoleCalculator.Math.Operators
{
    // ReSharper disable once UnusedType.Global
    public sealed class MultiplicationOperation : IOperator
    {
        public OperatorPriority Priority => OperatorPriority.High;

        public string Operator => "*";
        public Func<decimal, decimal, decimal> Function => (a, b) => a * b;
    }
}
