using PasswordServerApi.Models.Enums;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Requests.Account
{
	[DataContract]
	public class StoreDocumentRequest : BaseRequest
	{
		[DataMember(Name = "documentType")]
		public DocumentType DocumentType { get; set; }

		[DataMember(Name = "fileName")]
		public string FileName { get; set; }

		[DataMember(Name = "data")]
		public byte[] Data { get; set; }
	}
}
