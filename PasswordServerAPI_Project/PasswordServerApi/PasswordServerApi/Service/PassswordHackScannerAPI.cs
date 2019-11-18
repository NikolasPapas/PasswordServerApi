using Newtonsoft.Json;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.API;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using PasswordServerApi.Utilitys.Configuration;

namespace PasswordServerApi.Service
{
	public class PassswordHackScannerAPI : IPassswordHackScanner
	{
		private readonly HttpClient _client;
		private readonly IConfigurationManager _configurationManager;

		public PassswordHackScannerAPI(HttpClient client, IConfigurationManager configurationManager)
		{
			_client = client;
			_configurationManager = configurationManager;
		}

		public async Task<IsHackedResponce> IsThisEmailHacked(string email)
		{
			var streamTask = await _client.GetStringAsync(_configurationManager.GetString("HttpClient", "MainPath") + _configurationManager.GetString("HttpClient", "GetIsHacked") + $"?email={email}");
			var responce = JsonConvert.DeserializeObject<IsHackedResponce>(streamTask);
			return responce;
		}

		public async Task<HackTimeResponce> EmailHackedInfo(string email)
		{

			var json = JsonConvert.SerializeObject(new EmailHackedInfoRequest() { Email = $"?email={email}" });
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			var url = _configurationManager.GetString("HttpClient", "MainPath") + _configurationManager.GetString("HttpClient", "GetHackTimes");

			var response = await _client.PostAsync(url, data);
			return JsonConvert.DeserializeObject<HackTimeResponce>(response.Content.ReadAsStringAsync().Result);
		}

	}
}
