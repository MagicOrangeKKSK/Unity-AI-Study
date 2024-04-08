using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThread : MonoBehaviour
{
    public static Queue<Action> jobs = new Queue<Action>();

    private void Update()
    {
        while(jobs.Count > 0)
        {
            jobs.Dequeue().Invoke();
        }
    }

    public static void AddJob(Action newjob) 
    {
       jobs.Enqueue(newjob);
    }

}
