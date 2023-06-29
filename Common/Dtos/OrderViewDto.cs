using Common.Entities;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class OrderViewDto
    {
        public int Id { get; set; }

        public OrderState State { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}
