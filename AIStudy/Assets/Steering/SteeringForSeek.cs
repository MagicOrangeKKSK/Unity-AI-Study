using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForSeek : Steering
{
    /// <summary>
    /// 要需要的目标物体
    /// </summary>
    public GameObject Target;

    //预期速度
    private Vector3 desiredVelocity;

    private Vehicle vehicle;
    private float maxSpeed;
    private bool isPlanar;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        isPlanar = vehicle.IsPlanar;
    }

    public override Vector3 Force()
    {
        desiredVelocity = (Target.transform.position - transform.position).normalized * maxSpeed;
        if (isPlanar)
        {
            desiredVelocity.y = 0;
        }
        //返回操控向量 
        //也就是预期速度合当前速度的差
        return desiredVelocity - vehicle.Velocity;
    }
}
