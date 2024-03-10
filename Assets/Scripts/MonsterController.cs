using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public enum MonsterState
//{
//    IDLE,
//    CHASE,
//    ATTACK,
//    DIE,
//}

public class MonsterController : MonoBehaviour
{
    private MonsterState _monsterState;

    private void Awake()
    {
        ChangeState(MonsterState.IDLE);
    }

    void Start()
    {
        //StartCoroutine(CheckState());
        //StartCoroutine(CheckStateForAction());
    }

    private void Update()
    {
        // �� �ൿ�� �߻��ϴ� ������ ����Ѵ�. -> Coroutine���� �ϴ°� ���ɻ����� ����.
        if (Input.GetKeyDown("1"))
        {
            ChangeState(MonsterState.IDLE);
        }
        else if (Input.GetKeyDown("2"))
        {
            ChangeState(MonsterState.CHASE);
        }
        else if ( Input.GetKeyDown("3"))
        {
            ChangeState(MonsterState.ATTACK);
        }
        else if ( Input.GetKeyDown("4"))
        {
            ChangeState(MonsterState.DIE);
        }
    }

    private void ChangeState(MonsterState newState)
    {
        // ������ ����.ToString()���� enum�� ���ǵ� ���� �̸��� string���� ��ȯ ����
        // _monsterState = MonsterState.IDLE; => "IDLE" ��ȯ
        // �̸� �̿��ؼ� enum�� ���ǵ� ���¿� ������ �̸��� �ڷ�ƾ �޼ҵ带 �����Ѵ�.


        StopCoroutine(_monsterState.ToString());    // ���� ���� �ڷ�ƾ ����
        _monsterState = newState;                   // ���ο� ���·� ����
        StartCoroutine(_monsterState.ToString());   // ���� ������ �ڷ�ƾ ����
    }

    private IEnumerator IDLE()
    {
        // while ������ ���� ������ ��, 1ȸ ȣ���ϴ� ������ �ۼ�
        Debug.Log("������ ���·� ����");
        Debug.Log("ü���� �ʴ� 10�� �ڵ� ȸ��");

        // �� ������ ȣ���ϴ� ���� �ۼ�
        while (true)
        {
            Debug.Log("���Ͱ� ���ڸ����� �����");
            yield return null;
        }

        // while ���Ĵ� ���� ���°� ����� ��, 1ȸ ȣ���ϴ� ���� �ۼ�
    }
    private IEnumerator CHASE()
    {
        // �ν� ���� ���� �ִ� ���
        Debug.Log("�߰� ���·� ����");

        while (true)
        {
            Debug.Log("���Ͱ� �÷��̾ �߰�");
            yield return null;
        }
    }
    private IEnumerator ATTACK()
    {
        Debug.Log("���� ���·� ����");

        // ���� ��Ÿ� ���ο� �ִ� ���
        while (true)
        {
            Debug.Log("���Ͱ� �÷��̾ ����");
            yield return null;
        }
    }
    private IEnumerator DIE()
    {
        Debug.Log("��� ���·� ����");
        yield return null;
    }
}
