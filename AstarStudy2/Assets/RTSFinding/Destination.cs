using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    //聚集操控力的权重
    public int coherencyWeight = 1;
    //分离操控力的权重
    public int separatonWeight = 2;
    //小队中各个AI角色的寻路目标点的集合
    private List<Destination> destinations;

    private float velocity;
    public float Velocity => velocity;

    //聚集操控力
    private Vector3 coherency;
    //分离操控力
    private Vector3 separaton;
    private Vector3 calculatedForce;
    private Vector3 relativePos;

    private void Start()
    {
        //目标点管理器的实例
        destinations = DestinationManager.Instance.destinations;
    }

    //计算操控力 修改移动目标点
    public void CalculateForce(Vector3 center)
    {
        //计算所有目标点的中心 对当前目标点施加的聚集力
        coherency = center - transform.position;
        separaton = Vector3.zero;
        foreach(Destination d in destinations)
        {
            //如果不是当前目标点，那么求出它对当前目标点产生的分离力（斥力）
            if (d != this)
            {
                relativePos = transform.position - d.transform.position;
                separaton += relativePos / (relativePos.sqrMagnitude);
            }
        }
        //求出加权和   得到总的操控向量
        calculatedForce = (coherency*coherencyWeight) + (separaton*separatonWeight);
        calculatedForce.y = 0;
        //移动目标点
        transform.GetComponent<Rigidbody>().velocity = calculatedForce * 20;
        velocity = transform.GetComponent<Rigidbody>().velocity.magnitude;
    }
}
