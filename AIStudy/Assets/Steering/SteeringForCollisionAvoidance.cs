using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForCollisionAvoidance : Steering
{
    public bool isPlanar;
    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;
    private float maxForce;

    //避免障碍产生的操作力
    public float avoidanceForce;
    //能向前看到的最大距离
    public float MAX_SEE_AHEAD = 2.0f;
    //场景中所有碰撞体组成数组
    private GameObject[] allColliders;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        maxForce = vehicle.MaxForce;
        isPlanar = vehicle.IsPlanar;

        //如果避免操控力大于最大操控力 截断到最大操控力
        avoidanceForce = Mathf.Clamp(avoidanceForce, 0f, maxForce);
        allColliders = GameObject.FindGameObjectsWithTag("obstacle");
    }

    public override Vector3 Force()
    {
        RaycastHit hit;
        Vector3 force = new Vector3(0, 0, 0);
        Vector3 velocity = vehicle.Velocity;
        Vector3 normalizedVelocity = velocity.normalized;

        //画出一条射线
        Debug.DrawLine(transform.position, transform.position + normalizedVelocity * MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed));
        if (Physics.Raycast(transform.position, normalizedVelocity, out hit, MAX_SEE_AHEAD * velocity.magnitude / maxSpeed))
        {
            //如果射线与某个碰撞体相交
            Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed);
            force = ahead - hit.collider.transform.position;
            force *= avoidanceForce;
            if (isPlanar)
                force.y = 0;
            foreach (GameObject c in allColliders)
            {
                if (hit.collider.gameObject == c)
                {
                    c.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    c.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
        }
        else
        {
            foreach (GameObject c in allColliders)
            {
                c.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
        return force;
    }
}
