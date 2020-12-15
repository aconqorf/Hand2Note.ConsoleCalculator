using System;


namespace Hand2Note.ConsoleCalculator.Math.Functions
{
    // ReSharper disable once UnusedType.Global
    public sealed class AbsFunction : IFunction
    {
        public string FunctionName => "abs";
        public Func<decimal, decimal> Function => System.Math.Abs;
    }
}
