using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Display(Name = "*Nama Depan")]
        [Required(ErrorMessage = "Nama Depan harus diisi.")]
        [StringLength(40, ErrorMessage = "Panjang karakter Nama Depan maksimal 40.")]
        public string FirstName { get; set; }

        [Display(Name = "*Nama Belakang")]
        [Required(ErrorMessage = "Nama Belakang harus diisi.")]
        [StringLength(40, ErrorMessage = "Panjang karakter Nama Belakang maksimal 40.")]
        public string LastName { get; set; }

        [Display(Name = "Kota")]
        [StringLength(40, ErrorMessage = "Panjang karakter Kota maksimal 40.")]
        public string City { get; set; }

        [Display(Name = "Negara")]
        [StringLength(40, ErrorMessage = "Panjang karakter Negara maksimal 40.")]
        public string Country { get; set; }

        [Display(Name = "No. Telepon")]
        [DataType(DataType.PhoneNumber)]

        /* (123) 456 7899
        (123).456.7899
        (123)-456-7899
        123-456-7899
        123 456 7899
        1234567899 */

        [RegularExpression("^\\(?([0-9]{3})\\)?([ .-]?)([0-9]{3})\\2([0-9]{4})$", ErrorMessage = "Mohon masukkan No. Telepon yang valid.")]
        [StringLength(40, ErrorMessage = "Panjang karakter No. Telepon maksimal 20.")]
        public string Phone { get; set; }

        [Display(Name = "Alamat E-mail")]
        [EmailAddress(ErrorMessage = "Format penulisan e-mail tidak valid.")]
        [StringLength(40, ErrorMessage = "Panjang karakter Alamat E-mail maksimal 35.")]
        public string Email { get; set; }
    }
}
