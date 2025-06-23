using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_ResetPassword : MonoBehaviour
{
    public class ResetPasswordResponse
    {
        public bool status;
        public string message;
    }
    
    public static IEnumerator Send(string email, Action<bool, string> onComplete)
    {
        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/reset-password?email={email}", "PATCH")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<ResetPasswordResponse>(jsonText);
            
            onComplete?.Invoke(result.status, result.message);
        }
        else
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<ResetPasswordResponse>(jsonText);
            
            onComplete?.Invoke(false, $"Request Error: {result.message}");
        }
        
    }

}
