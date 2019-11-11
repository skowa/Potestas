using System;
using System.IO;

namespace Potestas.Processors
{
    /* TASK. Implement Processor which saves IEnergyObservation to the provided file.
     * 1. Try to decorate SerializeProcessor.
     * QUESTIONS:
     * Which bonuses does decoration have?
     * TEST: Which kind of tests should be written for this class?
     */
    public class SaveToFileProcessor<T> : IDisposable, IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly SerializeProcessor<T> _serializeProcessor;
        private readonly FileStream _fileStream;
        private bool _isDisposed;

        public SaveToFileProcessor(SerializeProcessor<T> serializeProcessor, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _fileStream = new FileStream(filePath, FileMode.Append);
            _serializeProcessor = serializeProcessor ?? throw new ArgumentNullException(nameof(serializeProcessor));
        }

        public string Description => "The processor which saves IEnergyObservation to the provided file.";

        public void OnCompleted()
        {
            _serializeProcessor.OnCompleted();
            this.CompleteFile();
        }

        public void OnError(Exception error)
        {
            _serializeProcessor.OnError(error);
            this.CompleteFile();
        }

        public void OnNext(T value)
        {
            _serializeProcessor.OnNext(value);
            
            this.CopyToFileStream(_serializeProcessor.PreviousObjectPosition);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _serializeProcessor?.Dispose();
                _fileStream?.Dispose();
                _isDisposed = true;
            }
        }

        private void CompleteFile()
        {
            if (_serializeProcessor.LastObjectPosition != _serializeProcessor.Stream.Position)
            {
                this.CopyToFileStream(_serializeProcessor.LastObjectPosition);
            }
        }

        private void CopyToFileStream(long position)
        {
            _serializeProcessor.Stream.Seek(position, SeekOrigin.Begin);
            _serializeProcessor.Stream.CopyTo(_fileStream);
        }
    }
}
