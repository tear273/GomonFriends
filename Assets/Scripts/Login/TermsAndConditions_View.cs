using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TermsAndConditions_View : MonoBehaviour
{
    [SerializeField]
    UIButton agree_btn;

    public UIButton GetAgree_Btn
    {
        get
        {
            return agree_btn;
        }
    }
    
}
