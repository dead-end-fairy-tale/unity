using Cysharp.Threading.Tasks;

namespace BlockCoding
{
    public interface IBlockCommand
    {
        CommandType Type { get; }
        UniTask ExecuteAsync();
    }
}