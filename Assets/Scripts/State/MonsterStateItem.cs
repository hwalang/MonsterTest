using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterStateItem
{
    public class IDLE : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.PrintText($"대기중");
        }

        public override void Execute(Monster entity)
        {
            // 자동 회복
            if (entity.Hp < entity.MaxHp)
            {
                entity.Hp += 10;
            }
            else if (entity.Hp > entity.MaxHp)
            {
                entity.Hp = entity.MaxHp;
            }

            entity.PrintText("자동 회복 중...");
        }

        public override void Exit(Monster entity)
        {
            entity.PrintText($"행동 시작");
        }
    }
    public class CHASE : State<Monster>
    {
        private Transform _target;

        public override void Enter(Monster entity)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;

            entity.PrintText("인식 범위 내에 들어온 플레이어 확인");
        }

        public override void Execute(Monster entity)
        {
            entity.PrintText("플레이어를 추격 중...");

            float thisToTargetDist = Vector3.Distance(
                GameObject.FindGameObjectWithTag("Player").transform.position,
                entity.transform.position);

            //entity.PrintText(thisToTargetDist.ToString());
            //entity.PrintText("거리차이: " + Mathf.Abs(thisToTargetDist - entity.AttackRange).ToString());

            // 플레이어 바라보기

            // 플레이어한테 이동하기
        }

        public override void Exit(Monster entity)
        {
            // 목표로 하는 플레이어 사망, 인식 범위내에 플레이어가 없음, 공격 사거리 내에 플레이어가 존재
            entity.PrintText("추격 종료");
        }
    }
    public class ATTACK : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.PrintText("공격 범위 내에 들어온 플레이어 확인");
        }

        public override void Execute(Monster entity)
        {
            entity.PrintText("플레이어 공격");

            // 플레이어 바라보기
        }

        public override void Exit(Monster entity)
        {
            // 플레이어 사망, 사거리 내에 플레이어가 없음
            entity.PrintText("공격 종료");
        }
    }

    // Global State에 의해 DIE 상태가 호출된다.
    public class DIE : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.PrintText("사망");
        }

        public override void Execute(Monster entity)
        {
            // 플레이어 부활 조건 확인
            // 1. 스테이지가 바뀐 경우
            // entity.RevertToPreviousState();
        }

        public override void Exit(Monster entity)
        {
            entity.PrintText("부활");
        }
    }

    public class StateGlobal : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            // 비워둔다.
        }

        // stateMachine에서 매 프레임마다 수행하면서 조건에 부합한지 확인한다.
        public override void Execute(Monster entity)
        {
            // 만약 현재 상태가 Global State를 통해 수행되는 상태라면 종료한다.
            if (entity.curState == MonsterState.DIE) return;

            if (entity.Hp <= 0)
            {
                entity.Hp = 0;
                entity.ChangeState(MonsterState.DIE);
            }
            // 여기에 확률적으로 사용하는 패턴( 상태 )을 넣어도 된다.
            int patternState = Random.Range(0, 100);
            if (patternState < 10) return;              // 10% 확률
        }

        public override void Exit(Monster entity)
        {
            // 비워둔다.
        }
    }
}

