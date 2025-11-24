using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICar : MonoBehaviour
{
    public float speed;
    public int MaxNum;
    int pointIndex;
    Transform movePoint;
    public Transform[] points;

    void Start()
    {
        pointIndex = 0;
        movePoint = points[pointIndex];
    }

    void Update()
    {
   
        transform.position = Vector3.MoveTowards(
            transform.position,
            movePoint.position,
            speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) < 0.1f)
        {
            pointIndex++;

            if (pointIndex > MaxNum)
                pointIndex = 0;

            movePoint = points[pointIndex];
        }

        //face next point
        Vector3 dir = movePoint.position - transform.position;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }
  


}

