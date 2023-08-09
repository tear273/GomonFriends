using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsSpeacker_btn_Block : MonoBehaviour
{
    [SerializeField]
    UILabel timer_Label;
    private void OnEnable()
    {
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {

        float timer = 5;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            timer_Label.text = ((int)timer).ToString();
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
    }
}
