using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Game>? Games { get; set; } = new List<Game>();
    }
}

