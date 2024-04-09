using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum MonsterState    // �ǰ� ���ϸ� ������ ���� �÷��̾ �����ϴ� ���
{
    IDLE = 0,
    // PATROL,
    CHASE,
    ATTACK,
    DIE,
    GLOBAL,
}

public class MonsterController : EnemyBaseEntity
{
    [SerializeField]
    private Transform _detectPlayer;
    [SerializeField]
    private Transform _attackPlayer;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private NavMeshAgent _agent;

    private int _level;         // �������� �ö󰡸� ���͵� �������� ��� �ʿ�
    [SerializeField]
    private int _hp;
    private int _maxHp;
    private int _damage;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _detectRange;

    private State<MonsterController>[] states;     // Monster�� ��� ���� ����
    private StateMachine<MonsterController> stateMachine;     // ���� ������ StateMachine�� ����

    private Coroutine updateDetectPlayer;
    private Coroutine updateAttackPlayer;
    private Coroutine checkMonsterState;

    public MonsterState curState { private set; get; }  // ���� ����, Global State�� prievState�� ���
    public NavMeshAgent Agent { private set; get; }
    public Transform DetectPlayer
    {
        set => _detectPlayer = value;
        get => _detectPlayer;
    }
    public Transform AttackPlayer
    {
        set => _attackPlayer = value;
        get => _attackPlayer;
    }
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
    public int Damage
    {
        set => _damage = value;
        get => _damage;
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

        gameObject.name = $"{ID:D2}_Monster_{name}";    // 00_Monster_name ���� hierarchy â���� ����

        states = new State<MonsterController>[5];
        states[(int)MonsterState.IDLE] = new MonsterStateItem.IDLE();
        states[(int)MonsterState.CHASE] = new MonsterStateItem.CHASE();
        states[(int)MonsterState.ATTACK] = new MonsterStateItem.ATTACK();
        states[(int)MonsterState.DIE] = new MonsterStateItem.DIE();
        states[(int)MonsterState.GLOBAL] = new MonsterStateItem.StateGlobal();

        // stateMachine �ʱ�ȭ
        stateMachine = new StateMachine<MonsterController>();
        stateMachine.Setup(this, states[(int)MonsterState.IDLE]);
        stateMachine.SetGlobalState(states[(int)MonsterState.GLOBAL]);  // ���� ���� ����

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();

        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _damage = 5;
        _moveSpeed = 2.0f;
        _attackRange = 1.8f;
        _agent.stoppingDistance = 1.5f;
        _detectRange = 15.0f;

        //updateAttackPlayer = StartCoroutine(UpdateAttackPlayer());
        //updateDetectPlayer = StartCoroutine(UpdateDetectPlayer());
        checkMonsterState = StartCoroutine(CheckMonsterState());
    }

    public override void Updated()
    {
        
        // stateMachine ����
        stateMachine.Execute();
    }

    // ĳ���Ϳ��� �������� �޾Ƶ� �з����� ���ӵ��� ���� �̵��� ���ع��� �ʴ´�.
    public void FreezeVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    // �ν� ���� ���� �÷��̾ ����
    private void UpdateDetectPlayer()
    {
        // ���� ���� Player layer�� ��ü�� ����
        Collider[] detectPlayers = Physics.OverlapSphere(transform.position, DetectRange, 1 << 7);
        Collider[] targetPlayers = Physics.OverlapSphere(transform.position, AttackRange, 1 << 7);
        //Physics.OverlapBoxNonAlloc() �ν��ϴ� ������ ��Ȯ�ϸ� �޸𸮸� �Ƴ� �� �ֱ� ������ �� ����.

        float minDistAttack = AttackRange;
        if (targetPlayers.Length > 0)
        {
            for (int i = 0; i < targetPlayers.Length; ++i)
            {
                float dist = Vector3.Distance(this.transform.position, targetPlayers[i].transform.position);
                PrintText($"dist: {dist}");
                PrintText($"���� �����Ÿ� ���� �÷��̾ {targetPlayers.Length}��ŭ �ν�");
                if (minDistAttack > dist)
                {
                    minDistAttack = dist;
                    AttackPlayer = targetPlayers[i].gameObject.transform;
                }
            }
        }
        else
        {
            AttackPlayer = null;
        }

        float minDistDetect = DetectRange;
        if (detectPlayers.Length > 0)
        {
            for (int i = 0; i < detectPlayers.Length; ++i)
            {
                float dist = Vector3.Distance(this.transform.position, detectPlayers[i].transform.position);
                PrintText($"�÷��̾ {detectPlayers.Length}��ŭ �ν�");
                if (minDistDetect > dist)
                {
                    minDistDetect = dist;
                    DetectPlayer = detectPlayers[i].gameObject.transform;
                }
            }
        }
        else
        {
            DetectPlayer = null;
        }

        minDistAttack = AttackRange;
        minDistDetect = DetectRange;
    }

    // ���� ���� ���� �÷��̾� ����
    IEnumerator UpdateAttackPlayer()
    {
        yield return null;

        Collider[] attackPlayers = Physics.OverlapSphere(transform.position, AttackRange, 1 << 7);

        float minDist = AttackRange;
        for (int i = 0; i < attackPlayers.Length; i++)
        {
            Transform target = attackPlayers[i].gameObject.transform;
            if (minDist > Vector3.Distance(this.transform.position, target.position))
            {
                AttackPlayer = target;
                Debug.Log("target: " + AttackPlayer);
            }
        }
    }

    // Monster State�� �ٲ��ִ� �Լ�
    IEnumerator CheckMonsterState()
    {
        while (Hp > 0)
        {
            // yield return new WaitForSeconds(0.3f);
            yield return null;

            UpdateDetectPlayer();

            _animator.SetBool("isDie", false);

            if (AttackPlayer != null)
            {
                if (curState == MonsterState.ATTACK) continue;
                
                _animator.SetBool("isDetect", true);
                _animator.SetBool("isPlayerInAttackRange", true);
                ChangeState(MonsterState.ATTACK);
            }
            else if (DetectPlayer != null)
            {
                if (curState == MonsterState.CHASE) continue;
                
                _animator.SetBool("isDetect", true);
                _animator.SetBool("isPlayerInAttackRange", false);
                ChangeState(MonsterState.CHASE);
            }
            else
            {
                if (curState == MonsterState.IDLE) continue;
                
                _animator.SetBool("isDetect", false);
                _animator.SetBool("isPlayerInAttackRange", false);
                DetectPlayer = null;
                AttackPlayer = null;
                ChangeState(MonsterState.IDLE);
            }

        }

        
        _animator.SetBool("isDie", true);
        ChangeState(MonsterState.DIE);
    }

    public void ChangeState(MonsterState newState)
    {
        // ���� �ٲ�� ���¸� ����
        curState = newState;

        // stateMachine�� ���� ������ ����
        stateMachine.ChangeState(states[(int)newState]);
    }

    public void RevertToPreviousState()
    {
        stateMachine.RevertToPreviousState();
    }
}
