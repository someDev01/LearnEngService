using Domain.Model.Common;

namespace Application.Interfaces.Translate;

public interface ITranslateService
{
    Task<Result<string?>> TranslateAsync(string word, CancellationToken cancellationToken);
}
