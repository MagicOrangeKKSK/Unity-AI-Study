using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public GameObject destinationObjectToMove;
    public GameObject destinationPrefab;
    //Ŀ���������ʵ��
    private static DestinationManager instance;
    public static DestinationManager Instance { get { return instance; } }
    //С�ӳ�Ա������Ŀ�����б�
    public List<Destination> destinations;
    private List<Boid> boids = new List<Boid>();
    //Ŀ����Ƿ��Ѿ������ȶ�λ��
    private bool destinationsAreDoneMoving = false;
    //Ŀ����Ƿ��Ѿ���ֵ
    private bool destinationsAreAssigned = true;
    private Ray ray;
    private RaycastHit hitInfo;

    //Ŀ��Բ�İ뾶
    public float destCircleRadius = 1;
    private bool generateDestination = true;
    private Vector3[] offset;

    private void Awake()
    {
        instance = this;
        destinations = new List<Destination>();
        FindBoids(); //�ҵ����е�AI
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

    //���ø���Ŀ���
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin,ray.direction,out hitInfo))
            {
                //������˵ذ�
                if(hitInfo.collider.gameObject.layer == 7)
                {
                    //���ø���Ŀ���
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
        //�������Ŀ����ƽ��ֵ
        foreach(Destination d in destinations)
        {
            center += d.transform.position;
        }
        Vector3 destinationCenter = center / destinations.Count;
        //�������Ŀ��㶼�Ѵﵽ�ȶ�״̬ ���ǻ�û����·��
        if(destinationsAreDoneMoving && !destinationsAreAssigned)
        {
            //����AssignNode���� Ϊ���г�Ա����·��
            AssignNodes();
            //�ѷ�������·������
            destinationsAreAssigned = true;
            return;
        }

        int destinationsStopped = 0;
        //����Ŀ����б��е�ÿ��Ŀ���
        foreach(Destination d in destinations)
        {
            //����destination�ű�calculateForce��������ٿ���
            d.CalculateForce(destinationCenter);
            //��Ŀ���ĵ�ǰ�ٶ��ۼ�
            velocity += d.GetComponent<Rigidbody>().velocity;
            //�����ǰĿ�����ٶ�С��һ����ֵ ��ô������Ϊ���Ѿ�����ֹͣ
            //����ֹͣ��Ŀ�������+1
            if (d.Velocity < 1)
            {
                destinationsStopped++;
            }
        }
        //�������Ŀ�����ٶȺ� С��һ����ֵ
        //˵��Ŀ����Ѵﵽ���ȶ�״̬
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
            //�����Ҫ���òٿ����ķ�ʽΪ������Ŀ���
            if (generateDestination)
            {
                //��ָ����λ���ϳ�ʼ��Ŀ���prefab
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
