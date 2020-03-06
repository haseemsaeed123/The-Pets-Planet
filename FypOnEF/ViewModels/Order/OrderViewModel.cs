using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FypOnEF.ViewModels.Order
{
    public class OrderViewModel
    {
        [Key]
        public int OD_Id { get; set; }
        public string US_Name { get; set; }

        public string DeliveryAddress { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public Int32 TotalPrice { get; set; }

        public Int16 Totalitem { get; set; }

        public int PC3_Id { get; set; }

        public int Qty { get; set; }

        public string OrderStatus { get; set; }

        public String OrderDate { get; set; }
    }
}