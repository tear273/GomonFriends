using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Star : MonoBehaviour
{
    [SerializeField]
    Transform frandship_pos;
    [SerializeField]
    AnimationCurve curved;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        float time = 0f;

        

        while(time <= 0.5f)
        {
            transform.localScale = Vector2.Lerp(new Vector2(0, 0), new Vector2(1, 1), time * 2);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        float limitTime = Random.Range(0.5f, 0.9f);

        yield return new WaitForSeconds(limitTime);

        time = 0;
        Vector3 originPos = transform.position;
        while(time <= 0.5f)
        {
            //print(time);
            //print(curved.Evaluate(time));
            transform.position = Vector3.Lerp(originPos, frandship_pos.position,curved.Evaluate(time * 2));

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

}
