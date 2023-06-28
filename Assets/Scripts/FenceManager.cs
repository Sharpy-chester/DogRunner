using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    List<GameObject> fences = new List<GameObject>();
    [SerializeField] float fenceSpeed = 1f;
    [SerializeField] float fenceSpeedIncreacer = 0.05f;
    [SerializeField] float fenceDeletePosX;
    [SerializeField] float startingSpawnTime = 3f;
    float spawnTime = 3f;
    [SerializeField] float spawnTimeRand = 1f;
    float currentSpawnTime = 0;
    [SerializeField] Vector3 spawnPos;
    bool playerDead = false;
    PlayerController playerController;
    

    void Start()
    {
        spawnTime = startingSpawnTime;
        playerController = FindObjectOfType<PlayerController>();
        playerController.playerDeath += PlayerDead;
    }

    void Update()
    {
        if(!playerDead)
        {
            currentSpawnTime += Time.deltaTime;
            if (currentSpawnTime > spawnTime)
            {
                currentSpawnTime = 0;
                GameObject newFence = Instantiate(fencePrefab, spawnPos, fencePrefab.transform.rotation);
                fences.Add(newFence);

                spawnTime = Random.Range(startingSpawnTime - spawnTimeRand, startingSpawnTime + spawnTimeRand);
            }

            if (fences.Count > 0)
            {
                foreach (GameObject fence in fences)
                {
                    fence.transform.position -= new Vector3(fenceSpeed * Time.deltaTime, 0, 0);
                    if (fence.transform.position.x < fenceDeletePosX)
                    {
                        fences.Remove(fence);
                        Destroy(fence);
                        return;
                    }
                }
            }
        }
    }

    void PlayerDead()
    {
        playerDead = true;
    }

    public void Retry()
    {
        playerDead = false;
        foreach (GameObject fence in fences)
        {
            Destroy(fence);
        }
        fences.Clear();
    }
}
