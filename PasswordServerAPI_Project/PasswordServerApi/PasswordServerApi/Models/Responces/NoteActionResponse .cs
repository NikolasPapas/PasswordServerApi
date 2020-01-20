using PasswordServerApi.Models.DTO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Responces
{
	[DataContract]
	public class NoteActionResponse : BaseResponse
	{
		[DataMember(Name = "notes")]
		public List<NoteDto> Notes { get; set; }
	}
}