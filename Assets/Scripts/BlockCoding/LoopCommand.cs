using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace BlockCoding
{
    public class LoopCommand : IBlockCommand
    {
        public CommandType Type { get; }
        private readonly List<IBlockCommand> _innerCommands;
        private readonly int _count;

        public LoopCommand(List<IBlockCommand> innerCommands, int count)
        {
            _innerCommands = innerCommands;
            _count = count;
            Type = CommandType.Loop;
        }

        public async UniTask ExecuteAsync()
        {
            for (int i = 0; i < _count; i++)
            {
                foreach (var cmd in _innerCommands)
                {
                    await cmd.ExecuteAsync();
                }
            }
        }
    }
}