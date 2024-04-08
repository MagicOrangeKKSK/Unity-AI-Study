using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILocomotion : Vehicle
{
    //AI角色的角色控制器
    private CharacterController controller;
    //AI角色的Rigidbody
    private Rigidbody rigidbody;
    //AI角色的每次移动距离
    private Vector3 moveDistance;


     void Start()
    {
        //获取角色控制器
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        moveDistance = Vector3.zero;

        base.Start();
    }


    //物理操作在FixedUpdate中更新
    private void FixedUpdate()
    {
        //计算速度
        Velocity += acceleration * Time.fixedDeltaTime;
        //限制速度 要低于最大速度
        if(Velocity.sqrMagnitude > sqrMaxSpeed)
        {
            Velocity = Velocity.normalized * MaxSpeed;
        }

        if(acceleration.sqrMagnitude == 0)
        {
            Velocity *= 0.9f;
        }

        //计算AI角色的移动距离
        moveDistance = Velocity * Time.fixedDeltaTime;
        //如果要求AI角色在平面上移动 那么就将Y设置为0
        if (IsPlanar)
        {
            Velocity.y = 0;
            moveDistance.y = 0;
        }

        //如果有角色控制器就用角色控制器来移动
        if (controller)
        {
            controller.SimpleMove(Velocity);
        }
        //如果没有 就用RigidBody移动
        //如果有 但是要求用动力学的方式操控
        else if (rigidbody == null || rigidbody.isKinematic)
        {
            transform.position += moveDistance;
        }
        else
        {
            rigidbody.MovePosition(rigidbody.position + moveDistance);
        }


        //更新朝向 如果速度大于一个值（为了防止抖动）
        //position == position
        if(Velocity.sqrMagnitude > 0.00001)
        {
            //通过当前朝向与速度的方向插值 计算新的朝向
            Vector3 newForward = Vector3.Slerp(transform.forward, Velocity, damping * Time.deltaTime);
            if (IsPlanar)
            {
                newForward.y = 0;
            }

            transform.forward = newForward;
        }

        ////播放一下行走动画
        //gameObject.animation.Play("walk");
        
    }

}
