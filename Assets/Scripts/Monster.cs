using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster
{ 
    private State state;

    public Monster(State state) // � ���°� ������ ������ �ϴ� ���¸� �Է� �޴´�.
    {
        this.state = state;
    }

    public void setState(State state)
    {
        this.state = state;
    }

    public void act()   // ���¿� ���� �ൿ�� �ڵ����� ����
    {
        state.Action();
    }
}
