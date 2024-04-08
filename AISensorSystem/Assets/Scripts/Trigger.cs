using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    //����������Ķ���
    protected TriggerSystemManager manager;
    //��������λ��
    protected Vector3 position;
    //�������뾶
    public int radius;
    public bool toBeRemoved;
    //��鴥�����Ƿ��ܱ���֪��s�о���  ����� ��ô��ȡ��Ӧ����Ϊ
    public virtual void Try(Sensor sensor)
    {

    }

    //���´��������ڲ�״̬ ����������������ʣ����Чʱ��
    public virtual void Updateme()
    {

    }

    //��鴥�����Ƿ��ܱ���֪��s�о���  
    protected virtual bool isTouchingTrigger(Sensor sensor)
    {
        return false;
    }

    private void Awake()
    {
        //���ҹ�����������
        manager = FindObjectOfType<TriggerSystemManager>();
    }

    protected void Start()
    {
        toBeRemoved = false;
    }

    private void Update()
    {
        
    }
}
