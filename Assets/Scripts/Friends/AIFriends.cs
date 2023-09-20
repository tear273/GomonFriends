using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum AIAnim
{
    Idle,
    Sleep,
    Dance,
    Sit,
    Pose,
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

    [SerializeField]
    AIAnim aiAnim = AIAnim.Idle;

    AnimationClip clip;

    SitDeco currSit = null;


    SpeachBubble speechObj = null;

    public string[] speech;
    private void OnDisable()
    {
        Destroy(speechObj.gameObject);
    }

    private void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 uiPos = UICamera.mainCamera.ViewportToWorldPoint(pos);
        uiPos.z = 0;
        speechObj.transform.position = uiPos;

        Vector3 local = speechObj.transform.localPosition;
        local.y += speechObj.background.height + 70;
        local.x -= 60;
        speechObj.transform.localPosition = local;

        speechObj.background.width = speechObj.text.width + 30;
        speechObj.background.height = speechObj.text.height + 50;
    }

    private void OnEnable()
    {
        StartCoroutine(StartAnimator());
        

        GameObject speech = Resources.Load("UI/Friends/SpeechBubble") as GameObject;
        
        speechObj = NGUITools.AddChild(GameObject.Find("UI Root"), speech).GetComponent<SpeachBubble>();
        

        int num = GameObject.FindGameObjectsWithTag("SpeechBubble").Length;

        speechObj.background.depth = num * 2;
        speechObj.text.depth = num * 2 + 1;
        StartCoroutine(SpeechBubble());
    }

    IEnumerator SpeechBubble()
    {
        
        speechObj.gameObject.SetActive(true);
        int index = UnityEngine.Random.Range(0, speech.Length);
        speechObj.text.text = speech[index].Replace("\\n", "\n");

        yield return new WaitForSeconds(1.3f);

        speechObj.gameObject.SetActive(false);


        int waitTime = UnityEngine.Random.Range(8, 13);

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SpeechBubble());
    }

    IEnumerator StartAnimator()
    {
        animator.SetInteger(animState, 0);
        animator.SetBool(walkEnd, true);
        animator.SetBool(sitWalk, true);
        yield return new WaitForSeconds(1);
        animator.SetBool(walkEnd, false);
        animator.SetBool(sitWalk, false);

        Vector3 pos = transform.GetChild(0).transform.position;
        pos.y = 0;
        transform.GetChild(0).transform.position = pos;

        float waiteTime = UnityEngine.Random.Range(1f,10f);
        
        yield return new WaitForSeconds(waiteTime);

        animNum = UnityEngine.Random.Range(1, 101);
        //animNum = 20;
        if (0 < animNum && 10 >= animNum)
        {
            aiAnim = AIAnim.Pose;
        }
        else if (10 < animNum && 80 >= animNum)
        {
            aiAnim = AIAnim.Walk;
        }
        else if (70 < animNum && 80 >= animNum)
        {
            aiAnim = AIAnim.Sit;
        }
        else if (80 < animNum && 90 >= animNum)
        {
            aiAnim = AIAnim.Sleep;
        }
        else
        {
            aiAnim = AIAnim.Dance;
        }
        
        switch (aiAnim)
        {
            case AIAnim.Idle:

                break;
            case AIAnim.Sleep:
                StartCoroutine(StartSleep());
                break;
            case AIAnim.Dance:
                animator.SetInteger(animState, ((int)aiAnim));
                StartCoroutine(StartDance());
                break;
            case AIAnim.Sit:
                animator.SetInteger(animState, ((int)aiAnim));
                StartCoroutine(StartSit());
                break;
            case AIAnim.Pose:
                animator.SetInteger(animState, ((int)aiAnim));
                StartCoroutine(StartPose());
                break;
            case AIAnim.Walk:
                animator.SetInteger(animState, ((int)aiAnim));
                StartCoroutine(StartWalk());
                break;
        }

        
    }

    IEnumerator StartPose()
    {
        
        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(10);
        animator.SetBool(walkEnd, true);
        StartCoroutine(StartAnimator());
    }

    IEnumerator StartWalk()
    {
        
        Vector3 pos = transform.GetChild(0).position;
        pos.x += UnityEngine.Random.Range(-3.0f, 3.0f);
        pos.z += UnityEngine.Random.Range(-3.0f, 3.0f);
        nav.SetDestination(pos);
        yield return new WaitForEndOfFrame();

        float m_time = 0;
        while (m_time < 4)
        {
            Vector3 fpos = transform.GetChild(0).position;
            fpos.y = 0;
            pos.y = 0;
//            print("sqrMagnitude, " + gameObject.name + " : " + (pos - fpos).magnitude);
            if (Mathf.Abs( (pos - fpos).magnitude) >= 2.0f)
            {
                controller.Move(nav.velocity * Time.deltaTime);
            }
            else
            {
                controller.Move(Vector3.zero);
                break;
            }

           
            yield return new WaitForEndOfFrame();
            m_time += Time.deltaTime;
        }
        controller.Move(Vector3.zero);
        animator.SetBool(walkEnd, true);
        StartCoroutine(StartAnimator());
    }

    IEnumerator StartSit()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < GameManager.Instance.SitDecos.Length; i++)
        {
            SitDeco friends = GameManager.Instance.SitDecos[i];
            if (friends.Friends == null)
            {
                nav.SetDestination(friends.transform.position);
                friends.Friends = this;
                currSit = friends;
                animator.SetBool(walkEnd, false);
                animator.SetBool(sitWalk, false);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        float mtime = 0;
        while (mtime < 4)
        {
            if (nav.remainingDistance > nav.stoppingDistance)
            {
                controller.Move(nav.velocity * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(GameManager.Instance.fire.transform.position - transform.position);
                Vector3 pos = transform.GetChild(0).transform.position;
                pos.y = 1;
                transform.GetChild(0).transform.position = pos;
                animator.SetBool(sitWalk, true);

                break;
                
            }
            /*if(mtime > 3)
            {
                transform.rotation = Quaternion.LookRotation(GameManager.Instance.fire.transform.position - transform.position);
                Vector3 pos = transform.GetChild(0).transform.position;
                pos.y = 1;
                transform.GetChild(0).transform.position = pos;
                animator.SetBool(sitWalk, true);
                break;
            }*/

            mtime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(10);
        if(currSit != null)
        {
            currSit.Friends = null;
            currSit = null;
        }
        
        animator.SetBool(walkEnd, true);
        yield return new WaitForEndOfFrame();
        animator.SetBool(walkEnd, false);
        animator.SetInteger(animState, ((int)AIAnim.Walk));
        StartCoroutine(StartWalk());
        //StartCoroutine(StartAnimator());

    }

    IEnumerator StartSleep()
    {
        
        yield return new WaitForEndOfFrame();

        waiteTime = animator.GetCurrentAnimatorStateInfo(0).length;
        for (int i = 0; i < GameManager.Instance.SitDecos.Length; i++)
        {
            SitDeco friends = GameManager.Instance.SitDecos[i];
            if (friends.Friends == null)
            {

               
                friends.Friends = this;
                
                currSit = friends;
                animator.SetBool(walkEnd, false);
                animator.SetBool(sitWalk, false);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();

        if (currSit != null)
        {
            nav.SetDestination(currSit.transform.position);
            animator.SetInteger(animState, ((int)aiAnim));
            float mtime = 0f;
            while (true)
            {
                Vector3 sitPos = currSit.transform.position;
                Vector3 fpos = transform.GetChild(0).position;

                
                if (Mathf.Abs((sitPos - fpos).sqrMagnitude) >= 2f)
                {
                    controller.Move(nav.velocity * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(GameManager.Instance.fire.transform.position - transform.position);
                    animator.SetBool(sitWalk, true);
                    Vector3 pos = transform.GetChild(0).transform.position;
                    pos.y = 1;
                    transform.GetChild(0).transform.position = pos;

                    break;
                }

                /*if(mtime > 6)
                {
                    transform.rotation = Quaternion.LookRotation(GameManager.Instance.fire.transform.position - transform.position);
                    animator.SetBool(sitWalk, true);
                    Vector3 pos = transform.GetChild(0).transform.position;
                    pos.y = 1;
                    transform.GetChild(0).transform.position = pos;
                    break;
                }*/
                mtime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            int hh = int.Parse(DateTime.Now.ToString("HH"));

            if (hh > 18 || hh < 7)
            {

                animator.SetBool(walkEnd, false);
            }
            else
            {
                float m_time = 0;
                while(m_time < 10)
                {
                    transform.rotation = Quaternion.LookRotation(GameManager.Instance.fire.transform.position - transform.position);
                    m_time += Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }

                //yield return new WaitForSeconds(10);

                if (currSit != null)
                {
                    currSit.Friends = null;
                    currSit = null;
                }

                animator.SetBool(walkEnd, true);
                animator.SetBool(sitWalk, false);
                yield return new WaitForEndOfFrame();
                animator.SetBool(walkEnd, false);
                animator.SetInteger(animState, ((int)AIAnim.Walk));
                StartCoroutine(StartWalk());
            }

        }
        else
        {
            animator.SetBool(walkEnd, true);
            animator.SetBool(sitWalk, true);
            yield return new WaitForEndOfFrame();
            StartCoroutine(StartAnimator());
        }
        
    }

    IEnumerator StartDance()
    {
        yield return new WaitForSeconds(3);

        
        StartCoroutine(StartAnimator());
    }

    IEnumerator CorutionSpeaker()
    {
        nav.SetDestination(transform.position);
        animator.SetInteger(animState, ((int)AIAnim.Idle));
        if (animState == (int)AIAnim.Sleep || animState == (int)AIAnim.Sit)
        {
            animator.SetBool(walkEnd, true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool(sitWalk, true);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            animator.SetBool(walkEnd, true);
            animator.SetBool(sitWalk, true);
            yield return new WaitForSeconds(1);
        }
        

        

        Vector3 pos = transform.GetChild(0).transform.position;
        pos.y = 0;
        transform.GetChild(0).transform.position = pos;

        if (currSit != null)
        {
            currSit.Friends = null;
            currSit = null;
        }

        yield return new WaitForEndOfFrame();
        animator.SetBool(walkEnd, false);
        animator.SetBool(sitWalk, false);
        animator.SetInteger(animState, ((int)AIAnim.Dance));
        StartCoroutine(StartDance());

    }


    public void Speacker()
    {
        if(aiAnim != AIAnim.Sleep && aiAnim != AIAnim.Sit)
        {
            StopAllCoroutines();
            StartCoroutine(CorutionSpeaker());
        }
        

        /*time = 0;
        animNum = 2;
        animator.SetInteger(animState, animNum);
        waiteTime = animator.GetCurrentAnimatorStateInfo(0).length;
        animator.SetBool(walkEnd, false);

        Vector3 pos = transform.GetChild(0).transform.position;
        pos.y = 0;
        transform.GetChild(0).transform.position = pos;
        if(currSit != null)
        {
            currSit.Friends = null;
            currSit = null;
        }*/

    }
}
