using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    //��ҵ�transform���
    protected Transform playerTransform;

    //��һ��Ѳ�ߵ����ҵ�λ�� ȡ���ڵ�ǰ��״̬
    protected Vector3 destPos;

    //Ѳ�ߵ������
    protected GameObject[] pointList;

    protected float shootRate; //�ӵ��������
    protected float elapsedTime; //������һ�������ʱ��

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    private void Start()
    {
        Initialize(); //����FSM��ʼ��
    }

    private void Update()
    {
        FSMUpdate(); //ÿ֡����fsm
    }

    private void FixedUpdate()
    {
        //�Թ̶���ʱ�����ڸ���FSM
        FSMFixedUpdate();
    }


}
