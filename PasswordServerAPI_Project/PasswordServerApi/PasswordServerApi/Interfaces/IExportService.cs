using PasswordServerApi.Models.Requests.Account;
using PasswordServerApi.Models.Responces;
using System.Net.Http;

namespace PasswordServerApi.Interfaces
{
	public interface IExportService
	{
		HttpResponseMessage Export();
		StoreDocumentResponse Import(StoreDocumentRequest request);
	}
}
