using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Requests.Password
{
	[DataContract]
	public class SearchPasswordsRequest :BaseRequest
	{
		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "logInLink")]
		public string LogInLink { get; set; }

	}
}
