using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    [Header("Targets")]
    public Transform lookAt;  //where to look at 
    public Transform Player;  

    [Header("Camera Settings")]
    public float distance = 10.0f;   //max camera distance
    public float minDistance = 1.5f; //min camera distance (when close to walls)
    public float sensivity = 15.0f;
    public float smooth = 10f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float currentDistance;   //dynamic distance

    void Start()
    {
        currentDistance = distance;
    }

    void LateUpdate()
    {
        if (!lookAt) return;

        //handles mouse orbit
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, YMin, YMax);

        //base rotation and desired camera position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredDir = rotation * Vector3.back;
        Vector3 desiredPosition = lookAt.position + desiredDir * distance;

        //handles collisions 
        RaycastHit hit;
        if (Physics.Linecast(lookAt.position, desiredPosition, out hit))
        {
            //moves camera closer if player collides with something
            float adjustedDist = Mathf.Clamp(hit.distance * 0.9f, minDistance, distance);
            currentDistance = Mathf.Lerp(currentDistance, adjustedDist, Time.deltaTime * smooth);
        }
        else
        {
            //will return camera back to normal
            currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * smooth);
        }

        //applies position and rotation 
        transform.position = lookAt.position + desiredDir * currentDistance;
        transform.LookAt(lookAt.position);
    }
}

