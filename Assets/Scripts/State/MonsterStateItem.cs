using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterStateItem
{
    public class IDLE : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.PrintText($"�����");
        }

        public override void Execute(Monster entity)
        {
            // �ڵ� ȸ��
            if (entity.Hp < entity.MaxHp)
            {
                entity.Hp += 10;
            }
            else if (entity.Hp > entity.MaxHp)
            {
                entity.Hp = entity.MaxHp;
            }

            entity.PrintText("�ڵ� ȸ�� ��...");
        }

        public override void Exit(Monster entity)
        {
            entity.PrintText($"�ൿ ����");
        }
    }
    public class CHASE : State<Monster>
    {
        private Transform _target;

        public override void Enter(Monster entity)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;

            entity.PrintText("�ν� ���� ���� ���� �÷��̾� Ȯ��");
        }

        public override void Execute(Monster entity)
        {
            entity.PrintText("�÷��̾ �߰� ��...");

            float thisToTargetDist = Vector3.Distance(
                GameObject.FindGameObjectWithTag("Player").transform.position,
                entity.transform.position);

            //entity.PrintText(thisToTargetDist.ToString());
            //entity.PrintText("�Ÿ�����: " + Mathf.Abs(thisToTargetDist - entity.AttackRange).ToString());

            // �÷��̾� �ٶ󺸱�

            // �÷��̾����� �̵��ϱ�
        }

        public override void Exit(Monster entity)
        {
            // ��ǥ�� �ϴ� �÷��̾� ���, �ν� �������� �÷��̾ ����, ���� ��Ÿ� ���� �÷��̾ ����
            entity.PrintText("�߰� ����");
        }
    }
    public class ATTACK : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.PrintText("���� ���� ���� ���� �÷��̾� Ȯ��");
        }

        public override void Execute(Monster entity)
        {
            entity.PrintText("�÷��̾� ����");

            // �÷��̾� �ٶ󺸱�
        }

        public override void Exit(Monster entity)
        {
            // �÷��̾� ���, ��Ÿ� ���� �÷��̾ ����
            entity.PrintText("���� ����");
        }
    }

    // Global State�� ���� DIE ���°� ȣ��ȴ�.
    public class DIE : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.PrintText("���");
        }

        public override void Execute(Monster entity)
        {
            // �÷��̾� ��Ȱ ���� Ȯ��
            // 1. ���������� �ٲ� ���
            // entity.RevertToPreviousState();
        }

        public override void Exit(Monster entity)
        {
            entity.PrintText("��Ȱ");
        }
    }

    public class StateGlobal : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            // ����д�.
        }

        // stateMachine���� �� �����Ӹ��� �����ϸ鼭 ���ǿ� �������� Ȯ���Ѵ�.
        public override void Execute(Monster entity)
        {
            // ���� ���� ���°� Global State�� ���� ����Ǵ� ���¶�� �����Ѵ�.
            if (entity.curState == MonsterState.DIE) return;

            if (entity.Hp <= 0)
            {
                entity.Hp = 0;
                entity.ChangeState(MonsterState.DIE);
            }
            // ���⿡ Ȯ�������� ����ϴ� ����( ���� )�� �־ �ȴ�.
            int patternState = Random.Range(0, 100);
            if (patternState < 10) return;              // 10% Ȯ��
        }

        public override void Exit(Monster entity)
        {
            // ����д�.
        }
    }
}

