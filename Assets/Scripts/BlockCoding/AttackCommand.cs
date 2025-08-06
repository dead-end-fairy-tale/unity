using Cysharp.Threading.Tasks;
using Combat;

namespace BlockCoding
{
    public class AttackCommand : IBlockCommand
    {
        public CommandType Type => CommandType.Attack;
        private readonly CombatSystem combatSystem;

        public AttackCommand(CombatSystem combat)
        {
            combatSystem = combat;
        }

        public async UniTask ExecuteAsync()
        {
            combatSystem.PerformAttack();
            await combatSystem.PlayAttackEffectAsync();
        }
    }
}