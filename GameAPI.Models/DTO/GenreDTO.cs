using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<GameResponseDTO>? Games { get; set; }
    }

    public class CreateGenreDTO
    {
        public string Name { get; set; }
    }

    public class UpdateGenreDTO
    {
        public string? Name { get; set; }
    }
}
