using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.FileModels
{
	[DataContract]
	public class FileAccountModel
	{
		[DataMember(Name = "accountId")]
		public string AccountId { get; set; }

		[DataMember(Name = "firstName")]
		public string FirstName { get; set; }

		[DataMember(Name = "lastName")]
		public string LastName { get; set; }

		[DataMember(Name = "userName")]
		public string UserName { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "sex")]
		public string Sex { get; set; }

		[DataMember(Name = "lastLogIn")]
		public string LastLogIn { get; set; }


		//PRIVATE
		[DataMember(Name = "password")]
		public string Password { get; set; }

		[DataMember(Name = "passwords")]
		public List<string> PasswordsIds { get; set; }

	}
}
