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

    public static IEnumerator Send(Action<bool, string> onComplete)
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
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<LogoutResponse>(jsonText);

            onComplete?.Invoke(result.status, result.message);
            UserInfo.Instance.SetUserInfo(null, null);
        }
        else
        {
            if (webRequest.responseCode == 403)
            {
                //토큰 갱신요청 -- 요청 완료된 후 성공시 현재 코루틴 다시 실행
                yield break;
            }

            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<LogoutResponse>(jsonText);

            onComplete?.Invoke(false, $"Request Error: {result.message}");
        }

    }
}

