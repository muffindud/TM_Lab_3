using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Game game;

    public Sprite monsterSpriteStatic;
    public Sprite monsterSpriteWalk1;
    public Sprite monsterSpriteWalk2;

    public int monsterCount = 0;
    public int maxMonsterCount = 20;

    void Start()
    {
        StartCoroutine(SpawnDelay());
    }

    public void Spawn(float x)
    {
        if (monsterCount < maxMonsterCount)
            // create a new monster game object
            Instantiate(monsterPrefab, new Vector2(x, Globals.worldHeight - 5), Quaternion.identity);
            monsterCount++;
    }

    IEnumerator SpawnDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            Spawn(Random.Range(5, Globals.worldWidth - 5));
        }
    }
}
