using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public GameObject destinationObjectToMove;
    public GameObject destinationPrefab;
    //目标管理器的实例
    private static DestinationManager instance;
    public static DestinationManager Instance { get { return instance; } }
    //小队成员的所有目标点的列表
    public List<Destination> destinations;
    private List<Boid> boids = new List<Boid>();
    //目标点是否已经处于稳定位置
    private bool destinationsAreDoneMoving = false;
    //目标点是否已经赋值
    private bool destinationsAreAssigned = true;
    private Ray ray;
    private RaycastHit hitInfo;

    //目标圆的半径
    public float destCircleRadius = 1;
    private bool generateDestination = true;
    private Vector3[] offset;

    private void Awake()
    {
        instance = this;
        destinations = new List<Destination>();
        FindBoids(); //找到所有的AI
        offset = new Vector3[13];
        offset[0] = new Vector3(0, 0, 0);
        offset[1] = new Vector3(1, 0, 0);
        offset[2] = new Vector3(0.5f, 0, 0.87f);
        offset[3] = new Vector3(-0.5f, 0, 0.87f);
        offset[4] = new Vector3(-1, 0, 0);
        offset[5] = new Vector3(-0.5f, 0, -0.87f);
        offset[6] = new Vector3(0.5f, 0, -0.87f);
        offset[7] = new Vector3(0.87f, 0, 0.5f);
        offset[8] = new Vector3(0, 0, 1);
        offset[9] = new Vector3(-0.87f, 0, 0.5f);
        offset[10] = new Vector3(-0.87f, 0, -0.5f);
        offset[11] = new Vector3(0, 0, -1);
        offset[12] = new Vector3(0.87f, 0, -0.5f);
    }


    public void FindBoids()
    {
        boids.Clear();
        boids = FindObjectsOfType<Boid>().ToList();
    }

    //放置各个目标点
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin,ray.direction,out hitInfo))
            {
                //点击到了地板
                if(hitInfo.collider.gameObject.layer == 7)
                {
                    //放置各个目标点
                    placeDestination(hitInfo.point);
                }
                return;
            }
        }
        Debug.Log(destinations.Count);
        if (destinations.Count == 0)
        {
            return;
        }
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        //求出所有目标点的平均值
        foreach(Destination d in destinations)
        {
            center += d.transform.position;
        }
        Vector3 destinationCenter = center / destinations.Count;
        //如果所有目标点都已达到稳定状态 但是还没计算路径
        if(destinationsAreDoneMoving && !destinationsAreAssigned)
        {
            //调用AssignNode函数 为所有成员计算路径
            AssignNodes();
            //已发出计算路径请求
            destinationsAreAssigned = true;
            return;
        }

        int destinationsStopped = 0;
        //对于目标点列表中的每个目标点
        foreach(Destination d in destinations)
        {
            //调用destination脚本calculateForce函数计算操控力
            d.CalculateForce(destinationCenter);
            //将目标点的当前速度累加
            velocity += d.GetComponent<Rigidbody>().velocity;
            //如果当前目标点的速度小于一个阈值 那么可以认为它已经基本停止
            //将已停止的目标点数量+1
            if (d.Velocity < 1)
            {
                destinationsStopped++;
            }
        }
        //如果所有目标点的速度和 小于一个阈值
        //说明目标点已达到了稳定状态
        Vector3 destinationVelocity = velocity / destinations.Count;
        if(destinationsStopped == destinations.Count)
        {
            foreach (Destination dst in destinations)
            {
                dst.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            destinationsAreDoneMoving = true;
        }

    }

    private void AssignNodes()
    {
        Debug.Log(boids.Count);
        for(int i = 0; i < boids.Count; i++)
        {
            boids[i].CalculatePath();
        }
    }

    private void placeDestination(Vector3 point)
    {
        int index = 0;
        float radius = destCircleRadius;
        foreach (Boid b in boids)
        {
            //如果需要采用操控力的方式为它生成目标点
            if (generateDestination)
            {
                //在指定的位置上初始化目标点prefab
                GameObject des = Instantiate(destinationPrefab,
                    point + radius * offset[index++], Quaternion.identity);
                destinations.Add(des.GetComponent<Destination>());
                b.target = des;
            }
            else
            {
                b.target.transform.position = point +radius * offset[index++];
            }

            if (index > 12)
            {
                index = 1;
                radius *= 4;
            }
        }

        destinationsAreAssigned = false;
        generateDestination = false;
        destinationsAreDoneMoving = false;
    }
}
