using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.DTO
{
	[DataContract]
	public class NoteDto
	{
		[DataMember(Name = "noteId")]
		public Guid NoteId { get; set; }

		[DataMember(Name = "userId")]
		public Guid UserId { get; set; }

		[DataMember(Name = "note")]
		public string Note { get; set; }

		[DataMember(Name = "lastEdit")]
		public DateTime ?LastEdit { get; set; }
	}
}
