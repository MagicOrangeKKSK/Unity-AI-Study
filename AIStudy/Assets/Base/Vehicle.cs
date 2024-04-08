using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 交通工具类
/// </summary>
public class Vehicle : MonoBehaviour
{
    /// <summary>
    /// 最大速度
    /// </summary>
    public float MaxSpeed = 10;
    /// <summary>
    /// 可被施加的最大力
    /// </summary>
    public float MaxForce = 100;

    /// <summary>
    /// AI角色的质量
    /// </summary>
    public float Mass = 1;

    //最大速度的平方 预先计算并储存 节省资源
    protected float sqrMaxSpeed;

    /// <summary>
    /// 速度
    /// </summary>
    public Vector3 Velocity;

    /// <summary>
    /// 这个角色包含的操控行为列表
    /// </summary>
    private Steering[] steerings;

    /// <summary>
    /// 计算得到的操控力
    /// </summary>
    private Vector3 steeringForce;

    /// <summary>
    /// 加速度
    /// </summary>
    protected Vector3 acceleration;

    /// <summary>
    /// 是否在平面上 如果是则忽略Y轴
    /// </summary>
    public bool IsPlanar;

    /// <summary>
    /// 控制转向的速度
    /// </summary>
    public float damping = 0.9f;

    private float timer;
    //计算操控力的时间间隔 操控力 不需要每帧更新
    public float ComputeInterval = 0.2f;

    protected void Start()
    {
        steeringForce = Vector3.zero;
        steerings = GetComponents<Steering>();
        timer = 0;
        sqrMaxSpeed = MaxSpeed * MaxSpeed;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        steeringForce = new Vector3(0, 0, 0);
        if (timer > ComputeInterval)
        {
            foreach(Steering s in steerings)
            {
                if (s.enabled)
                {
                    steeringForce += s.Force() * s.Weight;
                }
            }
            //使操控力 不大于MaxForce
            steeringForce = Vector3.ClampMagnitude(steeringForce, MaxForce);
            //力除以质量 求出加速度
            acceleration = steeringForce / Mass;
            //重新计时
            timer = 0;
        }
    }

    public void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + acceleration.normalized * 3f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward*3f);
    }
}
