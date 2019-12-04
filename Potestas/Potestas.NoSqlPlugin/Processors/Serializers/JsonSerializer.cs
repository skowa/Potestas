using System;
using System.IO;
using Newtonsoft.Json;
using Potestas.Processors;
using Potestas.Utils;

namespace Potestas.NoSqlPlugin.Processors.Serializers
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public void Serialize(Stream stream, T value)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (Validator.IsGenericTypeNull(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            using var writer = new StreamWriter(stream, leaveOpen:true);
            using var jsonWriter = new JsonTextWriter(writer);
            _jsonSerializer.Serialize(jsonWriter, value);
            jsonWriter.Flush();
        }

        public T Deserialize(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            return _jsonSerializer.Deserialize<T>(jsonReader);
        }

        public void Serialize(JsonWriter jsonWriter, T value)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException(nameof(jsonWriter));
            }

            if (Validator.IsGenericTypeNull(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _jsonSerializer.Serialize(jsonWriter, value);
            jsonWriter.Flush();
        }

        public T Deserialize(JsonReader jsonReader)
        {
            if (jsonReader == null)
            {
                throw new ArgumentException(nameof(jsonReader));
            }

            return _jsonSerializer.Deserialize<T>(jsonReader);
        }
    }
}