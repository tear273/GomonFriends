using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNickName_View : MonoBehaviour
{
    [SerializeField]
    NickName_TextField nickName_TextField;

    [SerializeField]
    UIButton confirem_btn;

    public NickName_TextField nickTextField => nickName_TextField;
    public UIButton GetConfirem_btn => confirem_btn;

    
    // Start is called before the first frame update
    void Start()
    {
       // Initalize();
    }

    private void OnEnable()
    {
        nickName_TextField.OnClickClear_Btn();
    }

    
}
