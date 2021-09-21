using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hangman.Utilities
{
    public class HttpServices
    {
        public async Task<string> GetWordMeaning(string word)
        {
            try
            {
                string serviceResult = "";
                using (var http = new HttpClient())
                {
                    var BaseAddress = new Uri($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
                    using (HttpResponseMessage response = await http.GetAsync(BaseAddress))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            serviceResult = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            serviceResult = "failure";
                        }
                    }
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                return "failure";
            }
        }

    }
}
