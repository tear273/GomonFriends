using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GanetStoreChart : MonoBehaviour
{
    private const string decoURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTCskBqNU2SaQL3mOHUWRsIOUzfRMF5VD2nMgfyVEN4v82vCYPiJ9k4fuwoY191BHWTcbLDebffxeNs/pub?output=tsv&gid=869381465";

    public bool isLoading = false;

    public class Item
    {
        public string Name { get; private set; }
        public int Type { get; private set; }
        public string Price { get; private set; }
        public string Num { get; private set; }
        public string ProductID { get; private set; }
        public string ImagePath { get; private set; }

        public Item(string name, string type, string price, string num,string productID,string imagePath)
        {
            Name = name;
            Type = int.Parse(type);
            Price = price;
            Num = num;
            ProductID = productID;
            ImagePath = imagePath;

        }
    }

    public List<Item> ganetSheet = new();

    private IEnumerator GetFriendsSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(decoURL);
        yield return www.SendWebRequest();
        try
        {
            Debug.LogError("스토어 가넷 시트 가져오기 성공");
            SetFriendsList(www.downloadHandler.text);
        }
        catch (Exception e)
        {

        }
    }

    private void SetFriendsList(string tsv)
    {
        string[] row = tsv.Split('\n'); //세로
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length; //가로
        string[,] sentence = new string[rowSize, columnSize];

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
            {
                sentence[i, j] = column[j];
            }
        }

        //클래스 리스트
        for (int i = 1; i < rowSize; i++)
        {
            Item item = new Item(sentence[i, 0], sentence[i, 1], sentence[i, 2], sentence[i, 3], sentence[i, 4], sentence[i, 5]);
            ganetSheet.Add(item);
        }

        isLoading = true;
    }

    public void Initialize()
    {
        StartCoroutine(GetFriendsSheet());
    }
}
