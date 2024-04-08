using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBotsForFollowLeader : MonoBehaviour
{
    public GameObject botPrefab;
    public GameObject leader;
    public int botCount;

    public float minX = -10f;
    public float maxX = 60f;
    public float minZ = -10f;
    public float maxZ = 60f;
    public float yValue = 1f;

    public void Start()
    {
        for(int i = 0; i < botCount; i++)
        {
            Vector3 spawnPostion = new Vector3(Random.Range(minX, maxX),
                                               yValue,
                                               Random.Range(minZ, maxZ));
            GameObject bot = GameObject.Instantiate(botPrefab, spawnPostion, Quaternion.identity);
            bot.GetComponent<SteeringForLeaderFollowing>().leader = leader;
            bot.GetComponent<SteeringForEvada>().target = leader;
            bot.GetComponent<SteeringForEvada>().enabled = false;
            bot.GetComponent<EvadeController>().leader = leader;
        }
    }
}
