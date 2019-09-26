using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.DataSqliteDB
{
    [DataContract]
    public class EndityAbstractModelPassword
    {
        [Key]
        [DataMember(Name = "endityId")]
        public string EndityId { get; set; }

        [Required]
        [DataMember(Name = "jsonData")]
        public string JsonData { get; set; }

    }
}
