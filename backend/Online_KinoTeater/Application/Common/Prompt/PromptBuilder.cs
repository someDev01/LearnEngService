using Application.SharedDtos;

namespace Application.Common.Prompt;

public static class PromptBuilder
{
    /*public static string Build(
        string text, 
        string? context = null, 
        bool isIncludedTranslations = true, 
        bool isIncludedExamples = true)
    {
        string contextPart = context is not null ?
            $@"Context sentence:
            ""{context}""" : "";

        string translationsPart = isIncludedTranslations ?
            $@"- ""translations"": array of EXACTLY 3 Russian translations:
          - translations must be UNIQUE (no duplicates)
          - ordered from most common/basic to more advanced or less obvious meanings" :
          "";

        string examplesPart = isIncludedExamples ?
            $@"- ""examples"": array of EXACTLY 3 objects:
                - each object has:
                - ""Text"": English sentence
                - ""Translate"": Russian translation of the sentence" :
            "";

        var formatParts = new List<string>() 
        {
             @"""Word"":""english_word"""
        };

        if (isIncludedTranslations)
            formatParts.Add(@"""Translations"":[""t1"",""t2"",""t3""]");

        formatParts.Add(@"""Transcription"":""IPA""");

        if (isIncludedExamples)
            formatParts.Add(@"""Examples"":[{""Text"":""example1"",""Translate"":""пример1""},{""Text"":""example2"",""Translate"":""пример2""}]");

        formatParts.Add(@"""Level"":""B2""");
        var format = "{" + string.Join(",", formatParts) + "}";

        return $@"
            Translate the given word or phrase ""{text}"".

            {contextPart}

            Return ONLY valid JSON.

            Requirements:
            - If the input is Russian, first determine the correct English equivalent.
            - Always return the final word in English.
            - ""Word"": canonical English word or phrase
            {translationsPart}
            - ""translations"" must ALWAYS contain Russian translations of the English word
            - ""transcription"": IPA transcription of the English word
            {examplesPart}
            - Examples must ALWAYS be in English with Russian translations
            - ""level"": English level (A1–C2) based on frequency:
                A1–A2 = very common
                B1–B2 = medium
                C1–C2 = rare/advanced

            Rules:
            - Use EXACT property names from the format
            - Property names are case-sensitive
            - DO NOT repeat translations
            - DO NOT explain anything
            - JSON must be valid
            - JSON must be in ONE LINE

            Format:
            {format}
        ";

    }*/
    
    public static string Build(
    string text,
    string? context = null,
    List<string>? translations = null,
    List<ExampleDto>? examples = null)
{
    string contextPart = context != null
        ? $@"Context sentence:
        ""{context}"""
        : "";

    string translationsPart;

    if (translations != null && translations.Count > 0)
    {
        translationsPart = $@"
        User-provided translations:
        {string.Join(", ", translations.Select(x => $@"""{x}"""))}

        IMPORTANT:
        - These translations were provided by the user.
        - Preserve them EXACTLY as provided.
        - Do not modify, replace or remove them.
        - Generate additional translations only if fewer than 3 were provided.
        - All translations must be unique.";
    }
    else
    {
        translationsPart = @"
        No translations were provided by the user.
        Generate exactly 3 unique Russian translations.
        Order them from the most common/basic meaning to less obvious meanings.";
    }

    string examplesPart;

    if (examples != null && examples.Count > 0)
    {
        examplesPart = $@"
        User-provided examples:

        {string.Join(
            Environment.NewLine,
            examples.Select(x =>
                $@"- ""Text"": ""{x.Text}""
                   ""Translate"": ""{x.Translate}"""))}

        IMPORTANT:
        - These examples were provided by the user.
        - Preserve them EXACTLY as provided.
        - Do not modify, replace or remove them.
        - Generate additional examples only if fewer than 3 were provided.";
    }
    else
    {
        examplesPart = @"
        No examples were provided by the user.
        Generate exactly 3 examples:
        - each example must contain an English sentence;
        - each example must contain its Russian translation.";
    }

    var format = """
    {
        "Word":"english_word",
        "Translations":["t1","t2","t3"],
        "Transcription":"IPA",
        "Examples":[
            {"Text":"example1","Translate":"пример1"},
            {"Text":"example2","Translate":"пример2"},
            {"Text":"example3","Translate":"пример3"}
        ],
        "Level":"B2"
    }
    """;

    return $@"
        Translate the given word or phrase ""{text}"".

        {contextPart}

        Return ONLY valid JSON.

        USER-PROVIDED DATA:
        User-provided data is authoritative.
        Preserve all user-provided data EXACTLY.
        Generate only missing data.

        Requirements:

        - If the input is Russian, first determine the correct English equivalent.
        - Always return the final word in English.
        - ""Word"": canonical English word or phrase.

        TRANSLATIONS:
        {translationsPart}

        - ""Translations"" must contain Russian translations of the English word.

        - ""Transcription"": IPA transcription of the English word.

        EXAMPLES:
        {examplesPart}

        - ""Examples"" must contain English sentences with Russian translations.

        LEVEL:

        - ""Level"": Determine the CEFR English level of the word or phrase.
        - The level MUST be exactly one of: A1, A2, B1, B2, C1, C2.
        - Determine the level specifically for the word or phrase, not for the sentence and not for the user's English level.

        CEFR guidelines: - A1: extremely common and basic words used in everyday situations.
        - A2: common words that beginners frequently encounter but are slightly less basic.
        - B1: common intermediate vocabulary used in everyday communication.
        - B2: less common or more precise vocabulary that an intermediate learner may need to learn.
        - C1: advanced vocabulary, abstract concepts, formal language, or words with nuanced meanings.
        - C2: very advanced, rare, highly specialized, literary, or sophisticated vocabulary.

        IMPORTANT LEVEL RULES:
        - Do NOT default to B1.
        - Do NOT assign the same level to every word.
        - Determine the level independently for each word.
        - Choose the level that best matches the actual difficulty and frequency of the word.

        Rules:

        - User-provided translations and examples have priority over generated data.
        - Never rewrite, replace or regenerate user-provided translations.
        - Never rewrite, replace or regenerate user-provided examples.
        - Generate only information that was not provided by the user.
        - Use EXACT property names from the format.
        - Property names are case-sensitive.
        - Do NOT repeat translations.
        - Do NOT explain anything.
        - JSON must be valid.
        - JSON must be in ONE LINE.

        Format:
        {format}
    ";
}
}
