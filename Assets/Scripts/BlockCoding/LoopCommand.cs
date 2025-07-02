using System.Collections;
using System.Collections.Generic;

namespace BlockCoding
{
    public class LoopCommand : BaseCommand
    {
        private readonly List<IBlockCommand> _innerCommands;
        private readonly int _count;

        public LoopCommand(List<IBlockCommand> innerCommands, int count)
        {
            _innerCommands = innerCommands;
            _count = count;
        }

        public override CommandType Type => CommandType.Loop;

        public override IEnumerator Execute()
        {
            for (int i = 0; i < _count; i++)
            {
                foreach (var cmd in _innerCommands)
                {
                    yield return cmd.Execute();
                }
            }
        }
    }
}