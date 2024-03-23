using CurrencyService.Contracts;

namespace CurrencyService.Services
{
    public class ApiCallService : IApiCallService
    {
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling API: {ex.Message}");

                return new HttpResponseMessage();
            }
        }
    }
}
