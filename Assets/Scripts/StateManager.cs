using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    
    void Start()
    {
        Monster monster = new Monster(new Move());  // ���¸� ����� ���¿� ���� �ൿ�� ����
        monster.act();


        //monster.setState(new Attack()); // ���� ����
        //monster.act();
    }

    
    void Update()
    {
        
    }
}
