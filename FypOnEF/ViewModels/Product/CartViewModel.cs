using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FypOnEF.ViewModels.Product
{
    public class CartViewModel
    {
        public int PC1_Id { get; set; }

        [DisplayName("Product Of:")]
        public string PC1_Name { get; set; }

        public int PC2_Id { get; set; }

        [DisplayName("Product Category:")]
        public string PC2_Name { get; set; }

        public int PC3_Id { get; set; }

        [Required(ErrorMessage = "Enter Product.")]
        [DisplayName("Product:")]
        public string PC3_Name { get; set; }

        public int ProImg_Id { get; set; }

        public string ProImg_Name { get; set; }

        public string ProImg_Path { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFileProduct { get; set; }

        public int VP_Id { get; set; }

        public int VP_ApproveProduct { get; set; }

        [DisplayName("Product Description:")]
        public string VP_Desc { get; set; }

        [DisplayName("Product Specification:")]
        public string VP_Specs { get; set; }

        public int VP_NotApproveProduct { get; set; }

        public int VPQ_Id { get; set; }

        [DisplayName("Product Quantity:")]
        public int VPQ_Qty { get; set; }

        public int VPQ_Status { get; set; }

        public int VPQ_RemainingQty { get; set; }

        public int VPC_Id { get; set; }

        [DisplayName("Product Price:")]
        public int Vendor_Price { get; set; }

        [DisplayName("Company Price:")]
        public int Company_Price { get; set; }

        public int OI_Id { get; set; }

        public string OI_Address { get; set; }

        public string OI_Phone1 { get; set; }

        public string OI_Phone2 { get; set; }

        public int OI_Total_Amount { get; set; }

        public int OI_NoOfItem { get; set; }

        public string OM_Id { get; set; }

        public DateTime OM_OrderDate { get; set; }

        public int US_Id { get; set; }

        public string US_Name { get; set; }

        public string US_Phone { get; set; }

        public string US_Address { get; set; }

        public int VI_Id { get; set; }

        public string OM_OrderStatus { get; set; }

        public int OD_Id { get; set; }

        public int OD_ProductQty { get; set; }


        public string DeliveryAddress { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public Int32 TotalPrice { get; set; }

        public Int16 Totalitem { get; set; }

        public int Qty { get; set; }

        public string OrderStatus { get; set; }

        public String OrderDate { get; set; }

        public int QuantityOrder { get; set; }

        public Int32 TotalItemPrice { get; set; }
    }
}