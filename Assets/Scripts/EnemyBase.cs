using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    // https://www.youtube.com/watch?v=mHYroWpIIP0
    public float maxHp = 100f;
    public float curHp = 100f;

    public float damage = 100f;

    protected float playerRealizeRange = 10f;
    protected float attackRange = 5f;
    protected float attackCoolTime = 5f;
    protected float attackCoolTimeCacl = 5;
    protected bool canAtk = true;

    protected float moveSpeed = 2f;

    protected GameObject player;
    protected NavMeshAgent nmAgent;
    protected float distance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
