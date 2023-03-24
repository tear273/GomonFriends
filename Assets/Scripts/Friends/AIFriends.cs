using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum AIAnim
{
    Idle,
    Jump,
    LightaFire,
    sit,
    sitNSlip,
    Walk
}

public class AIFriends : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;

    [SerializeField]
    Animator animator;

    [SerializeField]
    NavMeshAgent nav;

    int animState = Animator.StringToHash("animState");
    int walkEnd = Animator.StringToHash("walkEnd");

    float minWaiteTime = 3;
    float maxWaiteTime = 5;

    [SerializeField]
    float waiteTime = 3;

    [SerializeField]
    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if(time > waiteTime && animator.GetBool(walkEnd))
        {
            waiteTime = Random.Range(minWaiteTime, maxWaiteTime);
            time = 0;

            

            Vector3 pos = transform.position;
            pos.x += Random.Range(-3.0f, 3.0f);
            pos.z += Random.Range(-3.0f, 3.0f);
            
            nav.SetDestination(pos);
            
        }


        if (nav.remainingDistance - 1 > nav.stoppingDistance)
        {
            animator.SetInteger(animState, 5);
            controller.Move(nav.velocity * Time.deltaTime);
            animator.SetBool(walkEnd, false);
        }
        else
        {
            controller.Move(Vector3.zero);
            animator.SetBool(walkEnd, true);
            animator.SetInteger(animState, 0);
        }




    }
}
