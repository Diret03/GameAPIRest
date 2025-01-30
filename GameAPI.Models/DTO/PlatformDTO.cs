using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI.Models.DTO
{
    public class PlatformDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PlatformResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GameResponseDTO> Games { get; set; }
    }
}
