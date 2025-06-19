using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_VerifyEmail : MonoBehaviour
{
    public class VerifyEmailRequest
    {
        public string email;
        public string code;
    }

    public class VerifyEmailResponse
    {
        public bool status;
        public string message;
    }
    
    public static IEnumerator Send(string email, string code,Action<bool, string> onComplete)
    {
        var payload = new VerifyEmailRequest
        {
            email = email,
            code = code
        };
        
        string json = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        
        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/verify-email", "POST")
        {
            uploadHandler   = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };
        
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<VerifyEmailResponse>(jsonText);
            
            onComplete?.Invoke(result.status, result.message);
        }
        else
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<VerifyEmailResponse>(jsonText);
            
            onComplete?.Invoke(false, $"Request Error: {result.message}");
        }
        
    }
}
