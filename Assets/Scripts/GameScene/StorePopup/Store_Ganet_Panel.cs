using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Ganet_Panel : MonoBehaviour
{
    [SerializeField]
    UIGrid boxGrid;

    [SerializeField]
    GameObject origin_Store_Box;

    [SerializeField]
    UIGrid ganetGrid;

    [SerializeField]
    GameObject origin_Store_Ganet;

    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        GanetListSetting();
        BoxListSetting();
    }

    void BoxListSetting()
    {
        List<BoxChart.Item> items = StaticManager.Chart.BoxChart.boxSheet;

        for (int i = 0; i < items.Count; i++)
        {

            Store_Box contents = NGUITools.AddChild(boxGrid.gameObject, origin_Store_Box).GetComponent<Store_Box>();
            contents.SetData(items[i]);

        }

        boxGrid.enabled = true;
    }

    void GanetListSetting()
    {
        List<GanetStoreChart.Item> items = StaticManager.Chart.GanetStore.ganetSheet;

        for (int i = 0; i < items.Count; i++)
        {
            
                Store_Ganet contents = NGUITools.AddChild(ganetGrid.gameObject, origin_Store_Ganet).GetComponent<Store_Ganet>();
                contents.SetData(items[i]);
            
        }

        ganetGrid.enabled = true;

    }
}
