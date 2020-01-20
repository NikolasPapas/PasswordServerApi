using PasswordServerApi.DTO;
using PasswordServerApi.Models.DTO;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Requests.Note
{
	[DataContract]
	public class NoteActionRequest : BaseRequest
	{
		[DataMember(Name = "note")]
		public NoteDto Note { get; set; }
	}
}
