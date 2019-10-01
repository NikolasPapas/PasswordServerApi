using System.Net.Http;

namespace PasswordServerApi.Interfaces
{
	public interface IExportService
	{
		HttpResponseMessage Export();
	}
}
