﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelCharacters.API.Models
{
    public class Character
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ResourceURI { get; set; }

        public Thumbnail Thumbnail { get; set; }

        public bool Liked { get; set; }
    }
}
