﻿using Common.Entities;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class OrderCreateDto
    {

        [JsonIgnore]
        public Guid? UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [Required]
        public List<int> ProductsId { get; set; }
    }
}
