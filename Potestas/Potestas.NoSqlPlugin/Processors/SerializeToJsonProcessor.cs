using System;
using System.IO;
using Newtonsoft.Json;
using Potestas.NoSqlPlugin.Processors.Serializers;
using Potestas.Processors;

namespace Potestas.NoSqlPlugin.Processors
{
    public class SerializeToJsonProcessor<T> : SerializeProcessor<T>, IDisposable, IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly JsonWriter _jsonWriter;
        
        public SerializeToJsonProcessor() : base(new JsonSerializer<T>())
        {
            using var writer = new StreamWriter(Stream, leaveOpen: true);
            _jsonWriter = new JsonTextWriter(writer);
        }

        public override void OnCompleted()
        {
            this.EndDocument();

            base.OnCompleted();
        }

        public override void OnError(Exception error)
        {
            this.EndDocument();

            base.OnError(error);
        }

        protected override void SerializeToStream(T value)
        {
            if (_jsonWriter.WriteState == WriteState.Start)
            {
                _jsonWriter.WriteStartArray();
            }

            (Serializer as JsonSerializer<T>)?.Serialize(_jsonWriter, value);
        }

        private void EndDocument()
        {
            _jsonWriter.WriteEndArray();
            _jsonWriter.Flush();
        }
    }
}