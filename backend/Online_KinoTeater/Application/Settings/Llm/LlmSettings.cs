namespace Application.Settings.Llm;

public class LlmSettings
{
    public List<LlmProviderSettings> Providers { get; set; } = [];
}

public class LlmProviderSettings
{
    public string Provider {  get; set; } = string.Empty;

    public List<Model> Models { get; set; } = [];

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;
}

public class Model
{
    public string Name { get; set; } = string.Empty;

    public string Purpose {  get; set; } = string.Empty;
}