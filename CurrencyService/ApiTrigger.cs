using Core.Contracts;
using CurrencyService.Contracts;
using Data.Models.DTO;
using Newtonsoft.Json;

namespace CurrencyService
{
    public class ApiTrigger
    {
        private readonly IApiCallService _apiService;
        private readonly ICurrencyService _currencyService;

        private const string API_URL = "";

        public ApiTrigger(IApiCallService apiService, ICurrencyService currencyService)
        {
            _apiService = apiService;
            _currencyService = currencyService;
        }

        public void ScheduleTrigger()
        {
            var timer = new Timer(CallApi, null, GetTimeUntilMidnight(), 1);
        }

        private async void CallApi(object state)
        {
            var response = await _apiService.GetAsync(API_URL);
            
            var responseJson = await response.Content.ReadAsStringAsync();
            var currencyInfoDto = JsonConvert.DeserializeObject<CurrencyInfoDto>(responseJson);

            await _currencyService.InsertCurrencyDetails(currencyInfoDto, new CancellationToken());
        }

        private int GetTimeUntilMidnight()
        {
            var now = DateTime.Now;
            var midnight = now.Date.AddDays(1);
            var timeUntilMidnight = midnight - now;
            return (int) timeUntilMidnight.TotalMinutes;
        }
    }
}
