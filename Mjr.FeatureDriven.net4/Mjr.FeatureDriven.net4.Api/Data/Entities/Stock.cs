using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mjr.FeatureDriven.net4.Api.Data.Entities
{
    public class Stock
    {
        public int Id { get; set; }
        [Required]
        [StringLength(400)]
        public string Name { get; set; }
        [Required]
        [StringLength(16)]
        public string IsinCode { get; set; }
        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; }
        public List<StockHistory> StockHistory { get; set; }
        [Required]
        public DateTimeOffset CreationDateTime { get; set; }
    }
}