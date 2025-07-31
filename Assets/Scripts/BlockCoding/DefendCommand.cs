using Cysharp.Threading.Tasks;
using Combat;

namespace BlockCoding
{
    public class DefendCommand : IBlockCommand
    {
        public CommandType Type => CommandType.Defend;
        private readonly CombatSystem _combat;

        public DefendCommand(CombatSystem combat) => _combat = combat;

        public async UniTask ExecuteAsync()
        {
            _combat.PerformDefend();
            await UniTask.Delay(300);
        }
    }
}