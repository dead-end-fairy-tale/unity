using System.Collections.Generic;
using UnityEngine;
using BlockCoding;

namespace AI
{
    [RequireComponent(typeof(AIController))]
    public class DecisionMaker : MonoBehaviour
    {
        [Header("References (Inspector에 할당)")]
        [Tooltip("8×8 격자 및 좌표 변환을 담당하는 GridManager")]
        public GridManager gridManager;

        [Header("AI 설정")]
        [Tooltip("플레이어와 떨어져 있을 때 공격하려는 최대 그리드 거리")]
        public int attackDistance = 1;

        [Tooltip("한 턴에 사용할 최대 커맨드 수")]
        public int maxCommands = 3;

        [Tooltip("회전할 때 사용하는 각도(90°)")]
        public float rotationAngle = 90f;

        public List<IBlockCommand> DecideCommands(
            BlockCodingController aiCtrl,
            BlockCodingController playerCtrl)
        {
            var commands = new List<IBlockCommand>();
            var mv       = aiCtrl.movementSystem;
            var cb       = aiCtrl.combatSystem;

            Vector2Int simPos    = gridManager.WorldToCoord(mv.transform.position);
            Vector2Int targetPos = gridManager.WorldToCoord(
                                        playerCtrl.movementSystem.transform.position
                                    );

            Vector3 fwd = mv.transform.forward;
            int simDir = 0;
            if (Vector3.Dot(fwd, Vector3.right) > 0.9f) simDir = 1;
            else if (Vector3.Dot(fwd, Vector3.back)  > 0.9f) simDir = 2;
            else if (Vector3.Dot(fwd, Vector3.left)  > 0.9f) simDir = 3;

            for (int i = 0; i < maxCommands; i++)
            {
                CommandType nextType;

                int dist = Mathf.Abs(simPos.x - targetPos.x)
                         + Mathf.Abs(simPos.y - targetPos.y);

                Vector2Int forwardOffset = simDir switch
                {
                    0 => Vector2Int.up,
                    1 => Vector2Int.right,
                    2 => Vector2Int.down,
                    3 => Vector2Int.left,
                    _ => Vector2Int.zero
                };

                if (dist <= attackDistance && targetPos == simPos + forwardOffset)
                {
                    nextType = CommandType.Attack;
                }
                else
                {
                    Vector2Int delta = targetPos - simPos;
                    Vector2Int desiredMove =
                        new Vector2Int(
                            delta.x != 0 ? (delta.x > 0 ? 1 : -1) : 0,
                            delta.x == 0 && delta.y != 0 ? (delta.y > 0 ? 1 : -1) : 0
                        );

                    int desiredDir = 0;
                    if (desiredMove == Vector2Int.up)    desiredDir = 0;
                    if (desiredMove == Vector2Int.right) desiredDir = 1;
                    if (desiredMove == Vector2Int.down)  desiredDir = 2;
                    if (desiredMove == Vector2Int.left)  desiredDir = 3;

                    if (simDir != desiredDir)
                    {
                        int diff = (desiredDir - simDir + 4) % 4;
                        if (diff == 1 || diff == 3)
                        {
                            nextType = diff == 1
                                ? CommandType.RotateRight
                                : CommandType.RotateLeft;
                            simDir = (simDir + (diff == 1 ? 1 : -1) + 4) % 4;
                        }
                        else
                        {
                            nextType = CommandType.RotateRight;
                            simDir = (simDir + 1) % 4;
                        }
                    }
                    else
                    {
                        nextType = CommandType.Move;
                        simPos += desiredMove;
                    }
                }

                IBlockCommand cmd = nextType switch
                {
                    CommandType.Attack      => new AttackCommand(cb),
                    CommandType.Defend      => new DefendCommand(cb),
                    CommandType.Move        => new MoveCommand(mv),
                    CommandType.RotateLeft  => new RotateCommand(mv, -rotationAngle),
                    CommandType.RotateRight => new RotateCommand(mv,  rotationAngle),
                    _                       => null
                };
                if (cmd != null) commands.Add(cmd);
            }

            return commands;
        }
    }
}
