using System;
using System.Runtime.Serialization;

namespace Potestas.Exceptions
{
    [Serializable]
    public class NotSerializableTypeException : Exception
    {
        public NotSerializableTypeException(Type type) : base()
        {
            NotSerializableType = type;
        }

        public NotSerializableTypeException(Type type, string message) : base(message)
        {
            NotSerializableType = type;
        }

        public NotSerializableTypeException(Type type, string message, Exception innerException) : base(message, innerException)
        {
            NotSerializableType = type;
        }

        protected NotSerializableTypeException(Type type, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            NotSerializableType = type;
        }

        public Type NotSerializableType { get; }
    }
}