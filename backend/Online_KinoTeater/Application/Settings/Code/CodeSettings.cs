namespace Application.Settings.Code;

public class CodeSettings
{
    public int ExpireSecondsCode { get; set; }
    public int ResendIntervalSeconds { get; set; }
    public int AttemptsExpireSeconds { get; set; }
    public int UserMaxAttempts { get; set; }
    public int AdminMaxAttempts { get; set; }
}
