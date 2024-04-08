using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForEvada : Steering
{
    public GameObject target;
    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;
    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
    }

    public override Vector3 Force()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        
        //向前预测
        Vector3 tVelocity = target.GetComponent<Vehicle>().Velocity;
        float lookaheadTime =toTarget.magnitude / (maxSpeed + tVelocity.magnitude);
        //计算预期速度
        desiredVelocity = (transform.position - 
            (target.transform.position + tVelocity * lookaheadTime))
            .normalized * maxSpeed;
        return (desiredVelocity - vehicle.Velocity);
    }

   
}
