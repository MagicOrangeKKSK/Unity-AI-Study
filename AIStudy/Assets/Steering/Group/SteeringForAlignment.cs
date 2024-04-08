using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForAlignment : Steering
{
    public override Vector3 Force()
    {
        Vector3 averageDirection = new Vector3();//当前AI角色的邻居的平均朝向
        int neighborCount = 0;//邻居的数量
        //遍历这个AI角色的邻居列表
        foreach (GameObject s in GetComponent<Radar>().neighbors)
        {
            if (s != null && s != gameObject)
            {
                averageDirection += s.transform.forward;
                neighborCount++;
            }
        }
        if (neighborCount > 0)
        {
            averageDirection /= (float)neighborCount;
            averageDirection -= transform.forward;
        }
        return averageDirection;
    }

}
