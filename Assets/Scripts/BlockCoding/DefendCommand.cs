using Cysharp.Threading.Tasks;
using Combat;

namespace BlockCoding
{
    public class DefendCommand : IBlockCommand
    {
        public CommandType Type => CommandType.Defend;
        private readonly CombatSystem combatSystem;

        public DefendCommand(CombatSystem combat)
        {
            combatSystem = combat;
        }

        public async UniTask ExecuteAsync()
        {
            combatSystem.PerformDefend();
            await UniTask.Delay(200, DelayType.DeltaTime);
        }
    }
}