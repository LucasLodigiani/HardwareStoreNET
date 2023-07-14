using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public OrderState State { get; set; }

        public string PaymentMethod { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public List<Product> Products { get; set; }
    }
}
