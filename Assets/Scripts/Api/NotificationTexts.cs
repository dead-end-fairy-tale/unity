using UnityEngine;

public static class NotificationTexts
{
    //로그인 및 회원가입중 유니티 자체에서 생기는 모든 에러 메세지 및 알림 string여기에 정리
    public const string PasswordError = "Passwords must be same";
    public const string EmailVerifyError = "You must verify your email address before signing up";
    public const string TextNullError = "Text cannot be null";
    public const string VerifiedEmailChanged = "written Email has been changed after verify. Please verify your new email address";
    public const string ResetTokenError = "Your Token has been expired too long. Please login again to get new Token";
    public const string UnknownError = "Unknown Error has occured, please try again or Logout";
}
