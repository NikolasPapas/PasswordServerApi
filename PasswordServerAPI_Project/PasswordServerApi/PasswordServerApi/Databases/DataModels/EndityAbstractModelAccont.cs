using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PasswordServerApi.DataSqliteDB
{

    [DataContract]
    public class EndityAbstractModelAccount
    {
        [Key]
        [DataMember(Name = "endityId")]
        public string EndityId { get; set; }

		[Required]
        [DataMember(Name = "jsonData")]
        public string JsonData { get; set; }

    }
}
