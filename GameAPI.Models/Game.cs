using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }

        // Navigation properties
        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }

        public int DeveloperId { get; set; }
        [ForeignKey("DeveloperId")]
        public Developer? Developer { get; set; }

        // Many-to-many relationship with Platforms
        public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
    }
}
