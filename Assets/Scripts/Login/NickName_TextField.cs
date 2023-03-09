using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class NickName_TextField : BaseTextField
{
    [SerializeField]
    private UILabel infoLabel;

    [SerializeField]
    private Color redColor;

    [SerializeField]
    private Color blueTopColor;

    [SerializeField]
    private Color blueBottomColor;

    private string con_nick_str = "생성 가능한 닉네임 입니다.";
    private string non_nick_str = "영어와 숫자 조합으로 2자 이상 10자 이내로 입력해 주세요.";

    private void Start()
    {
        Initalize();
    }

    void Initalize()
    {
        EventDelegate _event = new EventDelegate(OnClickClear_Btn);
        GetClear_btn().onClick.Add(_event);
    }


    public void OnClickClear_Btn()
    {
        GetInput_TextField().value = "";
        GetBackground().applyGradient = false;
        GetBackground().color = Color.white;
        infoLabel.text = "";
    }

    public string GetCon_Nick_Str()
    {
        return con_nick_str;
    }

    public string Get_Non_Nick_Str()
    {
        return non_nick_str;
    }

    public bool Checked_NickName()
    {
        UIInput _input = GetInput_TextField();

        if(_input == null)
        {
            return false;
        }

        Regex regexPass = new Regex(@"^[0-9a-z]+$", RegexOptions.IgnorePatternWhitespace);

        if ((_input.value.Length >= 2 && _input.value.Length <= 10) && regexPass.IsMatch(GetInput_TextField().value))
        {
            SetInfoLabel(con_nick_str, true, blueBottomColor, blueBottomColor);

            return true;
        }
        else
        {
            SetInfoLabel(non_nick_str, false, redColor, new Color());
            return false;
        }
    }

    private void SetInfoLabel(string txt, bool apply, Color tc, Color bt)
    {
        UITexture _bg = GetBackground();

        infoLabel.text = txt;
        infoLabel.applyGradient = apply;
        _bg.applyGradient = apply;
        if (apply)
        {
            infoLabel.color = Color.white;
            infoLabel.gradientTop = tc;
            infoLabel.gradientBottom = bt;

            _bg.gradientBottom = bt;
            _bg.gradientTop = tc;
            _bg.color = Color.white;
            
        }
        else
        {
            infoLabel.color = tc;
            _bg.color = tc;
        }
    }

    public void ChangeTextFiled_Text()
    {
        UIInput _input = GetInput_TextField();

        if(_input.value.Length > 0)
        {
            GetClear_btn().gameObject.SetActive(true);
        }
        else
        {
            GetClear_btn().gameObject.SetActive(false);
        }
    }

    
}
