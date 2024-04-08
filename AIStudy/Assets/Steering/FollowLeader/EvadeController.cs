using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeController : MonoBehaviour
{
    public GameObject leader;
    private Vehicle leaderLocomotion;
    private Vehicle m_vehicle;
    private bool isPlanar;
    private Vector3 leaderAhead;
    private float LEADER_BEHIND_DIST;
    private Vector3 dist;
    public float evadeDistance;
    private float sqrEvadeDistance;
    private SteeringForEvada evadeScript;

    private void Start()
    {
        leaderLocomotion = leader.GetComponent<Vehicle>();
        evadeScript = GetComponent<SteeringForEvada>();
        m_vehicle = GetComponent<Vehicle>();
        isPlanar = m_vehicle.IsPlanar;
        LEADER_BEHIND_DIST = 2.0f;
        sqrEvadeDistance = evadeDistance * evadeDistance;
    }

    private void Update()
    {
        //计算领队前方的一个点
        leaderAhead = leader.transform.position +
            leaderLocomotion.Velocity.normalized * LEADER_BEHIND_DIST;

        dist = transform.position - leaderAhead;
        if (isPlanar)
        {
            dist.y = 0;
        }
        if(dist.sqrMagnitude < sqrEvadeDistance)
        {
            //如果小于躲避距离 激活躲避行为
            evadeScript.enabled = true;
        }
        else
        {
            evadeScript.enabled = false;
        }

    }
}
