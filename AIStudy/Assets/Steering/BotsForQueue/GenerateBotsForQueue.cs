using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBotsForQueue : MonoBehaviour
{
    public GameObject botPrefab;
    public int botCount;
    public GameObject target;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public float Yvalue;

    public void Start()
    {
        Vector3 spawnPosition;
        GameObject bot;

        for (int i = 0; i < botCount; i++)
        {
            spawnPosition = new Vector3(Random.Range(minX, maxX), 
                Yvalue, Random.Range(minZ, maxZ));
            bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity);
            bot.GetComponent<SteeringForArrive>().Target = target;
        }
    }

    public void DrawGizmos()
    {
        Vector3 centerPos = new Vector3(minX + (maxX - minX) / 2f
            ,Yvalue, minZ + (maxZ - minZ)/2);
        Vector3 size = new Vector3(maxX - minX, 5f, maxZ - minZ);
        Gizmos.DrawWireCube(centerPos, size);
    }
}
