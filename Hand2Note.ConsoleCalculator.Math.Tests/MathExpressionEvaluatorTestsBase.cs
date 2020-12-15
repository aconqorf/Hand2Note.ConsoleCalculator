using System.Globalization;

using Xunit;


namespace Hand2Note.ConsoleCalculator.Math.Tests
{
    public abstract class MathExpressionEvaluatorTestsBase
    {
        protected MathExpressionEvaluator MathParser { get; }

        public MathExpressionEvaluatorTestsBase(CultureInfo cultureInfo)
        {
            MathParser = new MathExpressionEvaluator(cultureInfo);
        }

        protected void TestExpression(string expression, decimal expected)
        {
            var result = MathParser.Evaluate(expression);

            Assert.Equal(result, expected);
        }
    }
}
