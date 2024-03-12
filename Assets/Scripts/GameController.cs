using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Agent�� �̵��� �� �ִ� ��� => ��Ÿ���� ���������� ������ �� ����غ���.
public enum Location
{
    GROUND = 0,
};

public class GameController : MonoBehaviour
{
    // TYPE�� �þ�� �̷��� �迭�� prefab ������ �߰��Ѵ�.
    [SerializeField]
    private string[] arrayMonsters;     // monster �̸� �迭, Inspector view���� ���� �Է�
    [SerializeField]
    private GameObject monsterPrefab;   // monster TYPE prefab

    private List<EnemyBaseEntity> entitys;  // Monster, Player �� ���� ���� ��� entity�� ���� �� �ִ�.

    public static bool IsGameStop { set; get; } = false;


    private void Awake()
    {
        entitys = new List<EnemyBaseEntity>();

        for (int i = 0; i < arrayMonsters.Length; i++)
        {
            GameObject clone = Instantiate(monsterPrefab);
            Monster monsterEntity = clone.GetComponent<Monster>();
            monsterEntity.Setup(arrayMonsters[i]);

            entitys.Add(monsterEntity);
        }
    }

    private void Update()
    {
        if (IsGameStop == true) return;

        // ��� monsterEntity�� ���۽�Ű�� ���ؼ� Updated()�� ȣ���Ѵ�.
        for (int i = 0; i < entitys.Count; ++i)
        {
            //if (entitys[i].GetComponent<Monster>().Hp < 0)
            //{
            //    Destroy(entitys[i], 3.0f);
            //    entitys[i] = null;
            //    continue;
            //}
            entitys[i].Updated();
        }
    }

    public static void Stop(EnemyBaseEntity entity)
    {
        IsGameStop = true;
    }
}
