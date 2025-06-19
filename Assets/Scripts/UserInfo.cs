using UnityEngine;

public class UserInfo : MonoBehaviour
{

    public static UserInfo Instance { get; private set; }

    [SerializeField] public string username;
    [SerializeField] public string accessToken;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetUserInfo(string givenUsername, string token)
    {
        username = givenUsername;
        accessToken = token;
        
        Debug.Log($"Set User Info by {username}");
    }
}
