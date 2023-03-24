using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DecoChart : MonoBehaviour
{
    private const string decoURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTCskBqNU2SaQL3mOHUWRsIOUzfRMF5VD2nMgfyVEN4v82vCYPiJ9k4fuwoY191BHWTcbLDebffxeNs/pub?output=tsv&gid=0";

    public bool isLoading = false;

    public class Item
    {
        public string Name { get; private set; }
        public string Info { get; private set; }
        public string TextrueUrl { get; private set; }
        public string Code { get; private set; }
        public int Price { get; private set; }
        public int From { get; private set; }
        public int Category { get; private set; }
        public string ObjName { get; private set; }

        public Item(string name, string info, string url, string code, string price, string from, string category,string objName)
        {
            Name = name;
            Info = info;
            Code = code;
            TextrueUrl = url;
            Price = int.Parse(price);
            From = int.Parse(from);
            Category = int.Parse(category);
            ObjName = objName;
        }
    }
    
    public List<Item> decoSheet = new();

    private IEnumerator GetDecorationSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(decoURL);
        yield return www.SendWebRequest();
        try
        {
            Debug.LogError("장식 시트 가져오기 성공");
            SetDecoList(www.downloadHandler.text);
        }
        catch (Exception e)
        {

        }
    }

    private void SetDecoList(string tsv)
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
            Item item = new Item(sentence[i, 0], sentence[i, 1], sentence[i, 2], sentence[i, 3], sentence[i, 4], sentence[i, 5], sentence[i, 6], sentence[i, 7]);
            decoSheet.Add(item);
        }

        isLoading = true;
    }

    public void Initialize()
    {
        StartCoroutine(GetDecorationSheet());
    }
}
