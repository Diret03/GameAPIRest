using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        // Many-to-many relationship with Games
        public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
    }
}
