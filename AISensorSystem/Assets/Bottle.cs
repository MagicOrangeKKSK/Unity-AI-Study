using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public GameObject collisionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ground")
        {
            Instantiate(collisionPrefab,transform.position,Quaternion.identity);
            Destroy(this);
        }
    }
}
