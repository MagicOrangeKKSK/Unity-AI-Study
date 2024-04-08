using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState 
{
    public PatrolState(Transform[] wp)
    {
        //接收巡逻点
        waypoints = wp;
        stateID = FSMStateID.Patrolling;
        curRotSpeed = 6.0f;
        curSpeed = 80f;
    }

    //这个方法定义了这个状态下AI角色的行为
    public override void Act(Transform player, Transform npc)
    {
        //如果以及到达了巡逻点 那么调用FindNextPoint函数  选择下一个巡逻点
        if (Vector3.Distance(npc.position, destPos) <= arriveDistance)
        {
            Debug.Log("Reached to the destination point\ncalculating the next point");
            FindNextPoint(); 
        }

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation,Time.deltaTime * curRotSpeed);

        //获得角色控制器组件 控制AI角色向前移动
        CharacterController controller = npc.GetComponent<CharacterController>();
        controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);

        //播放行走动画

    }

    //这个方法决定是否需要转换状态 以及发生哪种转换
    public override void Reason(Transform player, Transform npc)
    {
        float dist = Vector3.Distance(npc.position, player.position);
        //Debug.LogError($"dist:{dist}  ");
        if (dist <= chaseDistance)
        {
            Debug.Log("Switch to Chase State");
            //看见玩家
            npc.GetComponent<AIController>().SetTransition(Transition.SawPlayer);
        } 

    }

}
