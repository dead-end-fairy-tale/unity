using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class API_Token : MonoBehaviour
{
    public class TokenResponse
    {
        public bool status;
        public string username;
        public string token;
        public string message;
    }

    public static IEnumerator Send()
    {

        using var webRequest = 
            new UnityWebRequest($"{Constants.Url}/api/auth/token?accessToken={UserInfo.Instance.accessToken}", "POST")
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", $"Bearer {UserInfo.Instance.accessToken}");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<TokenResponse>(jsonText);
            
            UserInfo.Instance.SetUserInfo(result.username, result.token);
        }
        else
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<TokenResponse>(jsonText);
            
            Debug.Log(result.message);
            
            Caller.Instance.StartCoroutine(API_Logout.Send());
            AlertSystem.Instance.Error(NotificationTexts.ResetTokenError);
            //나중에 필요시 AlertSystem Error메세지 띄운 후 확인 버튼 눌렀을때 로그아웃 시키기
        }
    }
}
