using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForCohesion : Steering
{
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
        //操控力
        Vector3 steeringForce = Vector3.zero;
        Vector3 centerOfMass = Vector3.zero;//质心
        int neighborCount = 0;

        foreach (GameObject s in GetComponent<Radar>().neighbors)
        {
            if (s != null && s != gameObject)
            {
                centerOfMass += s.transform.position;
                neighborCount++;
            }
        }
        if (neighborCount > 0)
        {
            centerOfMass /= (float)neighborCount;
            desiredVelocity = (centerOfMass - transform.position).normalized * maxSpeed;
            steeringForce = desiredVelocity - vehicle.Velocity;
        }
        return steeringForce;
    }
}
