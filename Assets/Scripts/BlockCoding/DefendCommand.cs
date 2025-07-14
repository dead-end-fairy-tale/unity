using System.Collections;
using UnityEngine;

namespace BlockCoding
{
    public class DefendCommand : BaseCommand
    {
        private readonly Combat.CombatSystem _combat;
        public override CommandType Type => CommandType.Defend;

        public DefendCommand(Combat.CombatSystem combat)
        {
            _combat = combat;
        }

        public override IEnumerator Execute()
        {
            _combat.PerformDefend();
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}