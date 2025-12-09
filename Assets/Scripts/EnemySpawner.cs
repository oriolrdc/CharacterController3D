using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private bool EnemysDefeated = true;
    void Update()
    {
        if(EnemysDefeated == true)
        {
            StartCoroutine(Spawn());
        }
    }
    IEnumerator Spawn()
    {
        EnemysDefeated = false;
        Instantiate(enemyPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(5);
        EnemysDefeated = true;
    }
}
