using System;

namespace XYZTrainer
{

    public class XYZException : Exception
    {
        public XYZException()
        {
        }

        public XYZException(string message)
            : base(message)
        {
        }

        public XYZException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
