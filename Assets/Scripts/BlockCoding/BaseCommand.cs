using Cysharp.Threading.Tasks;

namespace BlockCoding
{
    public abstract class BaseCommand : IBlockCommand
    {
        public abstract CommandType Type { get; }
        public abstract UniTask ExecuteAsync();
    }
}