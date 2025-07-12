using System.Collections;

namespace BlockCoding
{
    public class LoopStartCommand : BaseCommand
    {
        public override CommandType Type => CommandType.LoopStart;
        public override IEnumerator Execute() => System.Linq.Enumerable.Empty<IEnumerator>().GetEnumerator().Current;
    }
}