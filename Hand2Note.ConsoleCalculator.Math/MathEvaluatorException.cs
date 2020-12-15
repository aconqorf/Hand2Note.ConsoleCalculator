using System;
using System.Runtime.Serialization;


namespace Hand2Note.ConsoleCalculator.Math
{
    [Serializable]
    public class MathEvaluatorException : Exception
    {
        public MathEvaluatorException() { }
        public MathEvaluatorException(string message) : base(message) { }
        public MathEvaluatorException(string message, Exception inner) : base(message, inner) { }
        protected MathEvaluatorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
