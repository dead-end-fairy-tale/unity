using System;
using TMPro;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    [Header("Notice")]
    public GameObject noticeObj;
    public TextMeshProUGUI noticeText;
    
    [Header("Error")]
    public GameObject errorObj;
    public TextMeshProUGUI errorText;
    
    
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
        errorObj.SetActive(false);
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

    public void Error(string error)
    {
        errorText.text = error;
        errorObj.SetActive(true);
    }
    
    public void Notice(string notice)
    {
        noticeText.text = notice;
        noticeObj.SetActive(true);
    }
    
    public void OnErrorOKButtonClick()
    {
        errorObj.SetActive(false);
        ResetErrorText();
    }
    
    public void OnNoticeOKButtonClick()
    {
        noticeObj.SetActive(false);
        ResetNoticeText();
    }
    
    private void ResetErrorText()
    {
        errorText.text = "";
    }
    
    private void ResetNoticeText()
    {
        noticeText.text = "";
    }
}
