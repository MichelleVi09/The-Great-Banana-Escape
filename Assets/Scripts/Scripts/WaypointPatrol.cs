using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CreatureMover))]
    public class WaypointPatrol : MonoBehaviour
    {
        [Header("Patrol Settings")]
        public Transform[] waypoints;
        public float waitTimeAtWaypoint = 2f;
        public float waypointTolerance = 0.5f;
        public bool loop = true;

        private CreatureMover mover;
        private int currentWaypoint = 0;
        private float waitTimer = 0f;

        private void Awake()
        {
            mover = GetComponent<CreatureMover>();
        }

        private void Update()
        {
            if (waypoints == null || waypoints.Length == 0)
                return;

            Vector3 targetPos = waypoints[currentWaypoint].position;
            Vector3 direction = (targetPos - transform.position);
            direction.y = 0f; 

            //Move toward the waypoint
            if (direction.magnitude > waypointTolerance)
            {
                Vector2 moveAxis = new Vector2(0, 1); //forward movement
                mover.SetInput(in moveAxis, in targetPos, false, false); //use CreatureMover movement
            }
            else
            {
                mover.SetInput(Vector2.zero, transform.position, false, false);

                //Wait, then go to the next waypoint
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTimeAtWaypoint)
                {
                    waitTimer = 0f;
                    currentWaypoint++;

                    if (currentWaypoint >= waypoints.Length)
                    {
                        if (loop)
                            currentWaypoint = 0;
                        else
                            enabled = false; //stop if not looping
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (waypoints == null || waypoints.Length == 0)
                return;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (i + 1 < waypoints.Length)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}

