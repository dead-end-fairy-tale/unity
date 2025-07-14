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
        StartCoroutine(API_Logout.Send());
        //로그인 화면씬으로 이동
    }
}
