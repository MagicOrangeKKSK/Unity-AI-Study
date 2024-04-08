using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    //碰撞体数组
    private Collider[] colliders;

    //计时器
    private float timer = 0;
    //邻居列表
    public List<GameObject> neighbors;
    //设置检测时间间隔
    public float checkInterval = 0.3f;
    //领域半径
    public float detectRadius = 10f;
    //设置检测的层级
    public LayerMask layersChecked;

    private void Start()
    {
        neighbors = new List<GameObject>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > checkInterval)
        {
            //清空邻居列表
            neighbors.Clear();
            //查找当前AI角色领域内的所有碰撞体
            colliders = Physics.OverlapSphere(transform.position, detectRadius, layersChecked);
            //放入另据列表当中
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Vehicle>())
                {
                    neighbors.Add(colliders[i].gameObject);
                }
            }
            //计时器归零
            timer = 0;
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

}
