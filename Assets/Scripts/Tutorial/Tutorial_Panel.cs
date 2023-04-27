using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Panel : MonoBehaviour
{
    public int currNum = 0;

    int second_Index = 0;

    [SerializeField]
    UILabel second_Label;

    [SerializeField]
    GameObject second_Conversation;

    [SerializeField]
    GameObject second_Star;

    [SerializeField]
    List<GameObject> tutorials;

    public Action fourthFunc;
    public Action fifthFunc;

    public List<GameObject> Tutorials => tutorials;

    public void OnClickFirst()
    {
        ShowTutorial(currNum);
    }

    public void OnClickSsecond()
    {
        if(second_Index == 0)
        {
            second_Label.text = "음.. 캠핑장이 좀 심심한 걸..\n우선 나부터 초대 해줄래?";
            second_Index++;

        }else if(second_Index == 1)
        {
            second_Conversation.SetActive(false);
            second_Star.SetActive(true);
            second_Index++;
        }
        else
        {
            ShowTutorial(currNum);
        }
    }

    public void OnClickThird()
    {
        ShowTutorial(currNum);
    }

    public void OnClickForth()
    {
        fourthFunc();
       // ShowTutorial(currNum);
    }

    public void OnClickFifth()
    {
        ShowTutorial(currNum);
    }

    public void OnClickSixth()
    {
        ShowTutorial(currNum);
    }

    public void OnClickSeventh()
    {
        fifthFunc();
        ShowTutorial(currNum);
    }

    public void OnClickEight()
    {
        tutorials[currNum].SetActive(false);
        StaticManager.UI.currState = CurrState.MAIN;
    }

    public void ShowTutorial(int index)
    {
        tutorials[index].SetActive(false);
        tutorials[++index].SetActive(true);
        currNum = index;
        NGUITools.BringForward(gameObject);
    }
}
