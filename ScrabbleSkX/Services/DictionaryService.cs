using System.Net.Http;
using System.Threading.Tasks;

namespace ScrabbleSkX.Services;

public class DictionaryService
{
    private readonly HttpClient _http;
    private string[]? _cachedWords;

    public DictionaryService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string[]> GetWordsAsync()
    {
        if (_cachedWords is not null)
            return _cachedWords;

        var url = "https://lubomirb.github.io/SlovnikSK/slovnik.txt";
        var text = await _http.GetStringAsync(url);

        _cachedWords = text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);

        return _cachedWords;
    }
}
