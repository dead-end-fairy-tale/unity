using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class API_Logout : MonoBehaviour
{

    public class LogoutResponse
    {
        public bool status;
        public string message;
    }

    public static IEnumerator Send()
    {

        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/logout", "GET")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", $"Bearer {UserInfo.Instance.accessToken}");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            UserInfo.Instance.SetUserInfo(null, null);
        }
        else
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<LogoutResponse>(jsonText);
            
            Debug.Log(result.message);
            
            UserInfo.Instance.SetUserInfo(null, null);
        }

    }
}

