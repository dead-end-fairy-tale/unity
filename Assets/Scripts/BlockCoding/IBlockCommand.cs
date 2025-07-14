using System.Collections;

namespace BlockCoding
{
    public interface IBlockCommand
    {
        CommandType Type { get; }
        IEnumerator Execute();
    }
}