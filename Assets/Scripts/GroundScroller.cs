using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    [SerializeField] float offsetPerSecond = 1f;
    float startingOffset;
    float currentOffset = 0;
    Material thisMat;
    PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        thisMat = GetComponent<MeshRenderer>().material;
        playerController.playerDeath += StopScrolling;
        startingOffset = offsetPerSecond;
    }

    void Update()
    {
        if (thisMat)
        {
            thisMat.mainTextureOffset += new Vector2(offsetPerSecond * Time.deltaTime, 0);
        }
    }

    void StopScrolling()
    {
        offsetPerSecond = 0;
    }

    public void Restart()
    {
        offsetPerSecond = startingOffset;
    }
}
