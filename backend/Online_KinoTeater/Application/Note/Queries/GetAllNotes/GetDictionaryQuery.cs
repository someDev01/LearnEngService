using Application.SharedDtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Note.Queries.GetAllNotes;

public record GetDictionaryQuery(Guid UserId): IRequest<Result<List<NoteDto>>>;
