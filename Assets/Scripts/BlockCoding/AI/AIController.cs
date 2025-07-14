using System.Collections.Generic;
using UnityEngine;
using BlockCoding;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [Header("References")]
        public BlockCodingController aiBlocks;
        public BlockCodingController playerController;
        public DecisionMaker decisionMaker;

        public List<IBlockCommand> DecideCommands()
            => decisionMaker.DecideCommands(aiBlocks, playerController);
    }
}