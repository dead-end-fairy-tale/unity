using UnityEngine;

public class UserInfo : MonoBehaviour
{

    public string username { get; private set; }
    public string accessToken { get; private set; }

    private static UserInfo instance;
    public static UserInfo Instance
    {
        get
        {
            if (instance == null) SetupInstance();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        instance = FindAnyObjectByType<UserInfo>();

        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "UserInfo";
            instance = obj.AddComponent<UserInfo>();
            DontDestroyOnLoad(obj);

        }
    }

    public void SetUserInfo(string givenUsername, string token)
    {
        username = givenUsername;
        accessToken = token;

        Debug.Log($"Set User Info by {username}");
    }
}
