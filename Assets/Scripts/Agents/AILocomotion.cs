using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class AILocomotion : MonoBehaviour
{
    private Animator animator;
    public Transform playerTransform;
    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //setting agent's position to player
        agent.destination = playerTransform.position;
        animator.SetFloat(SpeedParam, agent.velocity.magnitude);
        
    }
}
