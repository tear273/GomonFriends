using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/*
 * Btn_Type : 0. 무료 1. 가넷 or 프랜드쉽
 * Gift_Type : 0. 가넷 2. 프랜드쉽 
 */
public class BoxChart : MonoBehaviour
{
    private const string decoURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTCskBqNU2SaQL3mOHUWRsIOUzfRMF5VD2nMgfyVEN4v82vCYPiJ9k4fuwoY191BHWTcbLDebffxeNs/pub?output=tsv&gid=1639399688";

    public bool isLoading = false;

    public class Item
    {
        public string Title { get; private set; }
        public int RemainingNumber { get; private set; }
        public int Btn_Type { get; private set; }
        public int Gift_Type { get; private set; }
        public int Num { get; private set; }
        public int PossibleNumber { get; private set; }
        public string Code { get; private set; }

        public Item(string title, string remainingNumber, string btn_type, string gift_type, string num, string possibleNumber,string code)
        {
            Title = title;
            RemainingNumber = int.Parse(remainingNumber);
            Btn_Type = int.Parse(btn_type);
            Gift_Type = int.Parse(gift_type);
            Num = int.Parse(num);
            PossibleNumber = int.Parse(possibleNumber);
            Code = code;

        }
    }

    public List<Item> boxSheet = new();

    private IEnumerator GetFriendsSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(decoURL);
        yield return www.SendWebRequest();
        try
        {
            Debug.LogError("스토어 박스 시트 가져오기 성공");
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
            Item item = new Item(sentence[i, 0], sentence[i, 1], sentence[i, 2], sentence[i, 3], sentence[i, 4], sentence[i, 5], sentence[i, 6]);
            boxSheet.Add(item);
        }

        isLoading = true;
    }

    public void Initialize()
    {
        StartCoroutine(GetFriendsSheet());
    }
}
