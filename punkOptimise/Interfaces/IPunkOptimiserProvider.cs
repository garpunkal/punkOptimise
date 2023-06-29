using System.Threading;
using System.Threading.Tasks;

namespace punkOptimise.Interfaces
{
    public interface IPunkOptimiserProvider
    {
        string Name { get; }
        string Description { get; }
        bool IsValid(string[] extensions);
        Task<byte[]> Process(byte[] data, CancellationToken token);
    }
}