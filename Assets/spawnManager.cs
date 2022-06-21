using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] GameObject wolfPrefab;
    [SerializeField] float minTimeBetweenSpawns;
    [SerializeField] float maxTimeBetweenSpawns;
    [SerializeField] float minDistanceFromSpawn;
    [SerializeField] float maxDistanceFromSpawn;
    int[] multipliersForSpawn = new int[] { -1, 1 };
    List<GameObject> wolves = new List<GameObject>();
    Coroutine wolfTimer;
    private lake[] lakes;

    private void Start()
    {
        lakes = FindObjectsOfType<lake>();
    }
    public void startWolfTimer()
    {
        wolfTimer = StartCoroutine(spawnWolf());
    }
    public void stopWolfTimer() 
    {
        StopCoroutine(wolfTimer);
    }
    IEnumerator spawnWolf() 
    {
        yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));        
        Instantiate(wolfPrefab, generateSpawnPoint());
        startWolfTimer();
    }
    private Transform generateSpawnPoint() 
    {
        Vector3 positionToSpawnIn = new Vector3(Random.Range(minDistanceFromSpawn, maxDistanceFromSpawn) * multipliersForSpawn[Random.Range(0, multipliersForSpawn.Length)],
            Random.Range(minDistanceFromSpawn, maxDistanceFromSpawn) * multipliersForSpawn[Random.Range(0, multipliersForSpawn.Length)], 0);
        while (onLake(positionToSpawnIn.x, positionToSpawnIn.y))
        {
            positionToSpawnIn = new Vector3(Random.Range(minDistanceFromSpawn, maxDistanceFromSpawn) * multipliersForSpawn[Random.Range(0, multipliersForSpawn.Length)],
            Random.Range(minDistanceFromSpawn, maxDistanceFromSpawn) * multipliersForSpawn[Random.Range(0, multipliersForSpawn.Length)], 0);
        }        
        Transform toReturn = new GameObject("WolfSpawnPoint").transform;
        toReturn.transform.position = positionToSpawnIn;
        return toReturn;

    }
    bool onLake(float x, float y)
    {
        foreach (lake l in lakes)
        {
            if (l.gameObject.GetComponent<PolygonCollider2D>().OverlapPoint(new Vector2(x, y)))
            {
                return true;
            }
        }
        return false;
    }
    public void addToList(GameObject wolf) 
    {
        if (wolf != null)
        {
            wolves.Add(wolf);
        }
    }
}
