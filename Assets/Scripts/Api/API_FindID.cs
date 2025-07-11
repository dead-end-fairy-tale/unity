using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class API_FindID : MonoBehaviour
{
    public class FindIDResponse
    {
        public bool status;
        public string message;
    }
    
    public static IEnumerator Send(string email, Action<bool, string> onComplete)
    {
        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/find-id?email={email}", "GET")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<FindIDResponse>(jsonText);
            
            onComplete?.Invoke(result.status, result.message);
        }
        else
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<FindIDResponse>(jsonText);
            
            onComplete?.Invoke(false, $"Request Error: {result.message}");
        }
    }
}