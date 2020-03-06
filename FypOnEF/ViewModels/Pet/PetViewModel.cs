using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypOnEF.ViewModels.Pet
{
    public class PetViewModel
    {
        public int PetsC1_Id { get; set; }

        [Required(ErrorMessage = "*Pet Type is required.")]
        [DisplayName("Pet:")]
        public string PetsC1_Name { get; set; }

        public int PetsC2_Id { get; set; }

        [Required(ErrorMessage = "*Pet Breed is required.")]
        [DisplayName("Pet Breed:")]
        public string PetsC2_Name { get; set; }

        public int PetsC3_Id { get; set; }

        [Required(ErrorMessage = "*Add title is required.")]
        [DisplayName("Add title:")]
        public string PetsC3_Name { get; set; }

        [Required(ErrorMessage = "*Description is required.")]
        [DisplayName("Description:")]
        public string Descrption { get; set; }

        [Required(ErrorMessage = "*Price is required.")]
        [DisplayName("Price:")]
        public int Cost { get; set; }

        public string PetsImage_Name { get; set; }

        public string PetsImage_Path { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "*Pet image is required.")]
        [DisplayName("Pet Image:")]
        public HttpPostedFileBase ImageFilePets { get; set; }

        public List<SelectListItem> DropDownPets { get; set; }

        public List<SelectListItem> DropDownCategory { get; set; }

        public string US_Name { get; set; }

        public string US_Phone { get; set; }
    }
}