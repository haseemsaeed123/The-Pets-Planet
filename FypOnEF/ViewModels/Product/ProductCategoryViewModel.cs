using FypOnEF.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypOnEF.ViewModels.Product
{
    public class ProductCategoryViewModel
    {
        public int PC1_Id { get; set; }

        [Display(Name="Product Of:")]
        public string PC1_Name { get; set; }

        public List<SelectListItem> DropDownPets { get; set; }

        public int PC2_Id { get; set; }

        [Display(Name = "Product Category:")]
        public string PC2_Name { get; set; }

        public List<SelectListItem> DropDownCategory { get; set; }

        public int PC3_Id { get; set; }

        [Required(ErrorMessage = "Enter Product.")]
        [Display(Name = "Product:")]
        public string PC3_Name { get; set; }

        public List<GetNewProducts_Result> ProductDetails { get; set; }

        public int ProImg_Id { get; set; }

        public string ProImg_Name { get; set; }

        public string ProImg_Path { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFileProduct { get; set; }

        public int VP_Id { get; set; }

        public int VP_ApproveProduct { get; set; }

        [Display(Name = "Product Description:")]
        public string VP_Desc { get; set; }

        [Display(Name = "Product Specification:")]
        public string VP_Specs { get; set; }

        public int VP_NotApproveProduct { get; set; }

        public int VPQ_Id { get; set; }

        [Display(Name = "Product Quantity:")]
        public int VPQ_Qty { get; set; }

        public int VPQ_Status { get; set; }

        public int VPQ_RemainingQty { get; set; }

        public int VPC_Id { get; set; }

        [Display(Name = "Product Price:")]
        public int Vendor_Price { get; set; }

        [Display(Name = "Company Price:")]
        public int Company_Price { get; set; }

        public int OI_Id { get; set; }

        public string OI_Address { get; set; }

        public string OI_Phone1 { get; set; }

        public string OI_Phone2 { get; set; }

        public int OI_Total_Amount { get; set; }

        public int OI_NoOfItem { get; set; }

        public int OM_Id { get; set; }

        public DateTime OM_OrderDate { get; set; }

        public int US_Id { get; set; }

        public int VI_Id { get; set; }

        public string OM_OrderStatus { get; set; }

        public int OD_Id { get; set; }

        public int OD_ProductQty { get; set; }

        public List<CartViewModel> BasketItems { get; set; }
    
    }
}