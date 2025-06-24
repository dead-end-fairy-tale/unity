using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_Login : MonoBehaviour
{
    public class LoginRequest
    {
        public string username;
        public string password;
    }

    public class LoginResponse
    {
        public bool status;
        public string username;
        public string token;
        public string message;
    }

    public static IEnumerator Send(string username, string password, Action<bool, string> onComplete)
    {
        var payload = new LoginRequest
        {
            username = username,
            password = password,
        };
        
        string json = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        
        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/login", "POST")
        {
            uploadHandler   = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };
        
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<LoginResponse>(jsonText);
            
            onComplete?.Invoke(result.status, result.message);
            UserInfo.Instance.SetUserInfo(result.username, result.token);
        }
        else
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<LoginResponse>(jsonText);
            
            onComplete?.Invoke(false, $"Request Error: {result.message}");
        }
        
    }
    
}
