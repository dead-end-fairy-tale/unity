using System.Collections;
using UnityEngine;

namespace BlockCoding
{
    public class AttackCommand : BaseCommand
    {
        private readonly Combat.CombatSystem _combat;
        public override CommandType Type => CommandType.Attack;

        public AttackCommand(Combat.CombatSystem combat)
        {
            _combat = combat;
        }

        public override IEnumerator Execute()
        {
            _combat.PerformAttack();
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}