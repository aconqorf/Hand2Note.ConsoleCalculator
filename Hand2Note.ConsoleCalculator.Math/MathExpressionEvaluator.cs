using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Hand2Note.ConsoleCalculator.Math.Functions;
using Hand2Note.ConsoleCalculator.Math.Operators;
using Hand2Note.ConsoleCalculator.Math.Utilities;


namespace Hand2Note.ConsoleCalculator.Math
{
    public sealed class MathExpressionEvaluator
    {
        private readonly Dictionary<string, IFunction> _knownFunctions;
        private readonly Dictionary<string, IOperator> _knownOperators;

        public IEnumerable<IFunction> Functions => _knownFunctions.Values;
        public IEnumerable<IOperator> Operators => _knownOperators.Values;

        public CultureInfo CultureInfo { get; }

        public MathExpressionEvaluator(CultureInfo cultureInfo = null)
        {
            _knownOperators = InitializeOperators().ToDictionary(item => item.Operator, item => item.Value);
            _knownFunctions = InitializeFunctions().ToDictionary(item => item.FunctionName, item => item.Value);

            CultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;
        }

        private static IEnumerable<(string Operator, IOperator Value)> InitializeOperators()
        {
            var operators = ReflectionUtility.GetInstancesOfInterface<IOperator>();

            foreach (var @operator in operators) yield return (@operator.Operator, @operator);
        }

        private static IEnumerable<(string FunctionName, IFunction Value)> InitializeFunctions()
        {
            var functions = ReflectionUtility.GetInstancesOfInterface<IFunction>();

            foreach (var function in functions) yield return (function.FunctionName, function);
        }

        public decimal Evaluate(string mathExpression)
        {
            var tokens = GetTokens(mathExpression).ToList();

            while (tokens.Contains("("))
            {
                var openParenthesis = tokens.LastIndexOf("(");
                var closeParenthesis = tokens.IndexOf(")", openParenthesis);

                if (openParenthesis >= closeParenthesis) throw new MathEvaluatorException("Invalid expression: unclosed parentheses.");

                var functionName = tokens[openParenthesis == 0 ? 0 : openParenthesis - 1];
                var rawExpression = tokens.Skip(openParenthesis + 1).Take(closeParenthesis - openParenthesis - 1).ToList();

                var value = CalculateTokens(rawExpression);
                var intermediateResult = _knownFunctions.Keys.Contains(functionName) ? _knownFunctions[functionName].Function(value) : value;

                tokens.RemoveRange(openParenthesis, closeParenthesis - openParenthesis + 1);
                tokens.Insert(openParenthesis, intermediateResult.ToString(CultureInfo));

                if (_knownFunctions.Keys.Contains(functionName)) tokens.RemoveAt(openParenthesis - 1);
            }

            return CalculateTokens(tokens);
        }

        private IEnumerable<string> GetTokens(string expression)
        {
            for (var i = 0; i < expression.Length; i++)
            {
                var currentChar = expression[i];
                if (char.IsWhiteSpace(currentChar)) continue;

                if (char.IsLetter(currentChar) || char.IsDigit(currentChar))
                {
                    var token = currentChar.ToString();

                    while (i + 1 < expression.Length)
                    {
                        var breakLoop = true;

                        var nextChar = expression[i + 1];
                        var decimalSeparator = CultureInfo.NumberFormat.NumberDecimalSeparator;

                        if (char.IsLetterOrDigit(nextChar)) breakLoop = false;
                        else
                        {
                            if (nextChar.ToString() == decimalSeparator) breakLoop = false;
                            else if (decimalSeparator == "," && nextChar == '.' || decimalSeparator == "." && nextChar == ',')
                            {
                                throw new MathEvaluatorException("Invalid expression: may be used wrong decimal separator.");
                            }
                        }

                        if (breakLoop) break;

                        token += expression[++i];
                    }

                    yield return token;

                    continue;
                }

                yield return $"{currentChar}";
            }
        }

        private decimal CalculateTokens(List<string> tokens)
        {
            if (tokens.Count == 0)
            {
                throw new MathEvaluatorException("Invalid expression: no tokens found.");
            }

            if (tokens.Count == 1)
            {
                if (decimal.TryParse(tokens[0], NumberStyles.Number, CultureInfo, out var result)) return result;

                throw new MathEvaluatorException($"Invalid numeric value received: \"{tokens[0]}\".");
            }

            if (tokens.Count == 2)
            {
                throw new MathEvaluatorException("Invalid expression: incomplete expression.");
            }

            do
            {
                var priorityOperation = tokens.Select((item, index) => new { Token = item, Index = index })
                                              .Where(item => _knownOperators.ContainsKey(item.Token))
                                              .Select(item => new { Operator = _knownOperators[item.Token], item.Index })
                                              .OrderBy(item => item.Operator.Priority)
                                              .FirstOrDefault();

                if (priorityOperation == null) throw new MathEvaluatorException("Invalid expression: incomplete expression.");
                if (priorityOperation.Index == 0 || priorityOperation.Index + 1 == tokens.Count) throw new MathEvaluatorException("Invalid expression: operator takes first or last place.");

                if (!decimal.TryParse(tokens[priorityOperation.Index - 1], NumberStyles.Number, CultureInfo, out var leftSide))
                {
                    throw new MathEvaluatorException($"Invalid numeric value received of left side of expression: \"{tokens[priorityOperation.Index - 1]}\".");
                }

                if (!decimal.TryParse(tokens[priorityOperation.Index + 1], NumberStyles.Number, CultureInfo, out var rightSide))
                {
                    throw new MathEvaluatorException($"Invalid numeric value received of right side of expression: \"{tokens[priorityOperation.Index + 1]}\".");
                }

                var result = priorityOperation.Operator.Function(leftSide, rightSide);

                if (tokens.Count == 3) return result;

                tokens.RemoveRange(priorityOperation.Index - 1, 3);
                tokens.Insert(priorityOperation.Index - 1, result.ToString(CultureInfo));
            } while (tokens.Count >= 3);

            throw new MathEvaluatorException("Invalid expression: incomplete expression.");
        }
    }
}
