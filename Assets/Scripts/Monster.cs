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

public class Monster : EnemyBaseEntity
{
    [SerializeField]
    private Transform _detectPlayer;
    [SerializeField]
    private Transform _attackPlayer;
    private Animator _animator;

    private int _level;         // �������� �ö󰡸� ���͵� �������� ��� �ʿ�
    [SerializeField]
    private int _hp;
    private int _maxHp;
    private int _attack;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _detectRange;

    private State<Monster>[] states;     // Monster�� ��� ���� ����
    private StateMachine<Monster> stateMachine;     // ���� ������ StateMachine�� ����
    public MonsterState curState { private set; get; }  // ���� ����, Global State�� prievState�� ���

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

        gameObject.name = $"{ID:D2}_Monster_{name}";    // 00_Monster_name ���� hierarchy â���� ����

        states = new State<Monster>[5];
        states[(int)MonsterState.IDLE] = new MonsterStateItem.IDLE();
        states[(int)MonsterState.CHASE] = new MonsterStateItem.CHASE();
        states[(int)MonsterState.ATTACK] = new MonsterStateItem.ATTACK();
        states[(int)MonsterState.DIE] = new MonsterStateItem.DIE();
        states[(int)MonsterState.GLOBAL] = new MonsterStateItem.StateGlobal();

        // stateMachine �ʱ�ȭ
        stateMachine = new StateMachine<Monster>();
        stateMachine.Setup(this, states[(int)MonsterState.IDLE]);
        stateMachine.SetGlobalState(states[(int)MonsterState.GLOBAL]);  // ���� ���� ����

        _animator = GetComponent<Animator>();
        _animator.SetBool("isDetect", false);
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 5;
        _moveSpeed = 2.0f;
        _attackRange = 1.2f;
        _detectRange = 10.0f;

        // UpdateTarget �Լ��� 0.3�ʸ��� ȣ��
        // InvokeRepeating("UpdateTarget", 0f, 0.3f);      
        // ���� ���� üũ
        StartCoroutine(CheckMonsterState());
    }

    public override void Updated()
    {
        // stateMachine ����
        stateMachine.Execute();
    }

    private void UpdateTarget()
    {
        // ���� ���� Player layer�� ��ü�� ����
        Collider[] detectPlayers = Physics.OverlapSphere(transform.position, DetectRange, 1 << 7);
        Collider[] targetPlayers = Physics.OverlapSphere(transform.position, AttackRange, 1 << 7);
        
        if (targetPlayers.Length > 0)
        {
            for (int i = 0; i < targetPlayers.Length; ++i)
            {
                PrintText($"���� �����Ÿ� ���� �÷��̾ {targetPlayers.Length}��ŭ �ν�");
                AttackPlayer = targetPlayers[i].gameObject.transform;
            }
        }
        else
        {
            AttackPlayer = null;
        }

        if (detectPlayers.Length > 0)
        {
            for (int i = 0; i < detectPlayers.Length; ++i)
            {
                PrintText($"�÷��̾ {detectPlayers.Length}��ŭ �ν�");
               DetectPlayer = detectPlayers[i].gameObject.transform;
            }
        }
        else
        {
            DetectPlayer = null;
        }
    }

    // Monster State�� �ٲ��ִ� �Լ�
    IEnumerator CheckMonsterState()
    {
        while (Hp > 0 && _animator.GetBool("isDie") == false)
        {
            yield return new WaitForSeconds(0.3f);

            UpdateTarget();

            _animator.SetBool("isDie", false);

            if (AttackPlayer != null)
            {
                if (curState == MonsterState.ATTACK) continue;
                ChangeState(MonsterState.ATTACK);
                _animator.SetBool("isDetect", true);
                _animator.SetBool("isPlayerInAttackRange", true);
            }
            else if (DetectPlayer != null)
            {
                if (curState == MonsterState.CHASE) continue;
                ChangeState(MonsterState.CHASE);
                _animator.SetBool("isDetect", true);
                _animator.SetBool("isPlayerInAttackRange", false);
            }
            else
            {
                if (curState == MonsterState.IDLE) continue;
                ChangeState(MonsterState.IDLE);
                _animator.SetBool("isDetect", false);
                _animator.SetBool("isPlayerInAttackRange", false);
            }

        }

        ChangeState(MonsterState.DIE);
        _animator.SetBool("isDie", true);
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

    // ���� ������ �÷��̾ Ž���� ���, �÷��̾ �����Ѵ�.
    //      - �ܼ� �Ÿ� ��� + ������� �÷��̾ ���ؼ� ray�� ���� �÷��̾ �ν�
    //      - �� �ʸ��� ������ �ʴ� �÷��̾ �ν��ϴ� ���� �����Ѵ�.
}
