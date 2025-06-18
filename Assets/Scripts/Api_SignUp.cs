using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_SignUp : MonoBehaviour
{
    public class SignUpRequest
    {
        public string username;
        public string password;
    }
    
    public class SignUpResponse
    {
       public bool status;
       public string message;
       public string username;
       public string token;
    }
    
    public static IEnumerator Send(string username, string password, Action<bool, string> onComplete)
    {
        var payload = new SignUpRequest
        {
            username = username,
            password = password,
        };
        
        string json = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        
        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/signup", "POST")
        {
            uploadHandler   = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };
        
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        
        string jsonText = webRequest.downloadHandler.text;
        var result = JsonConvert.DeserializeObject<SignUpResponse>(jsonText);
        
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            onComplete?.Invoke(result.status, result.message);
            UserInfo.Instance.SetUserInfo(result.username, result.token);
        }
        else
        {
            onComplete?.Invoke(false, $"Request Error: {result.message}");
        }
        
    }
}
