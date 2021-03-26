using Abp.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AB.Domain.Models
{
    public class tblRecord
    {
        [JsonProperty(PropertyName = nameof(Id))]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonProperty(PropertyName = nameof(RegistrationDate))]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonProperty(PropertyName = nameof(LastActivityDate))]
        public DateTime LastActivityDate { get; set; }
    }
}
