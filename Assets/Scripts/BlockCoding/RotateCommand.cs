// Assets/Scripts/BlockCoding/RotateCommand.cs
using Cysharp.Threading.Tasks;
using Movement;

namespace BlockCoding
{
    public class RotateCommand : IBlockCommand
    {
        public CommandType Type { get; }
        private readonly MovementSystem _mv;
        private readonly float _angle;

        public RotateCommand(MovementSystem mv, float angle)
        {
            _mv = mv;
            _angle = angle;
            Type = angle < 0 ? CommandType.RotateLeft : CommandType.RotateRight;
        }

        public async UniTask ExecuteAsync()
        {
            await _mv.PerformRotateAsync(_angle);
            await UniTask.Delay(300);
        }
    }
}