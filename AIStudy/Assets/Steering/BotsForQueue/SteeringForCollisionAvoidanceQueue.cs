using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForCollisionAvoidanceQueue : Steering
{
    public bool isPlanar;
    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;
    private float maxForce;
    public float avoidanceForce;
    public float MAX_SEE_AHEAD;
    private GameObject[] allColliders;
    private int layerid;
    private LayerMask layerMask;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        maxForce = vehicle.MaxForce;
        isPlanar = vehicle.IsPlanar;
        if (avoidanceForce > maxForce)
            avoidanceForce = maxForce;
        allColliders = GameObject.FindGameObjectsWithTag("obstacle");
        layerid = LayerMask.NameToLayer("obstacle");
        layerMask = 1 << layerid;
    }

    public override Vector3 Force()
    {
        RaycastHit hit;
        Vector3 force = new Vector3(0, 0, 0);
        Vector3 velocity = vehicle.Velocity;
        Vector3 normalizedVelocity = velocity.normalized;
        if(Physics.Raycast(transform.position,normalizedVelocity,
            out hit, MAX_SEE_AHEAD, layerMask))
        {
            Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD;
            force = ahead - hit.collider.transform.position;
            force *= avoidanceForce;
            if (isPlanar)
                force.y = 0;
        }
        return force;
    }
}
