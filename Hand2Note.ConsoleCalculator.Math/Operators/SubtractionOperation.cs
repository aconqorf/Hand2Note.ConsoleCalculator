using System;


namespace Hand2Note.ConsoleCalculator.Math.Operators
{
    // ReSharper disable once UnusedType.Global
    public sealed class SubtractionOperation : IOperator
    {
        public OperatorPriority Priority => OperatorPriority.Low;

        public string Operator => "-";
        public Func<decimal, decimal, decimal> Function => (a, b) => a - b;
    }
}
