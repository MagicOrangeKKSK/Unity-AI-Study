using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLimitedLifetime : Trigger
{
    //�ô������ĳ���ʱ��
    protected int lifetime;
    public override void Updateme()
    {
        lifetime--;
        if(lifetime <= 0)
        {
            toBeRemoved = true;
        }
    }

    private void Start()
    {
        base.Start();
    }

    private void Update()
    {
        
    }
}
