using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTextField : MonoBehaviour
{
    [SerializeField]
    private UITexture background;

    [SerializeField]
    private UITexture forground;

    [SerializeField]
    private UIButton clear_btn;

    [SerializeField]
    private UIInput input_textField;

    public UITexture GetBackground()
    {
        return background;
    }

    public UITexture GetForground()
    {
        return forground;
    }

    public UIButton GetClear_btn()
    {
        return clear_btn;
    }

    public UIInput GetInput_TextField()
    {
        return input_textField;
    }

 


}
