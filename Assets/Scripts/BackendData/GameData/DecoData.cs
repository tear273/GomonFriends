using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;


namespace BackendData.GameData
{
    public class DecoData : Base.GameData
    {

        public Dictionary<string, bool> Deco { get; set; }

        protected override void InitializeData()
        {
            Deco = new Dictionary<string, bool>();
            Deco.Add("D0001", true);
            Deco.Add("D0007", true);
            Deco.Add("D0012", true);

            ES3.Save("Deco", Deco, GetTableName() + ".es3");
        }


        public override string GetColumnName()
        {
            return null;
        }

        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("Deco", Deco);

            return param;
        }

        public void LoadLocalData()
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            Deco = ES3.Load<Dictionary<string, bool>>("Deco", GetTableName() + ".es3");
            
        }

        public override string GetTableName()
        {
            return "DecoData";
        }



        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            

            if (ES3.KeyExists("Deco", GetTableName() + ".es3"))
                Deco = ES3.Load<Dictionary<string, bool>>("Deco", GetTableName() + ".es3");

            else
                Deco = JsonConvert.DeserializeObject<Dictionary<string, bool>>(gameDataJson["Deco"].ToJson());



            ES3.Save("Deco", Deco, GetTableName() + ".es3");
        }

        public void SaveLocalData()
        {
            ES3.Save("IsChangeData", true, GetTableName() + ".es3");
            ES3.Save("Deco", Deco, GetTableName() + ".es3");
        }

        public void SetDeco(string code, bool isOn = false)
        {
            IsChangedData = true;
            if (Deco.ContainsKey(code))
            {
                Deco[code] = isOn;
            }
            else
            {
                Deco.Add(code, isOn);
            }

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }


    }
}