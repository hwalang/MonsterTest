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
        // 각 행동이 발생하는 조건을 명시한다. -> Coroutine으로 하는게 성능상으로 좋다.
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
        // 열거형 변수.ToString()으로 enum에 정의된 변수 이름을 string으로 반환 가능
        // _monsterState = MonsterState.IDLE; => "IDLE" 반환
        // 이를 이용해서 enum에 정의된 상태와 동일한 이름의 코루틴 메소드를 정의한다.


        StopCoroutine(_monsterState.ToString());    // 이전 상태 코루틴 종료
        _monsterState = newState;                   // 새로운 상태로 변경
        StartCoroutine(_monsterState.ToString());   // 현재 상태의 코루틴 실행
    }

    private IEnumerator IDLE()
    {
        // while 이전은 현재 상태일 때, 1회 호출하는 로직을 작성
        Debug.Log("비전투 상태로 변경");
        Debug.Log("체력이 초당 10씩 자동 회복");

        // 매 프레임 호출하는 내용 작성
        while (true)
        {
            Debug.Log("몬스터가 제자리에서 대기중");
            yield return null;
        }

        // while 이후는 현재 상태가 종료될 때, 1회 호출하는 로직 작성
    }
    private IEnumerator CHASE()
    {
        // 인식 범위 내에 있는 경우
        Debug.Log("추격 상태로 변경");

        while (true)
        {
            Debug.Log("몬스터가 플레이어를 추격");
            yield return null;
        }
    }
    private IEnumerator ATTACK()
    {
        Debug.Log("공격 상태로 변경");

        // 공격 사거리 내부에 있는 경우
        while (true)
        {
            Debug.Log("몬스터가 플레이어를 공격");
            yield return null;
        }
    }
    private IEnumerator DIE()
    {
        Debug.Log("사망 상태로 변경");
        yield return null;
    }
}
