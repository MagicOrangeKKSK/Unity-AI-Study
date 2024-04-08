using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForFlee : Steering
{
    /// <summary>
    /// 要需要的目标物体
    /// </summary>
    public GameObject Target;

    /// <summary>
    /// 设置使AI角色感到危险 并 逃跑的范围
    /// </summary>
    public float FearDistance = 20;
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
        Vector3 tmpPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 tmpTargetPos = new Vector3(Target.transform.position.x, 0, Target.transform.position.z);
        if (Vector3.Distance(tmpPos, tmpTargetPos) > FearDistance)
            return new Vector3(0, 0, 0);

        desiredVelocity = (transform.position - Target.transform.position).normalized * maxSpeed;
        return desiredVelocity - vehicle.Velocity;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, FearDistance);
    }
}
