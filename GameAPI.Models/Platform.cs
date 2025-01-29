using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameAPI.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public ICollection<GamePlatform>? GamePlatforms { get; set; }
    }
}
