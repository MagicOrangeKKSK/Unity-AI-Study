using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //�ӵ���������
    public float LifeTime = 3f;
    //������ӵ����� ���ٵ�����ֵ
    public int damage = 50;
    //�ӵ���ǹ�ŵ��ٶ�
    public float beamVelocity = 100f;
    public Rigidbody rigidbody;

    public void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
    }

    public void Go()
    {
        rigidbody.AddForce(transform.forward * 10f, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(transform.forward * beamVelocity, ForceMode.Acceleration);
    }

    public void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
