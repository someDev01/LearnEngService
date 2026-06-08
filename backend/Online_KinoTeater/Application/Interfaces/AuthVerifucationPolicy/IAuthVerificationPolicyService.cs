namespace Application.Interfaces.AuthVerifucationPolicy;

public interface IAuthVerificationPolicyService
{
    Task<bool> CanSendCodeAsync(string email);

    Task<TimeSpan> LockCodeSendingAsync(string email);

    Task<bool> IsVerificationAttemptsBlockedAsync(string email, long maxAttempts);

    Task IncrementVerificationAttemptsAsync(string email);

    Task ResetVerificationAttemptsAsync(string email);
}
