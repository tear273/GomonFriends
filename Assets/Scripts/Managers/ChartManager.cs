using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChartManager : MonoBehaviour
{
    public bool isAllLoading = false;

    [SerializeField] private DecoChart decoChart;
    [SerializeField] private FriendsChart friendsChart;


    public DecoChart Deco => decoChart;
    public FriendsChart Friends => friendsChart;




    public IEnumerator Initialize()
    {
        StaticManager.UI.SetLoading(true);
        Deco.Initialize();
        

        yield return new WaitUntil(() => Deco.isLoading);

        Friends.Initialize();

        yield return new WaitUntil(() => Friends.isLoading);

        isAllLoading = true;

        StaticManager.UI.SetLoading(false);
    }
}
