using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;


namespace BackendData.GameData
{
    public class PurchaseData : Base.GameData
    {

        public Dictionary<string,int> FreeNum { get; set; }
        public Dictionary<string,string> PurchaseGanet { get; set; }


        protected override void InitializeData()
        {
            FreeNum = new Dictionary<string, int>();
            PurchaseGanet = new Dictionary<string, string>();

            ES3.Save("IsChangeData", false, GetTableName() + ".es3");
            ES3.Save("FreeNum", FreeNum, GetTableName() + ".es3");
            ES3.Save("PurchaseGanet", PurchaseGanet, GetTableName() + ".es3");

        }


        public override string GetColumnName()
        {
            return null;
        }

        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("FreeNum", FreeNum);
            param.Add("PurchaseGanet", PurchaseGanet);

            return param;
        }

        public void LoadLocalData()
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            FreeNum = (Dictionary<string,int>)ES3.Load("FreeNum", GetTableName() + ".es3");
            PurchaseGanet = (Dictionary<string,string>)ES3.Load("PurchaseGanet", GetTableName() + ".es3");

        }

        public override string GetTableName()
        {
            return "PurchaseData";
        }



        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            if (ES3.KeyExists("FreeNum", GetTableName() + ".es3"))
                FreeNum = ES3.Load<Dictionary<string, int>>("FreeNum", GetTableName() + ".es3");

            else
                FreeNum = new Dictionary<string, int>();

            if (ES3.KeyExists("PurchaseGanet", GetTableName() + ".es3"))
                PurchaseGanet = ES3.Load<Dictionary<string, string>>("PurchaseGanet", GetTableName() + ".es3");
            else
                PurchaseGanet = new Dictionary<string, string>();

            



            ES3.Save("FreeNum", FreeNum, GetTableName() + ".es3");
            ES3.Save("PurchaseGanet", PurchaseGanet, GetTableName() + ".es3");
        }

        public void SaveLocalData()
        {
            ES3.Save("IsChangeData", true, GetTableName() + ".es3");
            ES3.Save("FreeNum", FreeNum, GetTableName() + ".es3");
            ES3.Save("PurchaseGanet", PurchaseGanet, GetTableName() + ".es3");
        }

        public void SetFreeNum(string code, int num)
        {
            string key = code + "_" + DateTime.Now.ToString("yyyy_MM_dd");

            if (FreeNum.ContainsKey(key))
            {
                FreeNum[key] = num;
            }
            else
            {
                FreeNum.Add(key, num);
            }

            SaveLocalData();
        }

        public void SetPurchaseGanet(string code)
        {
            string key = code + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            PurchaseGanet.Add(key, code);

            SaveLocalData();
        }

        public int GetFreeNum(string code)
        {
            string key = code + "_" + DateTime.Now.ToString("yyyy_MM_dd");

            if (FreeNum.ContainsKey(key))
            {
                return FreeNum[key];
            }
            else
            {
                return 0;
            }
        }

        




    }
}

