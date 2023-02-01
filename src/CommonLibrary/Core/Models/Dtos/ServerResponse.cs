namespace CommonLibrary.Core.Dtos;

public class ErrorResponse
{
    public string ErrorMessage { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorHelpLink { get; set; } = string.Empty;
}