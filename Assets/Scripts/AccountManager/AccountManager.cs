using UnityEngine;

public class AccountManager : MonoBehaviour
{
    private static AccountManager instance;
    public static AccountManager Instance
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
        instance = FindAnyObjectByType<AccountManager>();

        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "AccountManager";
            instance = obj.AddComponent<AccountManager>();
            DontDestroyOnLoad(obj);

        }
    }
    
    public void OnLogoutButtonClick()
    {
        StartCoroutine(API_Logout.Send((status, message) =>
        {
            if (status)
            {
                AlertSystem.Instance.Notice(message);
            }
            else
            {
                AlertSystem.Instance.Error(message);
            }
        }));
    }
}
