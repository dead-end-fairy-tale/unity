using System.Collections;

namespace BlockCoding
{
    public abstract class BaseCommand : IBlockCommand
    {
        public abstract CommandType Type { get; }
        public abstract IEnumerator Execute();
    }
}