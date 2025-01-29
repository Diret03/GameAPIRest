﻿using System;
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

        public List<GameDTO>? Games { get; set; }
    }
}
