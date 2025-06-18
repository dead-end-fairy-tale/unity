using UnityEngine;

public class Api_SignUp : MonoBehaviour
{
    public class SignUpRequest
    {
        public string username;
        public string password;
    }
    
    public class SignUpResponse
    {
       public bool status;
       public string message;
    }
}
