using System.Collections;
using Movement;

namespace BlockCoding
{
    public class MoveCommand : BaseCommand
    {
        private readonly MovementSystem _mv;
        public override CommandType Type => CommandType.Move;

        public MoveCommand(MovementSystem mv) => _mv = mv;

        public override IEnumerator Execute() => _mv.PerformMove();
    }
}