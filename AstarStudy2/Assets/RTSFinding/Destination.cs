using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    //�ۼ��ٿ�����Ȩ��
    public int coherencyWeight = 1;
    //����ٿ�����Ȩ��
    public int separatonWeight = 2;
    //С���и���AI��ɫ��Ѱ·Ŀ���ļ���
    private List<Destination> destinations;

    private float velocity;
    public float Velocity => velocity;

    //�ۼ��ٿ���
    private Vector3 coherency;
    //����ٿ���
    private Vector3 separaton;
    private Vector3 calculatedForce;
    private Vector3 relativePos;

    private void Start()
    {
        //Ŀ����������ʵ��
        destinations = DestinationManager.Instance.destinations;
    }

    //����ٿ��� �޸��ƶ�Ŀ���
    public void CalculateForce(Vector3 center)
    {
        //��������Ŀ�������� �Ե�ǰĿ���ʩ�ӵľۼ���
        coherency = center - transform.position;
        separaton = Vector3.zero;
        foreach(Destination d in destinations)
        {
            //������ǵ�ǰĿ��㣬��ô������Ե�ǰĿ�������ķ�������������
            if (d != this)
            {
                relativePos = transform.position - d.transform.position;
                separaton += relativePos / (relativePos.sqrMagnitude);
            }
        }
        //�����Ȩ��   �õ��ܵĲٿ�����
        calculatedForce = (coherency*coherencyWeight) + (separaton*separatonWeight);
        calculatedForce.y = 0;
        //�ƶ�Ŀ���
        transform.GetComponent<Rigidbody>().velocity = calculatedForce * 20;
        velocity = transform.GetComponent<Rigidbody>().velocity.magnitude;
    }
}
