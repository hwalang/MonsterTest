using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public Transform target;

    public enum MonsterState
    {
        IDLE,
        CHASE,
        ATTACK,
        DIE,
    }

    public MonsterState curState = MonsterState.IDLE;
    public float chaseDist = 15.0f;
    public float attackDist = 5.0f;

    private Transform _transform;
    private Animator _animator;
    private NavMeshAgent _agent;

    private bool isDead = false;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);  // 0.2초마다 while문을 수행

            float dist = Vector3.Distance(target.position, _transform.position);
            if (dist <= attackDist)
            {
                curState = MonsterState.ATTACK;
            }
            else if (dist <= chaseDist)
            {
                curState = MonsterState.CHASE;
            }
            else
            {
                curState = MonsterState.IDLE;
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case MonsterState.IDLE:
                    _agent.speed = 0;
                    break;
                case MonsterState.CHASE:
                    _agent.destination = target.position;
                    // 플레이어를 바라본다.
                    _agent.speed = 2.5f;
                    _animator.SetBool("isNearPlayer", false);
                    break;
                case MonsterState.ATTACK:
                    _animator.SetBool("isNearPlayer", true);
                    break;
                case MonsterState.DIE:
                    _animator.SetBool("isDie", true);
                    break;
            }

            yield return null;
        }
    }

    private void Update()
    {
        //_agent.SetDestination(target.position);
    }
}
