using Cysharp.Threading.Tasks;
using Movement;

namespace BlockCoding
{
    public class MoveCommand : IBlockCommand
    {
        public CommandType Type => CommandType.Move;
        private readonly MovementSystem _mv;

        public MoveCommand(MovementSystem mv) => _mv = mv;

        public async UniTask ExecuteAsync()
        {
            await _mv.PerformMoveAsync();
            await UniTask.Delay(300);
        }
    }
}