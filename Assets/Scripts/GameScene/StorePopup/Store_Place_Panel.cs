using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Place_Panel : MonoBehaviour
{
    [SerializeField]
    UIGrid grid;
    [SerializeField]
    GameObject origin_Store_Background;


    [SerializeField]
    List<Store_Background> lstStoreBackground;
    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        DecoListSetting();
    }

    void DecoListSetting()
    {
        List<DecoChart.Item> items = StaticManager.Chart.Deco.decoSheet;

        for (int i = 0; i < items.Count; i++)
        {
            
                Store_Background contents = NGUITools.AddChild(grid.gameObject, origin_Store_Background).GetComponent<Store_Background>();
                contents.SetData(items[i]);
            lstStoreBackground.Add(contents);


        }

        grid.enabled = true;

    }

    private void OnEnable()
    {
        for(int i=0; i<lstStoreBackground.Count; i++)
        {
            lstStoreBackground[i].SetDataState();
        }
    }

}
