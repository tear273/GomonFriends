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
    [SerializeField] private GanetStoreChart ganetStoreChart;
    [SerializeField] private BoxChart boxChart;
    [SerializeField] private SkinChart skinChart;


    public DecoChart Deco => decoChart;
    public FriendsChart Friends => friendsChart;
    public GanetStoreChart GanetStore => ganetStoreChart;
    public BoxChart BoxChart => boxChart;
    public SkinChart SkinChart => skinChart;




    public IEnumerator Initialize()
    {
        StaticManager.UI.SetLoading(true);

        Deco.Initialize(); 

        yield return new WaitUntil(() => Deco.isLoading);

        Friends.Initialize();

        yield return new WaitUntil(() => Friends.isLoading);

        GanetStore.Initialize();

        yield return new WaitUntil(() => GanetStore.isLoading);

        BoxChart.Initialize();

        yield return new WaitUntil(() => BoxChart.isLoading);

        skinChart.Initialize();

        yield return new WaitUntil(() => BoxChart.isLoading);

        isAllLoading = true;

        StaticManager.UI.SetLoading(false);
    }
}
