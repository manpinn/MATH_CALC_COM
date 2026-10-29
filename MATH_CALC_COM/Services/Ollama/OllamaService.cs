using MathNet.Numerics;
using MathNet.Numerics.Distributions;
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
            var fullPrompt = $"""
You are a mathematics tutor.
Ask the user at the begining:
"Hello! Shall I explain linear regression to you?"

linear regression will be explained and 
qr decomposition and show the formulas.
explain why qr decomposition is needed.
as answer numerical stability.
explain numerical stability.
everything with formulas.

Any output language can be chosen by the user.

IMPORTANT FORMATTING RULES:

- Return Markdown only.
- Never generate HTML.
- Never use tags such as:
  <div>
  <span>
  <ul>
  <ol>
  <li>
  <strong>
  <em>
  <code>

- Use Markdown lists only:

  - Item 1
  - Item 2

- Mathematical formulas must be written ONLY with LaTeX.

Inline formula example:

\\( x^2 + y^2 = z^2 \\)

Display formula example:

$$
y = \\beta_0 + \\beta_1 x + \\epsilon
$$

Use:
$$ ... $$ for display math
\( ... \) for inline math

- Never output formulas as plain Unicode text.
- Never duplicate formulas.
- Never output the same formula twice.
- Do not explain the formula and then repeat it in HTML.
- Never use [ formula ] notation.
- Never use <br> tags.
- Use real line breaks.

GOOD EXAMPLE:

## Simple Linear Regression

The model is:

$$
y = \\beta_0 + \\beta_1 x + \\epsilon
$$

Where:

- \\(y\\) is the dependent variable.
- \\(x\\) is the independent variable.
- \\(\\beta_0\\) is the intercept.
- \\(\\beta_1\\) is the slope.

Before outputting a formula, verify that all matrix rows are separated by \\.

USER QUESTION:

{prompt}
""";

            var request = new
            {
                model = "llama3.2:1b",
                prompt = fullPrompt,
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
