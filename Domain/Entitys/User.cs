using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entitys
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name requires a maximum of 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email requires a maximum of 100 characters")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is invalid")]
        [StringLength(15, ErrorMessage = "Phone number requires a maximum of 15 characteres")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [StringLength(128)] //is default result hash
        public string Password { get; set; }

        public DateTime? NextPasswordUpdate { get; set; }

        public DateTime Date { get; set; }
    }
}
