using UnityEngine;

public class Caller : MonoBehaviour
{
    private static Caller instance;
    public static Caller Instance
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
        instance = FindAnyObjectByType<Caller>();

        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "Caller";
            instance = obj.AddComponent<Caller>();
            DontDestroyOnLoad(obj);
        }
    }
    
    
}
