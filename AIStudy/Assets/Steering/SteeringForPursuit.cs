using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 追逐目标
/// </summary>
public class SteeringForPursuit : Steering
{
    public GameObject target;
    private Vector3 desiredVelocity;//预期速度
    private Vehicle vehicle;
    private float maxSpeed; //最大速度

    public void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
    }

    public override Vector3 Force()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        //计算追逐者的前向与逃避者前向之间的夹角
        float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);
        //如果夹角大于0 且追逐者基本面对逃避者
        //那么直接向逃避者方向前进
        if ((Vector3.Dot(toTarget, transform.forward) > 0) && relativeDirection < -0.95f)
        {
            //计算预期速度
            desiredVelocity = (target.transform.position - transform.position)
                .normalized * maxSpeed;
            return desiredVelocity - vehicle.Velocity;
        }
        //计算预测时间，正比于追逐者与逃避者的距离 反比于追逐者与逃避者的速度和
        float lookaheadTime = toTarget.magnitude /
            (maxSpeed + target.GetComponent<Vehicle>().Velocity.magnitude);
        //计算预期速度
        desiredVelocity = (target.transform.position + target.GetComponent<Vehicle>().Velocity
            * lookaheadTime - transform.position).normalized * maxSpeed;
        return desiredVelocity - vehicle.Velocity;
    }


    public void OnDrawGizmos()
    {
        if (target != null)
        {
            Vector3 toTarget = target.transform.position - transform.position;
            float lookaheadTime = toTarget.magnitude /
           (maxSpeed + target.GetComponent<Vehicle>().Velocity.magnitude);
            Vector3 desiredVelocity = (target.transform.position + target.GetComponent<Vehicle>().Velocity
            * lookaheadTime);
            Gizmos.DrawLine(transform.position, desiredVelocity);
        }
    }
}
