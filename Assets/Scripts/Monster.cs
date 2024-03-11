using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum MonsterState    // 피격 당하면 공격을 가한 플레이어를 추적하는 기능
{
    IDLE = 0,
    // PATROL,
    CHASE,
    ATTACK,
    DIE,
    GLOBAL,
}

public class Monster : EnemyBaseEntity
{
    [SerializeField]
    private Transform _detectPlayer;
    [SerializeField]
    private Transform _attackPlayer;
    private Animator _animator;

    private int _level;         // 스테이지 올라가면 몬스터도 강해지는 경우 필요
    [SerializeField]
    private int _hp;
    private int _maxHp;
    private int _attack;
    private float _moveSpeed;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _detectRange;

    private State<Monster>[] states;     // Monster의 모든 상태 정보
    private StateMachine<Monster> stateMachine;     // 상태 관리를 StateMachine에 위임
    public MonsterState curState { private set; get; }  // 현재 상태, Global State와 prievState를 대비

    public int Level
    {
        set => _level = value;
        get => _level;
    }
    public int Hp
    {
        set => _hp = value;
        get => _hp;
    }
    public int MaxHp
    {
        set => _maxHp = value; 
        get => _maxHp;
    }
    public int Attack
    {
        set => _attack = value;
        get => _attack;
    }
    public float MoveSpeed
    {
        set => _moveSpeed = value; 
        get => _moveSpeed;
    }
    public float AttackRange
    {
        set => _attackRange = value;
        get => _attackRange;
    }
    public float DetectRange
    {
        set => _detectRange = value;
        get => _detectRange;
    }

    public override void Setup(string name)
    {
        base.Setup(name);

        gameObject.name = $"{ID:D2}_Monster_{name}";    // 00_Monster_name 으로 hierarchy 창에서 보임

        states = new State<Monster>[5];
        states[(int)MonsterState.IDLE] = new MonsterStateItem.IDLE();
        states[(int)MonsterState.CHASE] = new MonsterStateItem.CHASE();
        states[(int)MonsterState.ATTACK] = new MonsterStateItem.ATTACK();
        states[(int)MonsterState.DIE] = new MonsterStateItem.DIE();
        states[(int)MonsterState.GLOBAL] = new MonsterStateItem.StateGlobal();

        // stateMachine 초기화
        stateMachine = new StateMachine<Monster>();
        stateMachine.Setup(this, states[(int)MonsterState.IDLE]);
        stateMachine.SetGlobalState(states[(int)MonsterState.GLOBAL]);  // 전역 상태 설정

        _animator = GetComponent<Animator>();
        _animator.SetBool("isDetect", false);
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 5;
        _moveSpeed = 2.0f;
        _attackRange = 1.2f;
        _detectRange = 10.0f;

        // UpdateTarget 함수를 0.3초마다 호출
        // InvokeRepeating("UpdateTarget", 0f, 0.3f);      
        // 몬스터 상태 체크
        StartCoroutine(CheckMonsterState());
    }

    public override void Updated()
    {
        // stateMachine 실행
        stateMachine.Execute();
    }

    private void UpdateTarget()
    {
        // 범위 내의 Player layer의 객체를 저장
        Collider[] detectPlayers = Physics.OverlapSphere(transform.position, DetectRange, 1 << 7);
        Collider[] targetPlayers = Physics.OverlapSphere(transform.position, AttackRange, 1 << 7);
        
        if (targetPlayers.Length > 0)
        {
            for (int i = 0; i < targetPlayers.Length; ++i)
            {
                PrintText($"공격 사정거리 내의 플레이어를 {targetPlayers.Length}만큼 인식");
                _attackPlayer = targetPlayers[i].gameObject.transform;
            }
        }
        else
        {
            _attackPlayer = null;
        }

        if (detectPlayers.Length > 0)
        {
            for (int i = 0; i < detectPlayers.Length; ++i)
            {
                PrintText($"플레이어를 {detectPlayers.Length}만큼 인식");
                _detectPlayer = detectPlayers[i].gameObject.transform;
            }
        }
        else
        {
            _detectPlayer = null;
        }
    }

    // Monster State를 바꿔주는 함수
    IEnumerator CheckMonsterState()
    {
        while (Hp > 0 && _animator.GetBool("isDie") == false)
        {
            yield return new WaitForSeconds(0.3f);

            UpdateTarget();

            _animator.SetBool("isDie", false);

            if (_attackPlayer != null)
            {
                if (curState == MonsterState.ATTACK) continue;
                ChangeState(MonsterState.ATTACK);
                _animator.SetBool("isDetect", true);
                _animator.SetBool("isNearPlayer", true);
            }
            else if (_detectPlayer != null)
            {
                if (curState == MonsterState.CHASE) continue;
                ChangeState(MonsterState.CHASE);
                _animator.SetBool("isDetect", true);
                _animator.SetBool("isNearPlayer", false);
            }
            else
            {
                if (curState == MonsterState.IDLE) continue;
                ChangeState(MonsterState.IDLE);
                _animator.SetBool("isDetect", false);
                _animator.SetBool("isNearPlayer", false);
            }

        }

        ChangeState(MonsterState.DIE);
        _animator.SetBool("isDie", true);
        // Destroy(this, 1.5f);
    }

    public void ChangeState(MonsterState newState)
    {
        // 새로 바뀌는 상태를 저장
        curState = newState;

        // stateMachine에 상태 변경을 위임
        stateMachine.ChangeState(states[(int)newState]);
    }

    public void RevertToPreviousState()
    {
        stateMachine.RevertToPreviousState();
    }

    // 공격 범위에 플레이어가 탐지된 경우, 플레이어를 공격한다.
    //      - 단순 거리 계산 + 가까워진 플레이어에 한해서 ray를 통해 플레이어를 인식
    //      - 벽 너머의 보이지 않는 플레이어를 인식하는 것을 방지한다.
}
