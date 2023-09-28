namespace ApiTopicTwisterQuark.Entities.DTOs;

[Serializable]
public class LoginResultDTO
{
    public string ErrorMessage;
    public bool SuccessAuthentication;

    public LoginResultDTO(string errorMessage, bool successAuthentication)
    {
        ErrorMessage = errorMessage;
        SuccessAuthentication = successAuthentication;
    }
}