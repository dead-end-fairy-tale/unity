using System;
using TMPro;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    [Header("Notice")]
    public GameObject noticeObj;
    public TextMeshProUGUI noticeText;
    
    
    private static AlertSystem instance;
    public static AlertSystem Instance {get{ if (instance == null) SetupInstance(); return instance;}}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        noticeObj.SetActive(false);
    }

    private static void SetupInstance()
    {
        instance = FindAnyObjectByType<AlertSystem>();

        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "AlertSystem";
            instance = obj.AddComponent<AlertSystem>();
        }
    }
    
    public void Notice(string notice)
    {
        noticeText.text = notice;
        noticeObj.SetActive(true);
    }
    
    public void OnNoticeOKButtonClick()
    {
        noticeObj.SetActive(false);
        ResetNoticeText();
    }
    
    private void ResetNoticeText()
    {
        noticeText.text = "";
    }
}
