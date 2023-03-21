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


        protected override void InitializeData()
        {
            BackgroundVolum = 1;
            EffectVolum = 1;

            ES3.Save("IsChangeData", false, GetTableName() + ".es3");
            ES3.Save("BackgroundVolum", BackgroundVolum, GetTableName() + ".es3");
            ES3.Save("EffectVolum", EffectVolum, GetTableName() + ".es3");

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

            return param;
        }

        public void LoadLocalData()
        {
            if (!ES3.KeyExists("IsChangeData", GetTableName() + ".es3"))
                ES3.Save("IsChangeData", false, GetTableName() + ".es3");

            BackgroundVolum = (float)ES3.Load("BackgroundVolum", GetTableName() + ".es3");
            EffectVolum = (float)ES3.Load("EffectVolum", GetTableName() + ".es3");
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


            ES3.Save("BackgroundVolum", BackgroundVolum, GetTableName() + ".es3");
            ES3.Save("EffectVolum", EffectVolum, GetTableName() + ".es3");
        }

        public void SaveLocalData()
        {
            ES3.Save("IsChangeData", true, GetTableName() + ".es3");
            ES3.Save("BackgroundVolum", BackgroundVolum, GetTableName() + ".es3");
            ES3.Save("EffectVolum", EffectVolum, GetTableName() + ".es3");
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

