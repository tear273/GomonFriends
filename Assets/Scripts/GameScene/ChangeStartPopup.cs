using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStartPopup : MonoBehaviour
{

    [SerializeField]
    UISlider slider_Slider;
    [SerializeField]
    UIToggle check_Toggle;
    [SerializeField]
    UILabel starNum_Label;
    [SerializeField]
    UIButton cancel_Btn;
    [SerializeField]
    UIButton confirem_Btn;
    [SerializeField]
    Anim_Star anim_Star;
    [SerializeField]
    UIWidget star_Con;

    private int walkCount = 0;
    private int max_FirendStar = 0;
    public int WalkCount
    {
        get
        {
            return walkCount;
        }
        set
        {
            walkCount = value;
            max_FirendStar = (int)Mathf.Floor(walkCount) / 10;
            starNum_Label.text = ((int)(Mathf.Floor(max_FirendStar * slider_Slider.value))).ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AddLisener();
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickCancel_Btn);
        cancel_Btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickConfirem_Btn);
        confirem_Btn.onClick.Add(_event);

        _event = new EventDelegate(OnSlider);
        slider_Slider.onChange.Add(_event);

        _event = new EventDelegate(OnClickCheck_Btn);
        check_Toggle.onChange.Add(_event);
    }

    void OnClickCheck_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        if (check_Toggle.value)
        {
            slider_Slider.value = 1;
        }
    }

    void OnSlider()
    {
        starNum_Label.text = ((int)(Mathf.Floor(max_FirendStar * slider_Slider.value))).ToString();
        if (slider_Slider.value == 1)
        {
            check_Toggle.value = true;
        }
        else
        {
            check_Toggle.value = false;
        }
    }

    void OnClickCancel_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        gameObject.SetActive(false);
    }

    void OnClickConfirem_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        
        int purchaseStar = int.Parse(starNum_Label.text);

        StaticManager.Backend.backendGameData.UserData.SetPurchaseFriendShipStar(purchaseStar * 10);
        StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(StaticManager.Backend.backendGameData.UserData.FriendShipStar + purchaseStar);
        StaticManager.Backend.backendGameData.UserData.Update((callback) =>
        {
            if (callback.IsSuccess())
            {
                CreateAnimation();
                
            }
            else
            {
                Debug.LogError("우정별 구매 실패");
                StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(StaticManager.Backend.backendGameData.UserData.FriendShipStar - purchaseStar);
                StaticManager.Backend.backendGameData.UserData.SetPurchaseFriendShipStar(-purchaseStar * 10);
                gameObject.SetActive(false);
            }

        });

        
    }


    void CreateAnimation()
    {
        for(int i=1; i<star_Con.transform.childCount; i++)
        {
            Destroy(star_Con.transform.GetChild(i).gameObject);
        }
        float width = star_Con.width/2;
        float height = star_Con.height/2;


        int num = Random.Range(8, 16);

        for (int i = 0; i < num; i++)
        {
            float x = Random.Range(-width, width);
            float y = Random.Range(-height, height);

            float rot_z = Random.Range(-45f, 45f);


            GameObject obj = NGUITools.AddChild(star_Con.gameObject, anim_Star.gameObject);
            obj.transform.localPosition = new Vector3(x, y, 0);
            obj.transform.localRotation = Quaternion.Euler(0, 0, rot_z);
            obj.SetActive(true);
        }

        StartCoroutine(FriendShipLabelStarAnim());

    }

    IEnumerator FriendShipLabelStarAnim()
    {
        float time = 0f;
        int friendShip = int.Parse(GameManager.Instance.FriendsShip_Label.text);
        int plusFriendShip = int.Parse(starNum_Label.text);


        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, 0);
        curve.AddKey(0.5f, 1);
        curve.AddKey(1, 0);

        yield return new WaitForSeconds(1.4f);

        while (time <= 1.0f)
        {
            time += Time.deltaTime;

            int value =  (int)Mathf.Lerp(friendShip, plusFriendShip + friendShip, time);
            
            GameManager.Instance.FriendsShip_Label.text = value.ToString();
            GameManager.Instance.FriendsShip_Label.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1.2f, 1.2f, 1), curve.Evaluate(time * 1.5f));

            yield return new WaitForEndOfFrame();
        }

        StaticManager.Sound.PlaySounds(SoundsType.GETSTAR);
        GameManager.Instance.PedometerPlugin.LoadTotalStep();
        


        yield return new WaitForSeconds(0.1f);
        StaticManager.UI.alertUI.OpenUI("Info", "우정별 " + plusFriendShip + "개 구매 하셨습니다.");

        gameObject.SetActive(false);
    }

    
}
