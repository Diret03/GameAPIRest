using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models.DTO
{
    public class CreateGameDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int GenreId { get; set; }
        public int DeveloperId { get; set; }
        public List<int> PlatformIds { get; set; } = new List<int>();
    }
}
