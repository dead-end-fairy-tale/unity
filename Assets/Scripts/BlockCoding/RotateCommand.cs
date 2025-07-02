using System.Collections;
using Movement;

namespace BlockCoding
{
    public class RotateCommand : BaseCommand
    {
        private readonly MovementSystem _mv;
        private readonly float _angle;
        public override CommandType Type =>
            _angle < 0 ? CommandType.RotateLeft : CommandType.RotateRight;

        public RotateCommand(MovementSystem mv, float angle)
        {
            _mv    = mv;
            _angle = angle;
        }

        public override IEnumerator Execute() => _mv.PerformRotate(_angle);
    }
}