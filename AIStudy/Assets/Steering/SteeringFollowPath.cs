using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringFollowPath : Steering
{
    //由节点数组表示路径
    public GameObject[] waypoints = new GameObject[2];

    //目标点
    private Transform target;

    //当前路点
    private int currentNode;

    //抵达半径
    public float arriveDistance;
    private Vector3 force; //操控力
    private float sqrArriveDistance;
    //路点的数量
    private int numberOfNodes;

    private Vector3 desiredVelocity;

    private Vehicle vehicle;

    private float maxSpeed;
    private bool isPlanar;

    public float slowDownDistance; //减速距离

    private void Start()
    {
        numberOfNodes = waypoints.Length;
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        isPlanar = vehicle.IsPlanar;
        //设置当前路点为第0个
        currentNode = 0;
        //设置当前路点为目标点
        target = waypoints[currentNode].transform;
        sqrArriveDistance = arriveDistance * arriveDistance;
    }

    public override Vector3 Force()
    {
        force = new Vector3(0, 0, 0);
        Vector3 dist = target.position - transform.position;
        if (isPlanar)
        {
            dist.y = 0;
        }
        //如果当前路点已经是路点数组的最后一个
        if(currentNode == numberOfNodes-1)
        {
            //如果当前路点距离大于减速距离
            if(dist.magnitude > slowDownDistance)
            {
                desiredVelocity = dist.normalized * maxSpeed;
                force = desiredVelocity - vehicle.Velocity;
            }
            else
            {
                desiredVelocity = dist - vehicle.Velocity;
                force = desiredVelocity - vehicle.Velocity;
            }
        }
        else
        {
            //当前节点不是节点数组中的最后一个  也就是正走向中间的节点
            if (dist.sqrMagnitude < sqrArriveDistance)
            {
                currentNode++;
                target = waypoints[currentNode].transform;
            }
            desiredVelocity = dist.normalized * maxSpeed;
            force = desiredVelocity - vehicle.Velocity;
        }
        return force;
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawWireSphere(waypoints[i].transform.position, arriveDistance);
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
            }
        }

        if (waypoints[numberOfNodes - 1] != null)
        {
            Gizmos.DrawWireSphere(waypoints[numberOfNodes - 1].transform.position, slowDownDistance );
        }

    }

}
