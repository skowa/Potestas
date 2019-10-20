using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Potestas.Exceptions;

namespace Potestas.Processors
{
    public class BinarySerializer<T> : ISerializer<T>
    {
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        public void Serialize(Stream stream, T value)
        {
            this.CheckStreamForNull(stream);

            if (!typeof(T).IsValueType && value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!typeof(T).IsSerializable)
            {
                throw new NotSerializableTypeException(typeof(T), "The type should be marked as serializable to be binary serialized");
            }

            _binaryFormatter.Serialize(stream, value);
        }

        public T Deserialize(Stream stream)
        {
           this.CheckStreamForNull(stream);

           var value = (T)_binaryFormatter.Deserialize(stream);

           return value;
        }

        private void CheckStreamForNull(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
        }
    }
}