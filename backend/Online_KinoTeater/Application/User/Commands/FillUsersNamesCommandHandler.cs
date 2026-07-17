using Application.Interfaces.Context;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.User.Commands;
//к удалению
public class FillUsersNamesCommandHandler(IDataContext context, IUnitOfWork unitOfWork): IRequestHandler<FillUsersNamesCommand, Result>
{
    public async Task<Result> Handle(FillUsersNamesCommand request, CancellationToken cancellationToken)
    {
        var users = await context.Users.ToListAsync(cancellationToken);
        
        foreach (var user in users)
        {
            var result = user.InitializeName();

            if (!result.IsSuccess)
                return Result.Failure(result.Error!);
            
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return Result.Success();
    }
}