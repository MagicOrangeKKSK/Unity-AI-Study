using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition  //ת������
{
    SawPlayer = 0,//�������
    ReachPlayer , //�ӽ����
    LostPlayer , //����뿪����
    NoHealth,//����

}

public enum FSMStateID  //״̬
{
   Patrolling = 0,   //Ѳ�ߵ�״̬���0
   Chasing,          //׷���״̬���1
   Attacking,        //������״̬���2
   Dead,             //������״̬���3
}

