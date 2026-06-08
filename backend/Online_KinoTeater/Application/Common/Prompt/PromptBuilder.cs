namespace Application.Common.Prompt;

public static class PromptBuilder
{
    public static string Build(
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

    }
}
