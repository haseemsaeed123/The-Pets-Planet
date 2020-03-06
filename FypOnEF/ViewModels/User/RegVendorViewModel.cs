using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FypOnEF.ViewModels.User
{
    public class RegVendorViewModel
    {
        public int U_Id { get; set; }

        [Required(ErrorMessage = "*Username is required.")]
        [DisplayName("User Name:")]
        [RegularExpression(@"^([a-zA-Z][\w.]+|[0-9][0-9_.]*[a-zA-Z]+[\w.]*)$", ErrorMessage = "Enter valid username.")]
        [System.Web.Mvc.Remote("IsUser", "User", ErrorMessage = "Username is already taken.")]
        public String US_Name { get; set; }

        [Required(ErrorMessage = "*Contact No is required.")]
        [DisplayName("Contact No:")]
        [Range(03000000000, 03999999999, ErrorMessage = "Must be 11 Digit Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [System.Web.Mvc.Remote("IsPhoneInUse", "User", ErrorMessage = "Phone No is already in someone else use.")]
        public String US_Phone { get; set; }

        [Required(ErrorMessage = "*Address is required.")]
        [DisplayName("Address:")]
        public String US_Address { get; set; }

        [Required(ErrorMessage = "*Email Id is required.")]
        [DisplayName("Email Id:")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [System.Web.Mvc.Remote("IsEmailInUse", "User", ErrorMessage = "Email is already taken.")]
        public String US_Email { get; set; }

        [Required(ErrorMessage = "*Password is required.")]
        [DisplayName("Password:")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "*Must be Greater than 8 character")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,20}$", ErrorMessage = "Password must include upper case,lower case letter and digits and special character.")]
        [DataType(DataType.Password)]
        public String US_Password { get; set; }

        [Required(ErrorMessage = "*Confirm Password is required.")]
        [DisplayName("Confirm Password:")]
        [Compare("US_Password", ErrorMessage = "Password not matched.")]
        [DataType(DataType.Password)]
        public String US_ConfirmPassword { get; set; }

        public int IsValid { get; set; }

        public string US_ConfirmCode { get; set; }

        public int VI_Id { get; set; }

        [Required(ErrorMessage = "*Enter 13 digit CNIC No")]
        [DisplayName("CNIC:")]
        //[Range(0000000000000, 0099999999999, ErrorMessage = "Must be 13 Digit valid CNIC Number")]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "Entered CNIC format is not valid.")]
        [System.Web.Mvc.Remote("IsCNICInUse", "User", ErrorMessage = "CNIC is already in someone else use.")]
        public String VI_CNIC { get; set; }

        [Required(ErrorMessage = "*Enter Province.")]
        [DisplayName("Province:")]
        public String VI_Province { get; set; }

        [Required(ErrorMessage = "*Enter City.")]
        [DisplayName("City:")]
        public String VI_City { get; set; }

        public int VendorImage_Id { get; set; }
        public string VendorImage_ImagePath { get; set; }

        [Required(ErrorMessage = "*Please upload your CNIC image.")]
        [DisplayName("Upload CNIC:")]
        public HttpPostedFileBase ImageFile { get; set; }
        public string VendorImage_Name { get; set; }
    }
}