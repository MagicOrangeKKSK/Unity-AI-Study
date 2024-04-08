using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRange : MonoBehaviour
{
    public float fireRange; //玩家的火力范围
    public int penalty;//在火力范围的路点 需要增加代价
    public float fieldOfAttack = 45f;//火力攻击的一个角度

    private void OnDrawGizmos()
    {
        float fieldOfAttackInRadians = fieldOfAttack * 3.14f / 180f;
        for (int i = 0; i < 11; i++)
        {
            float angle = -fieldOfAttackInRadians + fieldOfAttackInRadians * 0.2f * (float)i;
            Vector3 rayPoint = transform.TransformPoint(new Vector3(fireRange * Mathf.Sin(angle),
                0, fireRange * Mathf.Cos(angle)));

            Vector3 rayDirection = rayPoint - transform.position;

            if(Physics.Raycast(transform.position,rayDirection,out RaycastHit hit,fireRange))
            {
                if(LayerMask.LayerToName(hit.transform.gameObject.layer)== "Obstables")
                {
                    Debug.DrawLine(transform.position + new Vector3(0, 1, 0),
                                   hit.point + new Vector3(0, 1, 0), Color.red);
                    continue;
                }
            }
            Debug.DrawLine(transform.position + new Vector3(0, 1, 0),
                                rayPoint + new Vector3(0, 1, 0), Color.red);
        }
    }
}
