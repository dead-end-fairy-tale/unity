using Cysharp.Threading.Tasks;

namespace BlockCoding
{
    public class LoopStartCommand : IBlockCommand
    {
        public CommandType Type => CommandType.LoopStart;
        public UniTask ExecuteAsync() => UniTask.CompletedTask;
    }
}