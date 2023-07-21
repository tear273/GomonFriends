using System;
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
    int sitWalk = Animator.StringToHash("sitWalk");

    float minWaiteTime = 3;
    float maxWaiteTime = 5;

    [SerializeField]
    float waiteTime = 3;

    [SerializeField]
    float time = 0;
    

    [SerializeField]
    int animNum = 0;

    AnimationClip clip;

    SitDeco currSit = null;

    private void OnEnable()
    {
        StartCoroutine(StartAnimator());
    }

    IEnumerator StartAnimator()
    {
        while (true)
        {
            time += Time.deltaTime;

            if (time > waiteTime && animator.GetBool(walkEnd) && animator.GetBool(sitWalk))
            {
                animNum = UnityEngine.Random.Range(1, 101);
                if (0 > animNum && 10 <= animNum)
                {
                    animNum = 4;
                }else if(10 > animNum && 50 >= animNum)
                {
                    animNum = 5; 
                }else if(50 > animNum && 70 >= animNum)
                {
                    animNum = 3;
                }
                else if (70 > animNum && 90 <= animNum)
                {
                    animNum = 1;
                }
                else
                {
                    animNum = 2;
                }

                int hh = int.Parse( DateTime.Now.ToString("HH"));

                if(hh > 18 || hh < 7)
                {
                    if(animNum == 3)
                    {
                        animNum = 1;
                    }
                }
                else
                {
                    if(animNum == 1)
                    {
                        animNum = 3;
                    }
                }
                
                time = 0;
                animator.SetInteger(animState, animNum);
                switch (animNum)
                {
                    case 0:
                        waiteTime = animator.GetCurrentAnimatorStateInfo(0).length / 2;
                        animator.SetBool(walkEnd, false);
                        break;
                    case 1:
                        waiteTime = animator.GetCurrentAnimatorStateInfo(0).length / 2;
                        for(int i=0; i<GameManager.Instance.SitDecos.Length; i++)
                        {
                            SitDeco friends = GameManager.Instance.SitDecos[i];
                            if (friends.Friends == null)
                            {
                                nav.destination = friends.transform.position;
                                friends.Friends = this;
                                currSit = friends;
                                animator.SetBool(walkEnd, false);
                                animator.SetBool(sitWalk, false);
                                break;
                            }
                        }

                        if(currSit == null)
                        {
                            time = 100;
                        }
                        
                        break;
                    case 2:
                        waiteTime = animator.GetCurrentAnimatorStateInfo(0).length / 2;
                        
                        animator.SetBool(walkEnd, false);
                        break;
                    case 3:
                        
                        waiteTime = 10;
                        for (int i = 0; i < GameManager.Instance.SitDecos.Length; i++)
                        {
                            SitDeco friends = GameManager.Instance.SitDecos[i];
                            if (friends.Friends == null)
                            {
                                nav.destination = friends.transform.position;
                                friends.Friends = this;
                                currSit = friends;
                                animator.SetBool(walkEnd, false);
                                animator.SetBool(sitWalk, false);
                                break;
                            }
                        }
                        
                        break;
                    case 4:
                        animator.SetBool(walkEnd, false);
                        waiteTime = 10;
                        break;
                    case 5:
                        waiteTime = 3;
                        animator.SetBool(walkEnd, false);
                        Vector3 pos = transform.position;
                        pos.x += UnityEngine.Random.Range(-3.0f, 3.0f);
                        pos.z += UnityEngine.Random.Range(-3.0f, 3.0f);
                        
                        nav.SetDestination(pos);
                        break;
                }

                yield return new WaitForSeconds(1);

            }

            switch (animNum)
            {
                case 1:
                    if (time > waiteTime)
                    {

                        int hh = int.Parse(DateTime.Now.ToString("HH"));

                        if (hh > 18 || hh < 7)
                        {
                            
                            animator.SetBool(walkEnd, false);
                        }
                        else
                        {
                            currSit.Friends = null;
                            currSit = null;
                            animator.SetBool(walkEnd, true);
                            Vector3 pos = transform.GetChild(0).transform.position;
                            pos.y = 0;
                            transform.GetChild(0).transform.position = pos;
                        }
                    }
                    if (nav.remainingDistance - 1 > nav.stoppingDistance)
                    {
                        controller.Move(nav.velocity * Time.deltaTime);
                        time = 0;
                    }
                    else
                    {
                        transform.rotation = Quaternion.LookRotation(GameManager.Instance.fire.transform.position - transform.position);
                        animator.SetBool(sitWalk, true);
                        Vector3 pos = transform.GetChild(0).transform.position;
                        pos.y = 1;
                        transform.GetChild(0).transform.position = pos;
                    }
                    break;
                case 2:
                    if (time > waiteTime)
                    {
                        animator.SetBool(walkEnd, true);
                    }
                    
                    break;
                case 3:


                    if (time > waiteTime)
                    {
                        currSit.Friends = null;
                        currSit = null;
                        animator.SetBool(walkEnd, true);
                        Vector3 pos = transform.GetChild(0).transform.position;
                        pos.y = 0;
                        transform.GetChild(0).transform.position = pos;
                    }
                    if (nav.remainingDistance - 1 > nav.stoppingDistance)
                    {
                        controller.Move(nav.velocity * Time.deltaTime);
                        time = 0;
                    }
                    else
                    {
                        transform.rotation = Quaternion.LookRotation(GameManager.Instance.fire.transform.position - transform.position);
                        Vector3 pos = transform.GetChild(0).transform.position;
                        pos.y = 1;
                        transform.GetChild(0).transform.position = pos;
                        animator.SetBool(sitWalk, true);
                    }
                    break;
                case 4:
                    if (time > waiteTime)
                    {
                        animator.SetBool(walkEnd, true);
                    }
                    break;
                case 5:
                    if (nav.remainingDistance - 1 > nav.stoppingDistance)
                    {
                        controller.Move(nav.velocity * Time.deltaTime);
                        animator.SetBool(walkEnd, false);
                    }
                    else
                    {
                        controller.Move(Vector3.zero);
                        animator.SetBool(walkEnd, true);
                        animator.SetInteger(animState, 0);
                    }
                    if (time > waiteTime)
                    {
                        animator.SetBool(walkEnd, true);
                    }
                    break;
            }

            yield return new WaitForEndOfFrame();

        }
    }


    public void Speacker()
    {
        animator.SetBool(walkEnd, true);
        animator.SetBool(sitWalk, true);

        time = 0;
        animNum = 2;
        animator.SetInteger(animState, animNum);
        waiteTime = animator.GetCurrentAnimatorStateInfo(0).length / 2;
        animator.SetBool(walkEnd, false);

        Vector3 pos = transform.GetChild(0).transform.position;
        pos.y = 0;
        transform.GetChild(0).transform.position = pos;
        if(currSit != null)
        {
            currSit.Friends = null;
            currSit = null;
        }

    }
}
