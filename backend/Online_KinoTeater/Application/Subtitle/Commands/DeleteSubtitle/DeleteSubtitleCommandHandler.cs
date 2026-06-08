using Application.Interfaces.Storage;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;
using Domain.Repositories.Subtitle;
using MediatR;

namespace Application.Subtitle.Commands.DeleteSubtitle;

public class DeleteSubtitleCommandHandler(
    ISubtitleRepository subtitleRepository,
    IFileStorageService fileStorageService,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteSubtitleCommand, Result>
{
    public async Task<Result> Handle(DeleteSubtitleCommand request, CancellationToken cancellationToken)
    {
        var subtitleExisting = await subtitleRepository.GetByIdAsync(request.SubtitleId, cancellationToken);

        if(subtitleExisting is null)
            return Result.Failure($"Субтитр с таким Id {request.SubtitleId} не найден!");

        await fileStorageService.DeleteAsync(request.FileKey, cancellationToken);

        await subtitleRepository.DeleteAsync(subtitleExisting, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
