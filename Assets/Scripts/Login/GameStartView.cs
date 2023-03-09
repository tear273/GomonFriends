using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartView : MonoBehaviour
{
    [SerializeField]
    UIButton gameStart_btn;

    [SerializeField]
    UIButton logOut_btn;

    public UIButton GetGameStart_Btn => gameStart_btn;
    public UIButton GetLogOut_Btn => logOut_btn;
}
