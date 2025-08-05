using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace BlockCoding
{
    public class LoopCommand : IBlockCommand
    {
        public CommandType Type { get; }
        private readonly List<IBlockCommand> inner;
        private readonly int count;

        public LoopCommand(List<IBlockCommand> inner, int count)
        {
            this.inner = inner;
            this.count = count;
            Type = CommandType.Loop;
        }

        public async UniTask ExecuteAsync()
        {
            for (int i = 0; i < count; i++)
                foreach (var cmd in inner)
                    await cmd.ExecuteAsync();
        }
    }
}