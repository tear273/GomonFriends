using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject loadingObject;
    public GameObject origin_ComonPopup;
    public Common_Popup alertUI;
    private GameObject _loadingObject;

    public void SetLoading(bool isActive)
    {
        NGUITools.BringForward(_loadingObject);
        _loadingObject.SetActive(isActive);
    }


    public Common_Popup AlertUI
    {
        get
        {
            return alertUI;
        }
    }

    public void Initialize()
    {
        _loadingObject = NGUITools.AddChild(GameObject.Find("UI Root"), loadingObject);
        _loadingObject.SetActive(false);
        Debug.LogWarning("UIManager 초기화");
        //fadeUI.gameObject.SetActive(false);
        // alertUI.gameObject.SetActive(false);
    }

    //============================================================
    //오브젝트가 없을 경우, Resources에서 검색하여 생성
    //============================================================
    private bool TryLoadUIObject(string prefabName, Transform parent, out GameObject gameObject)
    {
        gameObject = null;
        string path = $"{prefabName}";
        GameObject loadObject = Resources.Load<GameObject>(path);

        if (loadObject == null)
        {
            Debug.LogError($"{prefabName}가 Prefab에 존재하지 않습니다. in {path}");
            return false;
        }

        gameObject = Instantiate(loadObject, parent, false);

        return true;
    }

    //============================================================
    //현재 씬에 UICanvas를 생성. 씬이 이동할 경우 없어지며, 새로운 UI 생성
    //생성만 가능하기에, 삭제가 가능한 Resources의 오브젝트만 해당 UI로 할당
    //============================================================
    public GameObject OpenUI(string folderPath, Transform parent)
    {
        if (!TryLoadUIObject(folderPath, parent, out var uiObject))
        {
            StaticManager.UI.AlertUI.OpenUI("Info",folderPath + "를 찾을 수 없습니다.");
            Debug.LogError(folderPath + "를 찾을 수 없습니다.");
            return null;
        }

        return uiObject;
    }
}
