using ScrabbleSkX.Solver;
using ScrabbleSkX.Data;

using System;

namespace ScrabbleSkX.Services
{
    public class DictService
    {
        private readonly HttpClient _httpClient;

        public DictService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
       
        public async Task<bool> ReadSKdictionary(Node root)
        {

            try
            {
                using (Stream stream = await _httpClient.GetStreamAsync("SK/dictionary/SK_2to5.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    int slovnikSize = 0;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] words = line.Split(' ');
                        foreach (string word in words)
                            if (word.Length <= 20)
                            {
                                root.Insert(word,root);
                                slovnikSize++;
                            }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
       
            return true;
        }
}
}
