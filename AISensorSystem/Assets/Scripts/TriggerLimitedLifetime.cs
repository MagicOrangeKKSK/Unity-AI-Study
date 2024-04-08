using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLimitedLifetime : Trigger
{
    //该触发器的持续时间
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
