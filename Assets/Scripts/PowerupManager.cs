using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] GameObject powerupPrefab;
    List<GameObject> powerups = new List<GameObject>();
    [SerializeField] Vector2 rangeBetweenPowerups;
    [SerializeField] float xPosDestroy;
    [SerializeField] float speed;
    float timer;
    float currentTimer;

    void Start()
    {
        timer = Random.Range(rangeBetweenPowerups.x, rangeBetweenPowerups.y);
    }

    void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer > timer)
        {
            GameObject powerup = Instantiate(powerupPrefab);
            powerups.Add(powerup);
            currentTimer = 0;
        }

        if(powerups.Count > 0)
        {
            foreach (GameObject powerup in powerups)
            {
                if(powerup)
                {
                    powerup.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                    if (powerup.transform.position.x < xPosDestroy)
                    {
                        Destroy(powerup);
                    }
                }
            }
            for (int i = 0; i < powerups.Count; i++)
            {
                if (powerups[i] == null)
                {
                    powerups.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
