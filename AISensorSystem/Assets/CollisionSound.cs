using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip collisionSound;

    private AudioSource audio;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(collisionSound);
    }
}
