using Cysharp.Threading.Tasks;
using Combat;

namespace BlockCoding
{
    public class AttackCommand : IBlockCommand
    {
        public CommandType Type => CommandType.Attack;
        private readonly CombatSystem _combat;

        public AttackCommand(CombatSystem combat) => _combat = combat;

        public async UniTask ExecuteAsync()
        {
            _combat.PerformAttack();
            await _combat.PlayAttackEffectAsync();
            await UniTask.Delay(300);
        }
    }
}