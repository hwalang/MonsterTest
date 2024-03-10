using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState    // �ǰ� ���ϸ� ������ ���� �÷��̾ �����ϴ� ���
{
    IDLE = 0,
    CHASE,
    ATTACK,
    DIE,
    GLOBAL,
}

public class Monster : EnemyBaseEntity
{
    private int _level;         // �������� �ö󰡸� ���͵� �������� ��� �ʿ�
    [SerializeField]
    private int _hp;
    private int _maxHp;
    private int _attack;
    private float _moveSpeed;
    private float _attackRange;
    private bool _detectPlayer;

    private State<Monster>[] states;     // Monster�� ��� ���� ����
    private StateMachine<Monster> stateMachine;     // ���� ������ StateMachine�� ����
    public MonsterState curState { private set; get; }  // ���� ����, Global State�� prievState�� ���

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
    public bool DetectPlayer
    {
        set => _detectPlayer = value;
        get => _detectPlayer;
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


        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 5;
        _moveSpeed = 2.0f;
        _attackRange = 1.5f;
        _detectPlayer = false;
    }

    public override void Updated()
    {
        // stateMachine ����
        stateMachine.Execute();
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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DetectPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DetectPlayer = false;
        }
    }
}
