using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    
    public enum MonsterState
    {
        IDLE,
        CHASE,
        ATTACK,
        DIE,
    }

    public MonsterState curState = MonsterState.IDLE;
    public float chaseDist = 15.0f;
    public float attackDist = 2.0f;

    private Transform _transform;
    private Transform target;
    private NavMeshAgent nmAgent;
    private bool isDead = false;
    private Animator _animator;

    [System.Obsolete]
    void Start()
    {
        _transform = GetComponent<Transform>();
        nmAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

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

    [System.Obsolete]
    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case MonsterState.IDLE:
                    nmAgent.Stop();
                    break;
                case MonsterState.CHASE:
                    nmAgent.destination = target.position;
                    nmAgent.Resume();
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
}
