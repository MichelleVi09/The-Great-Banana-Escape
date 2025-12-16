using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform[] waypoints;
    void OnValidate()
    {
        int count = transform.childCount;
        waypoints = new Transform[count];
        for (int i = 0; i < count; i++) 
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
}
