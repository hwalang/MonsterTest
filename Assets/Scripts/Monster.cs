using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform target;
    NavMeshAgent nmAgent;


    void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        // 목적지를 설정한다.
        // 목적지는 플레이어가 움직이면 변하기 때문에 Update에서 다룬다.
        // 멀티 플레이에서는 가장 가까운 플레이어를 목적지로 설정한다.
        nmAgent.SetDestination(target.position);
    }
}
