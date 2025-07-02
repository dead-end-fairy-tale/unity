using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class API_ChangePassword : MonoBehaviour
{
    public class ChangePasswordResponse
    {
        public bool status;
        public string message;
    }

    public static IEnumerator Send(string password)
    {

        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/change-password?password={password}", "POST")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", $"Bearer {UserInfo.Instance.accessToken}");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<ChangePasswordResponse>(jsonText);

            AlertSystem.Instance.Notice(result.message);
        }
        else
        {
            if (webRequest.responseCode == 403)
            {
                Debug.Log(webRequest.error);
                yield return API_Token.Send();
                
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", $"Bearer {UserInfo.Instance.accessToken}");

                yield return webRequest.SendWebRequest();
                
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string jsonTextAgain = webRequest.downloadHandler.text;
                    var resultAgain = JsonConvert.DeserializeObject<API_Logout.LogoutResponse>(jsonTextAgain);

                    AlertSystem.Instance.Notice(resultAgain.message);
                }
                else
                {
                    string jsonTextAgain = webRequest.downloadHandler.text;
                    var resultAgain = JsonConvert.DeserializeObject<API_Logout.LogoutResponse>(jsonTextAgain);
                    
                    Debug.Log(resultAgain.message);
                    AlertSystem.Instance.Error(NotificationTexts.UnknownError);
                }
            }
            
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<API_Logout.LogoutResponse>(jsonText);
            
            AlertSystem.Instance.Error(result.message);
        }



    }
}
