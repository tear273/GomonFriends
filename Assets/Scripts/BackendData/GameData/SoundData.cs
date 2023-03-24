using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;


namespace BackendData.GameData
{
    public class SoundData : Base.GameData
    {

        public float BackgroundVolum { get; set; }
        public float EffectVolum { get; set; }
        public bool IsOnBackgroundVolum { get; set; }
        public bool IsOnEffectVolum{ get; set; }


        protected override void InitializeData()
        {
            BackgroundVolum = 1;
            EffectVolum = 1;
            IsOnBackgroundVolum = true;
            IsOnEffectVolum = true;

            ES3.Save("IsChangeData", false, GetTableName() + ".es3");
            ES3.Save("BackgroundVolum", BackgroundVolum, GetTableName() + ".es3");
            ES3.Save("EffectVolum", EffectVolum, GetTableName() + ".es3");
            ES3.Save("IsOnBackgroundVolum", IsOnBackgroundVolum, GetTableName() + ".es3");
            ES3.Save("IsOnEffectVolum", IsOnEffectVolum, GetTableName() + ".es3");

        }


        public override string GetColumnName()
        {
            return null;
        }

        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("BackgroundVolum", BackgroundVolum);
            param.Add("EffectVolum", EffectVolum);
            param.Add("IsOnBackgroundVolum", IsOnBackgroundVolum);
            param.Add("IsOnEffectVolum", IsOnEffectVolum);

            return param;
        }

        public void LoadLocalData()
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            BackgroundVolum = (float)ES3.Load("BackgroundVolum", GetTableName() + ".es3");
            EffectVolum = (float)ES3.Load("EffectVolum", GetTableName() + ".es3");
            IsOnBackgroundVolum = (bool)ES3.Load("IsOnBackgroundVolum", GetTableName() + ".es3");
            IsOnEffectVolum = (bool)ES3.Load("IsOnEffectVolum", GetTableName() + ".es3");
            
        }

        public override string GetTableName()
        {
            return "SoundData";
        }



        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            if (ES3.KeyExists("BackgroundVolum", GetTableName() + ".es3"))
                BackgroundVolum = ES3.Load<float>("BackgroundVolum", GetTableName() + ".es3");

            else
                BackgroundVolum = float.Parse(gameDataJson["BackgroundVolum"].ToString());

            if (ES3.KeyExists("EffectVolum", GetTableName() + ".es3"))
                EffectVolum = ES3.Load<float>("EffectVolum", GetTableName() + ".es3");
            else
                EffectVolum = float.Parse(gameDataJson["EffectVolum"].ToString());

            if (ES3.KeyExists("IsOnBackgroundVolum", GetTableName() + ".es3"))
                IsOnBackgroundVolum = ES3.Load<bool>("IsOnBackgroundVolum", GetTableName() + ".es3");
            else
            {
                if (!gameDataJson.ContainsKey("IsOnBackgroundVolum"))
                    gameDataJson["IsOnBackgroundVolum"] = true;
                IsOnBackgroundVolum = bool.Parse(gameDataJson["IsOnBackgroundVolum"].ToString());
            }

            if (ES3.KeyExists("IsOnEffectVolum", GetTableName() + ".es3"))
                IsOnEffectVolum = ES3.Load<bool>("IsOnEffectVolum", GetTableName() + ".es3");
            else
            {
                if (!gameDataJson.ContainsKey("IsOnEffectVolum"))
                    gameDataJson["IsOnEffectVolum"] = true;
                IsOnEffectVolum = bool.Parse(gameDataJson["IsOnEffectVolum"].ToString());
            }
                


            ES3.Save("BackgroundVolum", BackgroundVolum, GetTableName() + ".es3");
            ES3.Save("EffectVolum", EffectVolum, GetTableName() + ".es3");
            ES3.Save("IsOnBackgroundVolum", IsOnBackgroundVolum, GetTableName() + ".es3");
            ES3.Save("IsOnEffectVolum", IsOnEffectVolum, GetTableName() + ".es3");
        }

        public void SaveLocalData()
        {
            ES3.Save("IsChangeData", true, GetTableName() + ".es3");
            ES3.Save("BackgroundVolum", BackgroundVolum, GetTableName() + ".es3");
            ES3.Save("EffectVolum", EffectVolum, GetTableName() + ".es3");
            ES3.Save("IsOnBackgroundVolum", IsOnBackgroundVolum, GetTableName() + ".es3");
            ES3.Save("IsOnEffectVolum", IsOnEffectVolum, GetTableName() + ".es3");
        }

        public void SetIsOnEffectVolum(bool value)
        {
            IsChangedData = true;
            IsOnEffectVolum = value;

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetIsOnBackgroundVolum(bool value)
        {
            IsChangedData = true;
            IsOnBackgroundVolum = value;

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetBackgroundVolum(float value)
        {
            IsChangedData = true;
            BackgroundVolum = value;

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        public void SetEffectVolum(float value)
        {
            IsChangedData = true;
            EffectVolum = value;

            //  GameManager.Instance.ActiveQuestIcon();
            SaveLocalData();
        }

        


    }
}

