namespace Application.Common.Cache;

public static class CacheKeyBuilder
{
    #region KEYS EMAIL
    public static string BuildCodeKey(string email) => $"code-key:{email.ToLower().Trim()}";
    public static string BuildResendKey(string email) => $"resend-key:{email.ToLower().Trim()}";
    public static string BuildAttemptsKey(string email) => $"attempts-key:{email.ToLower().Trim()}";
    public static string BuildRevokeKey(string jti) => $"revoke-key:{jti.ToLower().Trim()}";
    #endregion

    #region KEYS CONTENTS
    public static string BuildContentsListKey(string key, int version, int page, int pageSize) =>
        $"contents-key:{key.ToLower()}:v{version}-{page}-{pageSize}";

    public static string BuildGetAllLearningContetnsKey() => $"all-learningContents-key";
    #endregion

    #region KEYS SUBTITLES
    public static string BuildSubtitlesKey(string videoId) => $"subtitles-by-video-key:{videoId.Trim()}";
    #endregion

    #region KEYS VIDEOS
    public static string BuildGetAllVideosKey(int page, int pageSize) => $"videos=paged-key:page:{page}-pageSize:{pageSize}";

    public static string BuildResetPagedVideoKey() => $"videos-paged-key:*";
    #endregion

    #region KEYS LLM
    public static string BuildLlmGlobalLimitsKey() => $"llm-global-limits-key";
    public static string BuildLlmUserLimitsKey(string userId) => $"llm-user-limits-key:{userId.Trim()}";

    public static string BuildLlmVocabularyKey(string value) => $"llm-vocabulary-key:{value.Trim()}";

    public static string BuildLlmModelLimitKey(string model) => $"llm-model-limit-key:{model}";
    #endregion

    #region KEYS TRANSLATION
    public static string BuildTranslateWordKey(string word) => $"translation-key:{word.Replace(" ", "").Trim()}";
    #endregion

    #region KEYS SEARCH
    public static string BuildLearningContentSearchKey(string value) => $"search-key:{value.Trim()}";
    #endregion

    #region KEYS NOTES
    public static string BuildNotesListKey(string userId) => $"notes-key:{userId.Trim()}";

    public static string BuildNoteRateLimit(string userId) => $"note-rate-limit-key:{userId.Trim()}";

    public static string BuildPagedNotesKey(string userId, int page, int pageSize) => 
        $"notes-paged-key:{userId.Trim()}-page:{page}-pageSize:{pageSize}";

    public static string BuildResetPagedNotesKey(string userId) => $"notes-paged-key:{userId.Trim()}-*";
    #endregion
}
