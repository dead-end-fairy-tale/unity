using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class API_SignUp : MonoBehaviour
{
    public class SignUpRequest
    {
        public string username;
        public string password;
        public string email;

    }
    
    public class SignUpResponse
    {
       public bool status;
       public string message;
       public string username;
       public string token;
    }
    
    public static IEnumerator Send(string username, string password, string email, Action<bool, string> onComplete)
    {
        var payload = new SignUpRequest
        {
            username = username,
            password = password,
            email = email
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
        
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<SignUpResponse>(jsonText);
            
            onComplete?.Invoke(true, result.message);
            UserInfo.Instance.SetUserInfo(result.username, result.token);
        }
        else
        {
            
            if (webRequest.responseCode == 401)
            {
                
            }

            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<SignUpResponse>(jsonText);
            
            onComplete?.Invoke(false, $"Request Error: {result.message}");
        }
        
    }
}
