using Cysharp.Threading.Tasks;
using Movement;

namespace BlockCoding
{
    public class MoveCommand : IBlockCommand
    {
        public CommandType Type => CommandType.Move;
        private readonly MovementSystem movementSystem;

        public MoveCommand(MovementSystem movementSystem)
        {
            this.movementSystem = movementSystem;
        }

        public async UniTask ExecuteAsync()
        {
            await movementSystem.PerformMoveAsync();
        }
    }
}