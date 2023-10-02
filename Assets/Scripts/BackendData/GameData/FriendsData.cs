using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;


namespace BackendData.GameData
{
    public class FriendsData : Base.GameData
    {

        public Dictionary<string,bool> Friends { get; set; }
        public Dictionary<string, List<string>> Skin { get; set; }
        public Dictionary<string, string> ConnectSkin { get; set; }

        protected override void InitializeData()
        {
            Friends = new Dictionary<string, bool>();
            Skin = new Dictionary<string, List<string>>();
            ConnectSkin = new Dictionary<string, string>();
            // Friends.Add("F0001", true); 
            // Friends.Add("F0008", true);

            ES3.Save("IsChangeData", false, GetTableName() + ".es3");
            ES3.Save("Friends", Friends, GetTableName() + ".es3");
            ES3.Save("Skin", Skin, GetTableName() + ".es3");
            ES3.Save("ConnectSkin", ConnectSkin, GetTableName() + ".es3");
        }


        public override string GetColumnName()
        {
            return null;
        }

        public override Param GetParam()
        {
            Param param = new Param();
            param.Add("Friends", Friends);
            param.Add("Skin", Skin);
            param.Add("ConnectSkin", ConnectSkin);

            return param;
        }

        public void LoadLocalData()
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            Friends = ES3.Load<Dictionary<string,bool>>("Friends", GetTableName() + ".es3");
            Skin = ES3.Load<Dictionary<string, List<string>>>("Skin", GetTableName() + ".es3");
            ConnectSkin = ES3.Load<Dictionary<string, string>>("ConnectSkin", GetTableName() + ".es3");

        }

        public override string GetTableName()
        {
            return "FriendsData";
        }



        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {
          
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            if (ES3.KeyExists("Friends", GetTableName() + ".es3"))
                Friends = ES3.Load<Dictionary<string, bool>>("Friends", GetTableName() + ".es3");

            else
                Friends = new Dictionary<string, bool>();


            if (ES3.KeyExists("Skin", GetTableName() + ".es3"))
                Skin = ES3.Load<Dictionary<string, List<string>>>("Skin", GetTableName() + ".es3");

            else
                Skin = new Dictionary<string, List<string>>();

            if (ES3.KeyExists("ConnectSkin", GetTableName() + ".es3"))
                ConnectSkin = ES3.Load<Dictionary<string, string>>("ConnectSkin", GetTableName() + ".es3");

            else
                ConnectSkin = new Dictionary<string, string>();


            ES3.Save("Friends", Friends, GetTableName() + ".es3");
            ES3.Save("Skin", Skin, GetTableName() + ".es3");
            ES3.Save("ConnectSkin", ConnectSkin, GetTableName() + ".es3");
        }

        public void SaveLocalData()
        {
            ES3.Save("IsChangeData", true, GetTableName() + ".es3");
            ES3.Save("Friends", Friends, GetTableName() + ".es3");
            ES3.Save("Skin", Skin, GetTableName() + ".es3");
            ES3.Save("ConnectSkin", ConnectSkin, GetTableName() + ".es3");

        }

        public void SetFriends(string code , bool isOn = false)
        {
            IsChangedData = true;

            if (Friends.ContainsKey(code))
            {
                Friends[code] = isOn;
            }
            else
            {
                Friends.Add(code, isOn);
            }

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetSkin(string code, string skinCode)
        {
            IsChangedData = true;

            if (Skin.ContainsKey(code))
            {
                Skin[code].Add(skinCode);
            }
            else
            {
                Skin.Add(code, new List<string>());
                Skin[code].Add(skinCode);
            }

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetConnectSkin(string code, string skinCode)
        {
            IsChangedData = true;

            if (ConnectSkin.ContainsKey(code))
            {
                ConnectSkin[code] = skinCode;
            }
            else
            {
                ConnectSkin.Add(code, skinCode);
            }

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }




    }
}

