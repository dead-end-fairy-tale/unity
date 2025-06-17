using UnityEngine;

public class UserInfo : MonoBehaviour
{

    public static UserInfo instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    //필요하면 여기에 유저 정보 저장?

    public void SetUserInfo()
    {
        
    }
}
