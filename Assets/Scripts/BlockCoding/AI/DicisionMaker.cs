// Assets/Scripts/AI/DecisionMaker.cs
using System.Collections.Generic;
using UnityEngine;
using BlockCoding;
using Movement;

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

        /// <summary>
        /// AI가 한 턴에 사용할 커맨드를 시뮬레이션하면서 생성합니다.
        /// </summary>
        public List<IBlockCommand> DecideCommands(
            BlockCodingController aiCtrl,
            BlockCodingController playerCtrl)
        {
            var commands = new List<IBlockCommand>();
            var mv       = aiCtrl.movementSystem;
            var cb       = aiCtrl.combatSystem;

            // 1) 시작 위치·목표 위치 (격자 좌표)
            Vector2Int simPos    = gridManager.WorldToCoord(mv.transform.position);
            Vector2Int targetPos = gridManager.WorldToCoord(
                                        playerCtrl.movementSystem.transform.position
                                    );

            // 2) 시작 방향을 0:up,1:right,2:down,3:left 로 인덱스화
            Vector3 fwd = mv.transform.forward;
            int simDir = 0;
            if (Vector3.Dot(fwd, Vector3.right) > 0.9f) simDir = 1;
            else if (Vector3.Dot(fwd, Vector3.back) > 0.9f) simDir = 2;
            else if (Vector3.Dot(fwd, Vector3.left) > 0.9f) simDir = 3;

            // 3) 시뮬레이션 루프
            for (int i = 0; i < maxCommands; i++)
            {
                CommandType nextType;

                int dist = Mathf.Abs(simPos.x - targetPos.x)
                         + Mathf.Abs(simPos.y - targetPos.y);

                // 3-1) 공격 사거리 안이면 무조건 Attack
                if (dist <= attackDistance)
                {
                    nextType = CommandType.Attack;
                }
                else
                {
                    // 3-2) 목표 셀까지 바로 한 칸만 이동하는 경로: 맨해튼 기준
                    Vector2Int delta = targetPos - simPos;
                    // 우선 X축부터
                    Vector2Int desiredMove = 
                        new Vector2Int(
                            delta.x != 0 ? (delta.x > 0 ? 1 : -1) : 0,
                            delta.x == 0 && delta.y != 0 ? (delta.y > 0 ? 1 : -1) : 0
                        );

                    // 원하는 방향 인덱스 계산
                    int desiredDir = 0;
                    if (desiredMove == Vector2Int.up)    desiredDir = 0;
                    if (desiredMove == Vector2Int.right) desiredDir = 1;
                    if (desiredMove == Vector2Int.down)  desiredDir = 2;
                    if (desiredMove == Vector2Int.left)  desiredDir = 3;

                    // 3-3) 시뮬레이션 방향(simDir)과 desiredDir 비교
                    if (simDir != desiredDir)
                    {
                        // 차이를 최소 회전으로 보정: 왼쪽 회전(-1) or 오른쪽 회전(+1)
                        int diff = (desiredDir - simDir + 4) % 4;
                        if (diff == 1 || diff == 3)
                        {
                            // diff==1: right, diff==3: -1 mod4 left
                            nextType = diff == 1 
                                ? CommandType.RotateRight 
                                : CommandType.RotateLeft;
                            // 시뮬레이션 방향 업데이트
                            simDir = (simDir + (diff == 1 ? 1 : -1) + 4) % 4;
                        }
                        else
                        {
                            // 정 반대(2칸 차)일 땐 오른쪽 회전 택하기
                            nextType = CommandType.RotateRight;
                            simDir = (simDir + 1) % 4;
                        }
                    }
                    else
                    {
                        // 3-4) 방향이 이미 맞춰져 있으면 앞으로 한 칸 이동
                        nextType = CommandType.Move;
                        simPos += desiredMove;
                    }
                }

                // 4) IBlockCommand 인스턴스화
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
