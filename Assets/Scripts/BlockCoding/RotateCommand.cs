using Cysharp.Threading.Tasks;
using Movement;

namespace BlockCoding
{
    public class RotateCommand : IBlockCommand
    {
        public CommandType Type { get; }
        private readonly MovementSystem movementSystem;
        private readonly float angle;

        public RotateCommand(MovementSystem movementSystem, float angle)
        {
            this.movementSystem = movementSystem;
            this.angle          = angle;
            Type = angle < 0 ? CommandType.RotateLeft : CommandType.RotateRight;
        }

        public async UniTask ExecuteAsync()
        {
            await movementSystem.PerformRotateAsync(angle);
        }
    }
}