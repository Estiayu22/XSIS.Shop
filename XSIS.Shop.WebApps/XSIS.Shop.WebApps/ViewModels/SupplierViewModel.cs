using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.WebApps.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        [Display(Name = "*Nama Perusahaan")]
        [Required(ErrorMessage = "Nama Perusahaan harus diisi.")]
        [StringLength(40, ErrorMessage = "Panjang karakter Nama Perusahaan maksimal 40.")]
        public string CompanyName { get; set; }

        [Display(Name = "Nama Kayawan")]
        [StringLength(50, ErrorMessage = "Panjang karakter Nama PIC maksimal 50.")]
        public string ContactName { get; set; }

        [Display(Name = "Gelar")]
        [StringLength(40, ErrorMessage = "Panjang karakter Gelar maksimal 40.")]
        public string ContactTitle { get; set; }

        [Display(Name = "Kota")]
        [StringLength(40, ErrorMessage = "Panjang karakter Kota maksimal 40.")]
        public string City { get; set; }

        [Display(Name = "Negara")]
        [StringLength(40, ErrorMessage = "Panjang karakter Negara maksimal 40.")]
        public string Country { get; set; }

        [Display(Name = "No. Telepon")]
        [StringLength(30, ErrorMessage = "Panjang karakter No. Telepon maksimal 30.")]
        public string Phone { get; set; }

        [Display(Name = "No. Faksimile")]
        [StringLength(30, ErrorMessage = "Panjang karakter No. Faksimile maksimal 30.")]
        public string Fax { get; set; }
    }
}