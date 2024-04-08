using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRespawning : Trigger
{
    //��һЩ��Ϸ�����ڱ�һ��ʵ�崥��֮�󣬻ᱣ��һ��ʱ��ķǻ״̬
    //����һЩ��ɫ���Լ�����ϵ�Ѫ������Ѫ������������ ����һ��ʱ���ڴ��ڷǻ״̬
    //֮�������±�Ϊ�����  ���Ա��ٴμ���

    //���λ�Ծ֮��ļ��ʱ��
    protected int numUpdatesBetweenRespawns;
    //�����´�������Ҫ�ȴ���ʱ��
    protected int numUpdatesRemainingUntilRespawn;

    protected bool isActive;

    protected void SetActive()
    {
        isActive = true; 
    }

    protected void SetInactive()
    {
        isActive = false;
    }

    protected void Deactivate()
    {
        SetInactive();
        numUpdatesRemainingUntilRespawn = numUpdatesBetweenRespawns;
    }

    public override void Updateme()
    {
        numUpdatesRemainingUntilRespawn--;
        if(numUpdatesRemainingUntilRespawn<0 && !isActive)
        {
            SetActive();
        }
    }

    protected void Start()
    {
        isActive = true;
        base.Start();
    }

    private void Update()
    {
        
    }
}

