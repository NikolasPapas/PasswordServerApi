using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PasswordServerApi.Databases.DataModels
{
	[DataContract]
	public class NoteModel
	{
		[Key]
		[DataMember(Name = "noteId")]
		public string NoteId { get; set; }

		[Required]
		[DataMember(Name = "userId")]
		public string UserId { get; set; }

		[Required]
		[DataMember(Name = "note")]
		public string Note { get; set; }

		[DataMember(Name = "lastEdit")]
		public DateTime? LastEdit { get; set; }

	}
}
