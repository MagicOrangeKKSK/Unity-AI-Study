using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForWander : Steering
{
    //徘徊半径
    public float wanderRadius;
    //徘徊距离
    public float wanderDistance;
    //每秒加到目标的随机位移的最大值
    public float wanderJitter;

    public bool isPlanar;

    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;
    private Vector3 circleTarget;
    private Vector3 wanderTarget;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        isPlanar = vehicle.IsPlanar;
        //选取圆圈上的一个点作为初始点
        circleTarget = new Vector3(wanderRadius * 0.707f, 0, wanderRadius * 0.707f);
    }

    public override Vector3 Force()
    {
        Vector3 randomDisplacement = new Vector3((Random.value - 0.5f) * 2 * wanderJitter, (Random.value - 0.5f) * 2 * wanderJitter, (Random.value - 0.5f) * 2 * wanderJitter);
        if (isPlanar)
        {
            randomDisplacement.y = 0;
        }
        //将随机位移加初始点上 得到新的位置
        circleTarget += randomDisplacement;
        //新位置可能不在圆周上 因此需要投影到圆周上
        circleTarget = wanderDistance * circleTarget.normalized;
        //计算出来的值是相对于AI的前进方向 需要转换为世界坐标
        wanderTarget = vehicle.Velocity.normalized * wanderDistance + circleTarget + transform.position;
        //计算速度 返回操控力
        desiredVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
        return (desiredVelocity - vehicle.Velocity);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, wanderTarget);
        Gizmos.DrawWireSphere(wanderTarget, wanderRadius);
    }
}
