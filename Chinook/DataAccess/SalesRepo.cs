using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Chinook.Models;
using Dapper;

namespace Chinook.DataAccess
{
    public class SalesRepo
    {
        const string ConnectionString = "Server=localhost;Database=Chinook;Trusted_Connection=True;";

        public IEnumerable<InvoiceTotalByCountry> GetInvoicesGroupedByCountry()
        {
            var sql = @"
                        select Invoice.BillingCountry, sum(Invoice.Total) as Total
                        from Invoice
                        group by Invoice.BillingCountry 
                      ";

            using (var db = new SqlConnection(ConnectionString))
            {
                //var parameters = new { Country = country } << Need to get the sum of the invoice ;
                var result = db.Query<InvoiceTotalByCountry>(sql);
                return result;
            }
        }

        
        public IEnumerable<InvoiceWithCustomerInfo> GetInvoicesWithCustomers()
        {
            var sql = @"
                        select i.InvoiceId,c.CustomerId, i.InvoiceDate, i.Total as InvoiceTotal, c.FirstName + ' ' + c.LastName as CustomerName
                        from Invoice i  
                            join customer c
                                on i.customerid = c.customerid
                      ";

            using (var db = new SqlConnection(ConnectionString))
            {
                //var parameters = new { Country = country } << Need to get the sum of the invoice ;
                var result = db.Query<InvoiceWithCustomerInfo>(sql);
                return result;
            }
        }
        
        public IEnumerable<InvoiceWithCustomerAndTrackInfo> GetInvoicesWithCustomersAndTracks()
        {
            var sql = @"
                        select i.InvoiceId,c.CustomerId, i.InvoiceDate, i.Total, c.FirstName + ' ' + c.LastName as CustomerName, il.TrackId
                        from Invoice i  
                            join customer c
                                on i.customerid = c.customerid
	                        join InvoiceLine il
		                        on i.InvoiceId = il.InvoiceId
                      ";

            using (var db = new SqlConnection(ConnectionString))
            {
                //var parameters = new { Country = country } << Need to get the sum of the invoice ;
                var result = db.Query<InvoiceWithCustomerAndTrackInfo>(sql);
                return result;
            }
        }
        
        public IEnumerable<InvoiceWithCustomerAndTrackInfoPart2> GetInvoicesWithCustomersAndTracksPart2()
        {
            var sql = @"
                        select i.InvoiceId,c.CustomerId, i.InvoiceDate, i.Total, c.FirstName + ' ' + c.LastName as CustomerName
                        from Invoice i  
                            join customer c
                                on i.customerid = c.customerid
                      ";

            var invoiceLineQuery = "select trackid, invoiceid from invoiceline";

            using (var db = new SqlConnection(ConnectionString))
            {
                //var parameters = new { Country = country } << Need to get the sum of the invoice ;
                var result = db.Query<InvoiceWithCustomerAndTrackInfoPart2>(sql);
                var invoiceLines = db.Query<InvoiceTrack>(invoiceLineQuery);

                foreach (var info in result)
                {
                    info.Tracks = invoiceLines.Where(il => il.InvoiceId == info.InvoiceId).Select(il => il.TrackId);
                }

                return result;
            }
        }
    }

    public class InvoiceWithCustomerInfo
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string CustomerName { get; set; }
    }

    
    public class InvoiceWithCustomerAndTrackInfo
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string CustomerName { get; set; }
        public int TrackId { get; set; }
    }


    public class InvoiceTrack
    {
        public int InvoiceId { get; set; }
        public int TrackId { get; set; }
    }
    
    public class InvoiceWithCustomerAndTrackInfoPart2
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<int> Tracks { get; set; }
    }
}
