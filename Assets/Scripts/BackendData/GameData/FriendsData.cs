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


        protected override void InitializeData()
        {
            Friends = new Dictionary<string, bool>();
            Friends.Add("F0001", true); 
            Friends.Add("F0008", true);

            ES3.Save("Friends", Friends, GetTableName() + ".es3");
        }


        public override string GetColumnName()
        {
            return null;
        }

        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("Friends", Friends);

            return param;
        }

        public void LoadLocalData()
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            Friends = ES3.Load<Dictionary<string,bool>>("Friends", GetTableName() + ".es3");
            
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


            ES3.Save("Friends", Friends, GetTableName() + ".es3");
        }

        public void SaveLocalData()
        {
            ES3.Save("IsChangeData", true, GetTableName() + ".es3");
            ES3.Save("Friends", Friends, GetTableName() + ".es3");
            
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

        


    }
}

