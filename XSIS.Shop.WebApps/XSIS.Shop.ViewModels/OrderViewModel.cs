using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tanggal Order")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "No. Order")]
        public string OrderNumber { get; set; }

        [Display(Name = "Nama Pembeli")]
        public string CustomerName { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Total Harga")]
        public Nullable<decimal> TotalAmount { get; set; }
    }

    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
