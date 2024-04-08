using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForArrive : Steering
{
    public bool IsPlanar = true;
    //抵达距离
    public float ArrivalDistance = 0.3f;


    public float SlowDownDistance= 1.2f;
    public GameObject Target;

    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        IsPlanar = vehicle.IsPlanar;
        maxSpeed = vehicle.MaxSpeed;
    }

    public override Vector3 Force()
    {
        //计算角色与目标之间的距离
        Vector3 toTarget = Target.transform.position - transform.position;
        //预期速度
        Vector3 desiredVelocity;

        //返回操控向量
        if (IsPlanar)
            toTarget.y = 0;

        float distance = toTarget.magnitude;
        //如果距离大于减速半径
        if (distance > SlowDownDistance)
        {
            //预期速度是AI与目标的距离
            desiredVelocity = toTarget.normalized * maxSpeed;
            return desiredVelocity - vehicle.Velocity;

        }
        else
        {
            //计算预期速度 并返回预期速度与当前速度的差
            desiredVelocity = toTarget - vehicle.Velocity;
            //返回预期速度与当前速度的差
            return desiredVelocity - vehicle.Velocity;
        }
    }


    public void OnDrawGizmos()
    {
        if(Target!=null)
        Gizmos.DrawWireSphere(Target.transform.position, SlowDownDistance);
    }
}

