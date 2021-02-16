﻿using System;
using System.ComponentModel.DataAnnotations;


namespace HallOfFame.Models
{
    public class Skill
    {
        public int id { get; set; }
        public string name { get; set; }
        [Range(1, 10)]
        public byte level { get; set; }

    }
}