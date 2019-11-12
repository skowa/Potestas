using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Potestas.Processors;
using Potestas.Utils;

namespace Potestas.Storages
{
    /* TASK. Implement file storage
     */
    public class FileStorage<T> : BaseFileStorage<T>, IEnergyObservationStorage<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : IEnergyObservation
    {
        private readonly ISerializer<T> _serializer;
        private readonly string _filePath;
        private int _lastCount;
        private long _lastPosition;

        public FileStorage(ISerializer<T> serializer, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _filePath = filePath;
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public string Description => "Stores energy observations in provided file";

        public int Count
        {
            get
            {
                if (!this.CheckIfFileWasUpdated())
                {
                    return _lastCount;
                }

                _lastCount = 0;
                foreach (var _ in this)
                {
                    ++_lastCount;
                }

                return _lastCount;
            }
        }

        public bool IsReadOnly { get; } = false;

        public void Add(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }

            using var stream = File.Open(_filePath, FileMode.Append);
            stream.Position = stream.Length;
            _serializer.Serialize(stream, item);
        }

        public void Clear()
        {
            using var fileStream = File.Open(_filePath, FileMode.Open);
            fileStream.SetLength(0);
        }

        public bool Contains(T item) => this.Contains(item, this);

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, this);

        public IEnumerator<T> GetEnumerator()
        {
            _lastCount = 0;
            using var stream = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Read);
            while (stream.Position < stream.Length)
            {
                ++_lastCount;
                yield return _serializer.Deserialize(stream);
            }

            _lastPosition = stream.Position;
        }

        public bool Remove(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }

            var outputName = Guid.NewGuid().ToString();
            var isDeleted = this.WriteObjectsToFileWithoutTheOneToBeDeleted(outputName, item);
            this.OverwriteFileOrKeepWithoutUpdates(isDeleted, outputName);
            
            return isDeleted;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private bool CheckIfFileWasUpdated()
        {
            using var stream = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Read);
            return _lastPosition != stream.Length - 1;
        }

        private bool WriteObjectsToFileWithoutTheOneToBeDeleted(string outputName, T item)
        {
            var isDeleted = false;
            using (var output = File.Open(outputName, FileMode.Create, FileAccess.Write))
            {
                foreach (var entity in this)
                {
                    if (entity.Equals(item))
                    {
                        isDeleted = true;
                        continue;
                    }

                    _serializer.Serialize(output, entity);
                }
            }

            return isDeleted;
        }

        private void OverwriteFileOrKeepWithoutUpdates(bool isDeleted, string outputName)
        {
            if (!isDeleted)
            {
                File.Delete(outputName);
            }
            else
            {
                File.Delete(_filePath);
                File.Move(outputName, _filePath);
            }
        }
    }
}
