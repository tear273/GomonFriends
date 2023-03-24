using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;


namespace BackendData.GameData
{
    public class UserData : Base.GameData
    {

        public int FriendShipStar { get; set; }
        public int Ganet { get;  set; }
        public int Pedometor { get; set; }
        public int PurchaseFriendShipStar { get; set; }
        public bool New { get; set; }

        protected override void InitializeData()
        {
            FriendShipStar = 100;
            Ganet = 1;
            Pedometor = 0;
            PurchaseFriendShipStar = 0;
            New = true;

#if UNITY_EDITOR
            FriendShipStar = 1000000;
            Ganet = 40;
            Pedometor = 100000;
            New = true;
#endif

            ES3.Save("IsChangeData", false, GetTableName() + ".es3");
            ES3.Save("FriendShipStar", FriendShipStar, GetTableName() + ".es3");
            ES3.Save("Ganet", Ganet, GetTableName() + ".es3");
            ES3.Save("Pedometor", Pedometor, GetTableName() + ".es3");
            ES3.Save("PurchaseFriendShipStar", PurchaseFriendShipStar, GetTableName() + ".es3");
            ES3.Save("New", New, GetTableName() + ".es3");

        }
        

        public override string GetColumnName()
        {
            return null;
        }

        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("FriendShipStar", FriendShipStar);
            param.Add("Ganet", Ganet);
            param.Add("Pedometor", Pedometor);
            param.Add("PurchaseFriendShipStar", PurchaseFriendShipStar);
            param.Add("New", New);

            return param;
        }

        public void LoadLocalData()
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            FriendShipStar = (int)ES3.Load("FriendShipStar", GetTableName() + ".es3");
            Ganet = (int)ES3.Load("Ganet", GetTableName() + ".es3");
            Pedometor = (int)ES3.Load("Pedometor", GetTableName() + ".es3");
            PurchaseFriendShipStar = (int)ES3.Load("PurchaseFriendShipStar", GetTableName() + ".es3");
            New = (bool)ES3.Load("New", GetTableName() + ".es3");
        }

        public override string GetTableName()
        {
            return "UserData";
        }

        

        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            if (ES3.KeyExists("FriendShipStar", GetTableName() + ".es3"))
                FriendShipStar = ES3.Load<int>("FriendShipStar", GetTableName() + ".es3");

            else
                FriendShipStar = int.Parse(gameDataJson["FriendShipStar"].ToString());

            if (ES3.KeyExists("Ganet", GetTableName() + ".es3"))
                Ganet = ES3.Load<int>("Ganet", GetTableName() + ".es3");

            else
                Ganet = int.Parse(gameDataJson["Ganet"].ToString());

            if (ES3.KeyExists("Pedometor", GetTableName() + ".es3"))
                Pedometor = ES3.Load<int>("Pedometor", GetTableName() + ".es3");

            else
                Pedometor = int.Parse(gameDataJson["Pedometor"].ToString());

            if (ES3.KeyExists("PurchaseFriendShipStar", GetTableName() + ".es3"))
                PurchaseFriendShipStar = ES3.Load<int>("PurchaseFriendShipStar", GetTableName() + ".es3");

            else
            {
                if (!gameDataJson.ContainsKey("PurchaseFriendShipStar"))
                    gameDataJson["PurchaseFriendShipStar"] = 0;
                PurchaseFriendShipStar = int.Parse(gameDataJson["PurchaseFriendShipStar"].ToString());
            }

            if (ES3.KeyExists("New", GetTableName() + ".es3"))
                New = ES3.Load<bool>("New", GetTableName() + ".es3");
            else
            {
                
                gameDataJson["New"] = true;
                New = bool.Parse(gameDataJson["New"].ToString());
            }



            ES3.Save("FriendShipStar", FriendShipStar, GetTableName() + ".es3");
            ES3.Save("Ganet", Ganet, GetTableName() + ".es3");
            ES3.Save("Pedometor", Pedometor, GetTableName() + ".es3");
            ES3.Save("PurchaseFriendShipStar", PurchaseFriendShipStar, GetTableName() + ".es3");
            ES3.Save("New", New, GetTableName() + ".es3");
        }

        public void SaveLocalData()
        {
            ES3.Save("IsChangeData", true, GetTableName() + ".es3");
            ES3.Save("FriendShipStar", FriendShipStar, GetTableName() + ".es3");
            ES3.Save("Ganet", Ganet, GetTableName() + ".es3");
            ES3.Save("Pedometor", Pedometor, GetTableName() + ".es3");
            ES3.Save("PurchaseFriendShipStar", PurchaseFriendShipStar, GetTableName() + ".es3");
            ES3.Save("New", New, GetTableName() + ".es3");
        }

        public void SetNew(bool value)
        {
            IsChangedData = true;
            New = value;

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetPurchaseFriendShipStar(int value)
        {
            IsChangedData = true;
            PurchaseFriendShipStar += value;

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetFriendShipStar(int value)
        {
            IsChangedData = true;
            FriendShipStar = value;

          //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetGanet(int value)
        {
            IsChangedData = true;
            Ganet = value;

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetPedometor(int value)
        {
            IsChangedData = true;
            Pedometor = value;

            SaveLocalData();
        }


    }
}
    
