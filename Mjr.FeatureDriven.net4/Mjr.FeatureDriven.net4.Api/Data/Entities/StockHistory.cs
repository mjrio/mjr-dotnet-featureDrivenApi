using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mjr.FeatureDriven.net4.Api.Data.Entities
{
    public class StockHistory
    {
        public int Id { get; private set; }

        [Required]
        public Stock Stock { get; set; }

        public int StockId { get; set; }

        [Required]
        public DateTimeOffset Date { get; private set; }

        [Required]
        public decimal Value { get; private set; }

    }
}