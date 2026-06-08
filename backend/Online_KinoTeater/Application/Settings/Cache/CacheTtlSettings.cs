namespace Application.Settings.Cache;

public class CacheTtlSettings
{
    public int NotesTtlSeconds { get; set; }
    public int PagedNotesTtlMinutes { get; set; }
    public int PagedVideosTtlMinutes { get; set; }
    public int SubtitlesTtlSeconds { get; set; }
    public int TranslationTtlHours { get; set; }
    public int LlmVocabularyTtlHours { get; set; }
    public int RateLimitNoteTtlSeconds { get; set; }
}
