using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    //���һ�θ�֪�����ʱ ��ҵ�λ��
    public Vector3 playerLastPosition;
    //��û�и�֪�����ʱ ���õ�λ��
    public Vector3 resetPosition;

    //�ϴθ��������Ϣ��ʱ��
    public float lastSensedTime = 0;
    //�������λ��ǰ�ȴ���ʱ��
    public float resetTime = 1.0f;

    private void Start()
    {
        playerLastPosition = new Vector3(100, 100, 100);
        resetPosition = new Vector3(100, 100, 100);

    }

    private void Update()
    {
        if(Time.time - lastSensedTime > resetTime)
        {
            playerLastPosition = resetPosition;
        }
    }
}
