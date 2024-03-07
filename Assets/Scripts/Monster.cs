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
        // �������� �����Ѵ�.
        // �������� �÷��̾ �����̸� ���ϱ� ������ Update���� �ٷ��.
        // ��Ƽ �÷��̿����� ���� ����� �÷��̾ �������� �����Ѵ�.
        nmAgent.SetDestination(target.position);
    }
}
