using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum MonsterState
    {
        IDLE,
        CHASE,
        ATTACK,
        DIE,
    }

    public Animator animator;
    public NavMeshAgent nmAgent;
    public Transform target;
    public float dest;

    private MonsterState state;
    private float HP = 0.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();

        state = MonsterState.IDLE;
        HP = 10.0f;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (HP > 0)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    private void Update()
    {
        switch (state)
        {
            case MonsterState.IDLE:
                Debug.Log("Monster - IDLE");
                if (animator.GetBool("isNearPlayer"))
                {
                    state = MonsterState.ATTACK;
                }
                else
                {
                    state = MonsterState.CHASE;
                }
                break;
            case MonsterState.CHASE:
                Debug.Log("Monster - CHASE");
                
                break;
            case MonsterState.ATTACK:
                Debug.Log("Monster - ATTACK");
                break;
            case MonsterState.DIE:
                Debug.Log("Monster - DIE");
                state = MonsterState.DIE;
                break;
            
        }
    }

    public void MoveAction()
    {
        animator.SetBool("Run", true);
    }
}
