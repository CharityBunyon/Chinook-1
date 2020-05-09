using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BillingCountry { get; set; }
        public decimal Total { get; set; }
    }

    public class InvoiceTotalByCountry
    {
        public string BillingCountry { get; set; }
        public decimal Total { get; set; }
    }
}
