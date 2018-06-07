using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "*Nama Produk")]
        [Required(ErrorMessage = "Nama Produk harus diisi.")]
        [StringLength(50, ErrorMessage = "Panjang karakter Nama Produk maksimal 50.")]
        public string ProductName { get; set; }

        [Display(Name = "*Nama Supplier")]
        [Required(ErrorMessage = "Nama Supplier harus diisi.")]
        public int SupplierId { get; set; }

        [Display(Name = "Nama Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Harga Produk (Rp.)")]
        [Range(0, 99999999999.99, ErrorMessage = "Harga Produk harus lebih dari 0.00")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Harga Produk tidak boleh memiliki lebih dari 2 angka desimal.")]
        public Nullable<decimal> UnitPrice { get; set; }

        [Display(Name = "Kemasan")]
        [StringLength(30, ErrorMessage = "Panjang karakter Kemasan maksimal 50.")]
        public string Package { get; set; }

        [Display(Name = "Kadaluarsa")]
        [Required(ErrorMessage = "Kadaluarsa harus diisi.")]
        public bool IsDiscontinued { get; set; }
    }
}
