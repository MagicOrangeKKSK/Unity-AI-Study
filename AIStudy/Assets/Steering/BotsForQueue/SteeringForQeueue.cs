using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForQeueue : Steering
{
    public float MAX_QUEUE_AHEAD;
    public float MAX_QUEUE_RADIUS;
    private Collider[] colliders;
    public LayerMask layersChecked;
    private Vehicle vehicle;
    private int layerid;
    private LayerMask layerMask;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        //设置碰撞检测时的层级
        layerid = LayerMask.NameToLayer("vehicles");
        layerMask = 1 << layerid;
    }

    public override Vector3 Force()
    {
        Vector3 velocity = vehicle.Velocity;
        Vector3 normalizedVelocity = velocity.normalized;
        //计算前方的一点
        Vector3 ahead = transform.position + normalizedVelocity * MAX_QUEUE_AHEAD;
        //以ahead为中心，MAX_QUEUE_RADIUS的球体内有其他角色
        colliders = Physics.OverlapSphere(ahead, MAX_QUEUE_RADIUS, layerMask);
        if (colliders.Length > 0)
        {
            foreach(Collider c in colliders)
            {
                if((c.gameObject!=gameObject) &&
                    (c.gameObject.GetComponent<Vehicle>().Velocity.magnitude 
                    < velocity.magnitude))
                {
                    vehicle.Velocity *= 0.8f;
                    break;
                }
            }
        }

        return base.Force();
    }
}
