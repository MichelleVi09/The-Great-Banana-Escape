using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarAIPath : MonoBehaviour
{
    [Header("Path")]
    public WaypointPath path; //which path to follow
    public float waypointThreshold = 1f;

    [Header("Speed")]
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 10f;

    [Header("Obstacle detection")]
    public float rayDistance = 5f;
    public LayerMask obstacleLayers;

    [Header("End of path behavior")]
    public bool teleportToStartOnEnd = true;

    private Transform[] waypoints;
    private int currentIndex = 0;
    private float currentSpeed = 0f;

    //store original psoition of car at beginning
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private int initialIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (path == null || path.waypoints == null || path.waypoints.Length == 0)
        {
            Debug.LogWarning("No path assigned or path has no waypoints");
            enabled = false;
            return;
        }
        waypoints = path.waypoints;
        //find closest waypoint
        currentIndex = FindClosestWaypointIndex();

        //save car's starting position
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialIndex = currentIndex;
    }
    int FindClosestWaypointIndex()
    {
        int closest = 0;
        float closestSqrDist = Mathf.Infinity;
        Vector3 pos = transform.position;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 diff = waypoints[i].position - pos;
            diff.y = 0f;
            float sqrDist = diff.sqrMagnitude;
            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closest = i;
            }

        }
        return closest;

    }


    // Update is called once per frame
    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) { return; }
        Transform target = waypoints[currentIndex];

        Vector3 toTarget = target.position - transform.position;
        Vector3 flatToTarget = new Vector3(toTarget.x, 0f, toTarget.z);
        float distanceToTarget = flatToTarget.magnitude;

        //rotating to target
        if (flatToTarget.sqrMagnitude > 0.001f)
        {
            Vector3 dir = flatToTarget.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        //raycast
        bool blocked = Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, out RaycastHit hit, rayDistance, obstacleLayers);
        if (blocked)
        {
            //slow down
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);

        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        }
        //move forward
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        //reaching the waypoint
        if (distanceToTarget < waypointThreshold)
        {
            if (currentIndex < waypoints.Length - 1)
            {
                currentIndex++;
            }
            else
            {
                if (teleportToStartOnEnd)
                {
                    teleportToStart();
                }
                else
                {
                    currentIndex = 0;
                }
            }
        }
    }
    void teleportToStart()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        transform.position = initialPosition;


        //reset speed and head to next waypont
        currentSpeed = 0f;
        currentIndex = initialIndex;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.up * 0.5f, transform.forward * rayDistance);
    }
}


