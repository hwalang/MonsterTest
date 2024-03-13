using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public enum MonsterCode
{
    Slime = 0,

}

public class MonsterStat : Stat
{
    private MonsterCode _monsterCode;
    private int _level;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _detectRange;

    public MonsterCode MonsterCode
    {
        set => _monsterCode = value;
        get => _monsterCode;
    }
    public int Level
    {
        set => _level = value;
        get => _level;
    }
    public float AttackRange
    {
        set => _attackRange = value;
        get => _attackRange;
    }
    public float DetectRange
    {
        set => _detectRange = value;
        get => _detectRange;
    }

    // Awake에서 호출
    public Stat InitStat(MonsterCode monsterCode)
    {
        Stat stat = null;

        switch (monsterCode)
        {
            case MonsterCode.Slime:
                stat = new Stat();
                break;
        }

        return stat;
    }
}
