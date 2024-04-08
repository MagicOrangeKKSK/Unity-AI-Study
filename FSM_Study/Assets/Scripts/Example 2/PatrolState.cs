using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState 
{
    public PatrolState(Transform[] wp)
    {
        //����Ѳ�ߵ�
        waypoints = wp;
        stateID = FSMStateID.Patrolling;
        curRotSpeed = 6.0f;
        curSpeed = 80f;
    }

    //����������������״̬��AI��ɫ����Ϊ
    public override void Act(Transform player, Transform npc)
    {
        //����Լ�������Ѳ�ߵ� ��ô����FindNextPoint����  ѡ����һ��Ѳ�ߵ�
        if (Vector3.Distance(npc.position, destPos) <= arriveDistance)
        {
            Debug.Log("Reached to the destination point\ncalculating the next point");
            FindNextPoint(); 
        }

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation,Time.deltaTime * curRotSpeed);

        //��ý�ɫ��������� ����AI��ɫ��ǰ�ƶ�
        CharacterController controller = npc.GetComponent<CharacterController>();
        controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);

        //�������߶���

    }

    //������������Ƿ���Ҫת��״̬ �Լ���������ת��
    public override void Reason(Transform player, Transform npc)
    {
        float dist = Vector3.Distance(npc.position, player.position);
        //Debug.LogError($"dist:{dist}  ");
        if (dist <= chaseDistance)
        {
            Debug.Log("Switch to Chase State");
            //�������
            npc.GetComponent<AIController>().SetTransition(Transition.SawPlayer);
        } 

    }

}
