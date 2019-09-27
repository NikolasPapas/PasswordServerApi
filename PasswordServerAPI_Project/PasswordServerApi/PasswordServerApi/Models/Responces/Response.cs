using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Responces
{
	public class Response<T>
	{
		public T Payload { get; set; }

		public List<string> Warnnings { get; set; }
	}
}
