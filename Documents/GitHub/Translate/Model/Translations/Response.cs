namespace Translation.Model.Translations;

public class Response
{
    public ICollection<TranslationResponse> Translations { get; set; } = new HashSet<TranslationResponse>();
}

public class TranslationResponse
{
    public string? Text { get; set; }
    public string? To { get; set; }
}