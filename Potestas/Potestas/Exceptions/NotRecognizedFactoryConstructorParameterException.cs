using System;
using System.Runtime.Serialization;

namespace Potestas.Exceptions
{
    public class NotRecognizedFactoryConstructorParameterException : Exception
    {
        public NotRecognizedFactoryConstructorParameterException(Type factoryType, Type notRecognizedParameterType) :base()
        {
            FactoryType = factoryType;
            NotRecognizedParameterType = notRecognizedParameterType;
        }

        public NotRecognizedFactoryConstructorParameterException(Type factoryType, Type notRecognizedParameterType, string message) : base(message)
        {
            FactoryType = factoryType;
            NotRecognizedParameterType = notRecognizedParameterType;
        }

        public NotRecognizedFactoryConstructorParameterException(Type factoryType, Type notRecognizedParameterType, string message, Exception innerException) : base(message, innerException)
        {
            FactoryType = factoryType;
            NotRecognizedParameterType = notRecognizedParameterType;
        }

        protected NotRecognizedFactoryConstructorParameterException(Type factoryType, Type notRecognizedParameterType, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            FactoryType = factoryType;
            NotRecognizedParameterType = notRecognizedParameterType;
        }

        public Type FactoryType { get; set; }

        public Type NotRecognizedParameterType { get; set; }
    }
}