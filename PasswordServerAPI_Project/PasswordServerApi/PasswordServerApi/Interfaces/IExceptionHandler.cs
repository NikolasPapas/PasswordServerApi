using PasswordServerApi.Models.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	public interface IExceptionHandler
	{
		Response<T> HandleException<T>(Func<T> action, Guid requestId) where T : class;
	}
}
