using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{

    public ChaseState(Transform[] wp)
    {
        waypoints = wp;
        //״̬���
        stateID = FSMStateID.Chasing;

        //����ת���ٶ����ƶ��ٶ�
        curRotSpeed = 6.0f;
        curSpeed = 160.0f;

        //��Ѳ�ߵ����������ѡ��һ�� 
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        destPos = player.position;

        //�������ҵľ���
        //���С�ڹ������� ��ôת�Ƶ�����״̬
        float dist = Vector3.Distance(npc.position, destPos);

        //Debug.LogError($"dist:{dist} attackDistance:{attackDistance}");
        if (dist <= attackDistance)
        {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<AIController>().SetTransition(Transition.ReachPlayer);
        }
        //�������Ҿ��볬��׷����� ��ô�ص�Ѳ��״̬
        else if (dist >= chaseDistance)
        {
            Debug.Log("Switch to Patrol State");
            npc.GetComponent<AIController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //�����λ������ΪĿ���
        destPos = player.position;
        //ת��Ŀ���
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        CharacterController controller = npc.GetComponent<CharacterController>();
        controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);

        //���ű��ܶ���

    }

}
