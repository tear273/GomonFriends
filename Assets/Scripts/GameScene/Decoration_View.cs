using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration_View : MonoBehaviour
{
    [SerializeField]
    UIButton close_btn;

    [SerializeField]
    UIButton map_btn;

    [SerializeField]
    UIButton background_btn;
    [SerializeField]
    UIButton tent_btn;
    [SerializeField]
    UIButton table_btn;
    [SerializeField]
    UIButton chair_btn;

    [SerializeField]
    UIGrid scroll_Grid;

    [SerializeField]
    GameObject _Origin_Decoration_Contents;

    [SerializeField]
    UIScrollView scrollview;

    public UIButton Close_Btn => close_btn;

    private void Start()
    {
        AddLisener();
        DecoListSetting(0);
    }

    void DecoListSetting(int cate)
    {
        List<DecoChart.Item> items = StaticManager.Chart.Deco.decoSheet;
        
        for (int i=0; i<items.Count; i++)
        {
            if(items[i].Category == cate)
            {
                Decoration_Contents contents = NGUITools.AddChild(scroll_Grid.gameObject, _Origin_Decoration_Contents).GetComponent<Decoration_Contents>();
                contents.SetData(items[i]);
            }
        }

        scroll_Grid.enabled = true;
        scrollview.ResetPosition();
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickClose_Btn);
        close_btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickMap_Btn);
        map_btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickBackground_Btn);
        background_btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickTent_Btn);
        tent_btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickTable_Btn);
        table_btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickChair_Btn);
        chair_btn.onClick.Add(_event);

        
    }

    void OnClickClose_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        Vector3 pos = GameManager.Instance.NumContainer.transform.localPosition;
        pos.y = GameManager.Instance.orgin_NimContainer_Y;
        GameManager.Instance.NumContainer.transform.localPosition = pos;


        gameObject.SetActive(false);
    }

    void OnClickMap_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        NGUITools.DestroyChildren(scroll_Grid.transform);
        DecoListSetting(0);
    }

    void OnClickBackground_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        NGUITools.DestroyChildren(scroll_Grid.transform);
        DecoListSetting(1);
    }

    void OnClickTent_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        NGUITools.DestroyChildren(scroll_Grid.transform);
        DecoListSetting(2);
    }

    void OnClickTable_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        NGUITools.DestroyChildren(scroll_Grid.transform);
        DecoListSetting(3);
    }

    void OnClickChair_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        NGUITools.DestroyChildren(scroll_Grid.transform);
        DecoListSetting(4);
    }

   
}
