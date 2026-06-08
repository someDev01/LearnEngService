using Domain.Model.Common;

namespace Application.Interfaces.NotePolicy;

public interface INotePolicyService
{
    Task<Result> CheckCreateDailyAsync(Guid userId);

    Task IncrementCreateDailyLimitAsync(Guid userId);

    Task<Result> CheckRateLimitAsync(Guid userId);

    Task SetRateLimitAsync(Guid userId);
}
