using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public GenreDTO Genre { get; set; }
        public DeveloperDTO Developer { get; set; }
        public List<PlatformDTO> Platforms { get; set; }
    }
}
