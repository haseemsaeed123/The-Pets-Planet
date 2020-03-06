using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FypOnEF.Models.User
{
    public class UserModel
    {
        [Key]
        public int US_Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "User Name : ")]
        public string US_Name { get; set; }

        [Required(ErrorMessage = "Contact No is required.")]
        [Display(Name = "Contact No : ")]
        public string US_Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address : ")]
        public string US_Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email : ")]
        public string US_Email { get; set; }

        public Nullable<int> U_Id { get; set; }

        [Required(ErrorMessage = "*Password is required.")]
        [DisplayName("Password:")]
        [DataType(DataType.Password)]
        public string US_Password { get; set; }

        [Required(ErrorMessage = "*Confirm Password is required.")]
        [Display(Name = "Confirm Password:")]
        [Compare("US_Password", ErrorMessage = "Password not matched.")]
        [DataType(DataType.Password)]
        public string US_ConfirmPassword { get; set; }

        public Nullable<int> IsValid { get; set; }
        public string US_ConfirmCode { get; set; }



    }
}