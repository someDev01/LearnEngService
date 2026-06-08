namespace Api.Requests;

public record UpdateLearningContentRequest(
    Guid LearningContentId,
    string? Title);
