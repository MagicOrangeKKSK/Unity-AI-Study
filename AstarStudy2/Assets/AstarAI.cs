using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame
{
    public class AstarAI : MonoBehaviour
    {
        //Ŀ��λ��
        public Transform target;

        private Seeker seeker;
        private Path path;

        //��ɫ��ÿ���ٶ�
        public float speed = 30f;
        //����ɫ�뵼���ڵ�С���������ʱ����ɫ��ת����һ���ڵ�
        public float nextWaypointDistance = 3f;
        //��ɫ��ǰ�ĵ������±�
        private int currentWaypoint = 0;

        private void Start()
        {
            seeker = GetComponent<Seeker>();
            seeker.pathCallback += OnPathComplete;

            Debug.Log(transform.position + " to " + target.position);
            seeker.StartPath(transform.position, target.position);
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        private void FixedUpdate()
        {
            if (path == null)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                Debug.Log("Ѱ·����");
                return;
            }
            //���ǰ������
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;
            transform.position += dir;

            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }

        private void OnDisable()
        {
            seeker.pathCallback -= OnPathComplete;
        }
    }
}