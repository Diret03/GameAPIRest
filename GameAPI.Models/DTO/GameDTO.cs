using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models.DTO
{
    public class GameResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public GenreDTO Genre { get; set; }
        public DeveloperDTO Developer { get; set; }
        public List<PlatformDTO> Platforms { get; set; }
    }

    public class CreateGameDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int GenreId { get; set; }
        public int DeveloperId { get; set; }
        public List<int> PlatformIds { get; set; } = new List<int>();
    }

    public class UpdateGameDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? GenreId { get; set; }
        public int? DeveloperId { get; set; }
        public List<int>? PlatformIds { get; set; }
    }
}
