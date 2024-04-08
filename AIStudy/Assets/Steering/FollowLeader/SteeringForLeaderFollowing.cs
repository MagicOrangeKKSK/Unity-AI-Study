using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringForArrive))]
public class SteeringForLeaderFollowing : Steering
{
    public Vector3 target;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private bool isPlanar;

    public GameObject leader;

    private Vehicle leaderController;
    private Vector3 leaderVelocity;
    //跟随者落后领队的距离
    private float LEADER_BEHIND_DIST = 2.0f;
    private SteeringForArrive arriveScript;
    private Vector3 randomOffset;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.MaxSpeed;
        isPlanar = m_vehicle.IsPlanar;
        leaderController = leader.GetComponent<Vehicle>();
        //为抵达行为指定目标点
        arriveScript = GetComponent<SteeringForArrive>();
        arriveScript.Target = new GameObject("arriveTarget");
        arriveScript.Target.transform.position = leader.transform.position;
    }

    public override Vector3 Force()
    {
        leaderVelocity = leaderController.Velocity;
        //计算目标点
        target = leader.transform.position + LEADER_BEHIND_DIST * (-leaderVelocity).normalized;
        arriveScript.Target.transform.position = target;
        return new Vector3();
    }
}
