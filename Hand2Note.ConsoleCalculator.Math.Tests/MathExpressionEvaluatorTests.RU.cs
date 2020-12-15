using System.Collections.Generic;
using System.Globalization;

using Xunit;


namespace Hand2Note.ConsoleCalculator.Math.Tests
{
    public sealed class MathExpressionEvaluatorTestsRu : MathExpressionEvaluatorTestsBase
    {
        public static List<object[]> TestDataEdgeCase => new List<object[]>
        {
            new object[] { "1/2/3", 0.1666666666666666666666666667m }, new object[] { "1 / 2 / 3", 0.1666666666666666666666666667m }
        };

        public MathExpressionEvaluatorTestsRu() : base(new CultureInfo("ru-RU")) { }

        [Theory]
        [InlineData("1 + 39", 40)]
        [InlineData("0 + 1", 1)]
        [InlineData("0,125 + 99,875", 100)]
        [InlineData("555,55 + 55,555", 611.105)]
        [InlineData("0 + 0", 0)]
        public void TestAdditionOperation(string expression, decimal expected) => TestExpression(expression, expected);

        [Theory]
        [InlineData("1 / 5", 0.2)]
        [InlineData("100 / 5", 20)]
        [InlineData("100 / 0,2", 500)]
        [InlineData("500 / 1", 500)]
        [InlineData("0,2 / 0,2", 1)]
        public void TestDivisionOperation(string expression, decimal expected) => TestExpression(expression, expected);

        [Theory]
        [InlineData("100 * 0,2", 20)]
        [InlineData("100 * 100", 10000)]
        [InlineData("0,2 * 0,2", 0.04)]
        [InlineData("100 * 0", 0)]
        public void TestMultiplicationOperation(string expression, decimal expected) => TestExpression(expression, expected);

        [Theory]
        [InlineData("1 - 0", 1)]
        [InlineData("100 - 200", -100)]
        [InlineData("200 - 100", 100)]
        [InlineData("100,25 - 0,25", 100)]
        [InlineData("100 - 0,00001", 99.99999)]
        public void TestSubtractionOperation(string expression, decimal expected) => TestExpression(expression, expected);

        [Theory]
        [InlineData("10 + 10 * 2 / 4", 15)]
        [InlineData("(1 + 2) + (3 + 4)", 10)]
        [InlineData("(1 - 20) * (3 - 40)", 703)]
        [InlineData("(1 + 20) * (3 - 40)", -777)]
        [InlineData("(1 + 20) * (3 - 40) / (10 - 5)", -155.4)]
        public void TestComplexExpression(string expression, decimal expected) => TestExpression(expression, expected);

        [Theory]
        [InlineData("abs(10 - 20)", 10)]
        [InlineData("abs((1 + 20) * (3 - 40))", 777)]
        [InlineData("abs((1 + 20) * (3 - 40) / (10 - 5))", 155.4)]
        public void TestAbsFunction(string expression, decimal expected) => TestExpression(expression, expected);

        [Theory]
        [InlineData("1", 1)]
        [InlineData("((1 + 1))", 2)]
        [InlineData("((1+1))", 2)]
        [InlineData("(1) + 1", 2)]
        [InlineData("(1)+1", 2)]
        [MemberData(nameof(TestDataEdgeCase))] // decimal isn't a valid attribute parameter type
        public void TestEdgeCases(string expression, decimal expected) => TestExpression(expression, expected);
    }
}
