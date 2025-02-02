using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameAPI.Models
{
    public class Developer
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string? Name { get; set; }
        [StringLength(150)]
        public string? Location { get; set; }
        public ICollection<Game>? Games { get; set; }
    }
}
