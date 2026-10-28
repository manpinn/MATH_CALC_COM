using System.Text;
using System.Text.Json;

namespace MATH_CALC_COM.Services.Ollama
{
    public class OllamaService
    {
        private readonly HttpClient _httpClient;

        public OllamaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AskAsync(string prompt)
        {
            var request = new
            {
                model = "phi4",
                prompt = prompt,
                stream = false
            };

            var json = JsonSerializer.Serialize(request);

            var response = await _httpClient.PostAsync(
                "http://localhost:11434/api/generate",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);

            return doc.RootElement
                      .GetProperty("response")
                      .GetString();
        }
    }
}
