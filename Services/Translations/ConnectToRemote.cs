using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
namespace Translation.Services.Translations;

using Translation.Model.Translations;

public class ConnectToRemote : TranslationConfiguration
{
    private string _fromLanguageCode;
    public ICollection<string> ToLanguageCode = new HashSet<string>();
    public ConnectToRemote(string fromLanguageCode) => _fromLanguageCode = fromLanguageCode;

    /// <summary>
    /// Get the translation from the remote service
    /// </summary>
    /// <param name="text">source text</param>
    /// <returns></returns>
    public async Task<ICollection<TranslationResponse>> GetTranslation(string text)
    {
        string route = $"{GetRouteParameter()}&from={_fromLanguageCode}&{String.Join("&", ToLanguageCode.Select(x => $"to={x}"))}";
        object[] body = new object[] { new { Text = text.Trim() } };

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            // Build the request.
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(endpoint + route);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", location);

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            // Read response as a string.
            string result = await response.Content.ReadAsStringAsync();
            var responses = JsonConvert
            .DeserializeObject<List<Response>>(result)?
            .FirstOrDefault()?
            .Translations ?? new HashSet<TranslationResponse>();
            return responses;
        }
    }
    
    /// <summary>
    /// Set the target language code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public ConnectToRemote SetToLanguage(string code)
    {
        this.ToLanguageCode.Add(code);
        return this;
    }
}