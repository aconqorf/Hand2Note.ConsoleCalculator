using Xunit;


namespace Hand2Note.ConsoleCalculator.Math.Tests
{
    public sealed partial class MathExpressionEvaluatorTestsUs
    {
        [Fact]
        public void UnclosedParenthesisThrowsException()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("(1 + 2"));

            Assert.Equal("Invalid expression: unclosed parentheses.", exception.Message);
        }

        [Fact]
        public void EmptyExpressionThrowsException()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate(string.Empty));

            Assert.Equal("Invalid expression: no tokens found.", exception.Message);
        }

        [Fact]
        public void InvalidNumericValueThrowsException()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("abc"));

            Assert.Equal("Invalid numeric value received: \"abc\".", exception.Message);
        }

        [Fact]
        public void IncompleteExpressionThrowsException1()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("10 +"));

            Assert.Equal("Invalid expression: incomplete expression.", exception.Message);
        }

        [Fact]
        public void IncompleteExpressionThrowsException2()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("10 + 20 + "));

            Assert.Equal("Invalid expression: incomplete expression.", exception.Message);
        }

        [Fact]
        public void InvalidValueOnLeftSideThrowsException()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("abc + 10"));

            Assert.Equal("Invalid numeric value received of left side of expression: \"abc\".", exception.Message);
        }

        [Fact]
        public void InvalidValueOnRightSideThrowsException()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("10 + cba"));

            Assert.Equal("Invalid numeric value received of right side of expression: \"cba\".", exception.Message);
        }

        [Fact]
        public void WrongDecimalSeparatorThrowsException()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("10,25 + 10"));

            Assert.Equal("Invalid expression: may be used wrong decimal separator.", exception.Message);
        }

        [Fact]
        public void OperatorOnFirstPlaceThrowsException()
        {
            var exception = Assert.Throws<MathEvaluatorException>(() => MathParser.Evaluate("-1000-1000"));

            Assert.Equal("Invalid expression: operator takes first or last place.", exception.Message);
        }
    }
}
