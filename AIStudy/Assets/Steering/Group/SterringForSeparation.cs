using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SterringForSeparation : Steering
{
    //可接受的距离
    public float comfortDistance = 1;
    //当AI角色与邻居过近时的惩罚倍数
    public float multiplierInsideComfortDistance = 2;

    private void Start()
    {
        
    }

    public override Vector3 Force()
    {
        Vector3 steeringForce = new Vector3(0, 0, 0);
        //遍历这个AI角色的邻居列表
        foreach(GameObject s in GetComponent<Radar>().neighbors)
        {
            if(s!=null && s != gameObject)
            {
                //计算距离
                Vector3 toNeighbor = transform.position - s.transform.position;
                float length = toNeighbor.magnitude;
                steeringForce += toNeighbor.normalized / length; //排斥力 大小与距离成反比
                if(length < comfortDistance)
                {
                    steeringForce *= multiplierInsideComfortDistance;
                }

            }
        }
        return steeringForce;
    }
}
