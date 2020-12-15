using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Hand2Note.ConsoleCalculator.Math;


namespace Hand2Note.ConsoleCalculator
{
    internal static class Program
    {
        private static readonly string ApplicationVersion = GetFormattedVersion();
        private static readonly string ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static void Main()
        {
            Console.WriteLine($"Console Calculator v{ApplicationVersion}");
            Console.WriteLine();

            var mathParser = new MathExpressionEvaluator();

            Console.WriteLine($"Supported operators: {string.Join(", ", mathParser.Operators.Select(item => item.Operator))}");
            Console.WriteLine($"Supported functions: {string.Join(", ", mathParser.Functions.Select(item => item.FunctionName))}");
            Console.WriteLine();

            while (true)
            {
                Console.Write("> Type of expression to evaluating: ");

                try
                {
                    var rawExpression = Console.ReadLine();
                    var result = mathParser.Evaluate(rawExpression);

                    Console.WriteLine($"Result: {result}");
                }
                catch (MathEvaluatorException exception)
                {
                    Console.WriteLine($"Error: {exception.Message}");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Error: Oops... Result is too large.");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Unexpected error \"{exception.GetType().FullName}\": {exception.Message}");

                    ProcessUnhandledException(exception);
                    Console.WriteLine("File with error details was saved to program folder.");
                }

                Console.WriteLine();
            }
        }

        private static string GetFormattedVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            return version?.ToString(version.Revision == 0 ? 3 : 4);
        }

        private static void ProcessUnhandledException(Exception exception)
        {
            try
            {
                var dateNow = DateTime.Now;

                var folderName = Path.Combine(ApplicationPath, "Exceptions");
                var fileName = Path.Combine(folderName, $"{dateNow:dd-MM-yyyy}.txt");

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Time: {dateNow:dd-MM-yyyy HH:mm:ss}");
                stringBuilder.AppendLine($"Version: {ApplicationVersion}");

                stringBuilder.AppendLine();

                do
                {
                    stringBuilder.AppendLine($"Information of {exception.GetType()}:");
                    stringBuilder.AppendLine($" - Message: {exception.Message}");
                    stringBuilder.AppendLine($" - Source: {exception.Source}");
                    stringBuilder.AppendLine(" - StackTrace: ");

                    using (var stringReader = new StringReader(exception.StackTrace))
                    {
                        string rawString;
                        while ((rawString = stringReader.ReadLine()) != null) stringBuilder.AppendLine(rawString);
                    }

                    if ((exception = exception.InnerException) != null) stringBuilder.AppendLine();
                } while (exception != null);

                stringBuilder.AppendLine(new string('=', 10));
                stringBuilder.AppendLine();

                Directory.CreateDirectory(folderName);
                File.AppendAllText(fileName, stringBuilder.ToString());
            }
            catch
            {
                // ignored
            }
        }
    }
}
