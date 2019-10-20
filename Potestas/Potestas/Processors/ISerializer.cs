using System.IO;

namespace Potestas.Processors
{
    public interface ISerializer<T>
    {
        void Serialize(Stream stream, T value);
        T Deserialize(Stream stream);
    }
}