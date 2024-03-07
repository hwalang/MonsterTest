using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster
{ 
    private State state;

    public Monster(State state) // 어떤 상태가 들어올지 모르지만 일단 상태를 입력 받는다.
    {
        this.state = state;
    }

    public void setState(State state)
    {
        this.state = state;
    }

    public void act()   // 상태에 따른 행동을 자동으로 수행
    {
        state.Action();
    }
}
