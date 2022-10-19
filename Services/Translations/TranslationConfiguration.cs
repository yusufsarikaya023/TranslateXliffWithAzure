using Translation.Model.Translations;
using Microsoft.Extensions.Configuration;
namespace Translation.Services.Translations;

public abstract class TranslationConfiguration
{
    public RemoteSetting? setting;
    protected readonly string? subscriptionKey;
    protected readonly string? endpoint;
    protected readonly string? location;
    public TranslationConfiguration()
    {
        setting = Configuration
            .AppSettings
            .GetRequiredSection("Azure")
            .Get<RemoteSetting>();
        this.subscriptionKey = setting.Key;
        this.endpoint = setting.Endpoint;
        this.location = setting?.Location;
    }
    public string GetRouteParameter() => $"/translate?api-version=3.0&";
}