using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    
    void Start()
    {
        Monster monster = new Monster(new Move());  // 상태를 만들고 상태에 따른 행동을 위임
        monster.act();


        //monster.setState(new Attack()); // 상태 변경
        //monster.act();
    }

    
    void Update()
    {
        
    }
}
