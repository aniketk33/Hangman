using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hangman.Utilities
{
    public class HttpServices
    {
        public async Task<(string,HttpStatusCode)> GetWordMeaning(string word)
        {
            try
            {
                string serviceResult = "";
                using (var http = new HttpClient())
                {
                    var BaseAddress = new Uri($"{Constants.BaseUrl}{word}");
                    using (HttpResponseMessage response = await http.GetAsync(BaseAddress))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            serviceResult = await response.Content.ReadAsStringAsync();
                            return (serviceResult, response.StatusCode);
                        }
                        else
                        {
                            return (serviceResult, response.StatusCode);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return ("", HttpStatusCode.BadRequest);
            }
        }

    }
}
