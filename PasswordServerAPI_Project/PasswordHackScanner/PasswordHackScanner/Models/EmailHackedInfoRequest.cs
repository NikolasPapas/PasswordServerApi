using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PassswordHackScanner.Models
{
	[DataContract]
	public class EmailHackedInfoRequest
	{
		[DataMember(Name = "email")]
		public string Email { get; set; }
	}
}
