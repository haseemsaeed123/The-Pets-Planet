using FypOnEF.Models.DB;
using FypOnEF.ViewModels.Order;
using FypOnEF.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypOnEF.Presenter.Order
{
    public class OrderPresenter
    {
        public void Order(List<CartViewModel> ses, OrderViewModel ordermodel, string name)
        {
            List<CartViewModel> model;
            model = ses;
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var date= DateTime.Now;
                        var userid = db.User_Info.Where(x => x.US_Name == name).FirstOrDefault<User_Info>();

                        #region *** Order Details Table Entry ***
                        OrderDetail od = new OrderDetail();
                        od.US_Id = userid.US_Id;
                        od.DeliveryAddress = ordermodel.DeliveryAddress;
                        od.Phone1 = ordermodel.Phone1;
                        od.Phone2 = ordermodel.Phone2;
                        od.TotalPrice = ordermodel.TotalPrice;
                        od.Totalitem = ordermodel.Totalitem;
                        od.US_Name = ordermodel.US_Name;
                        od.OrderStatus = "In process";
                        od.OrderDate = date.Date;
                        db.OrderDetails.Add(od);
                        db.SaveChanges();
                        #endregion *** Order Details Table Entry ***

                        #region *** Order Items Table Entry ***
                        List<OrderItem> oiList = new List<OrderItem>();
                        foreach (var items in model)
                        {
                            var productInventoryID = db.Product_Inventory.Where(x => x.PC3_Id == items.PC3_Id).FirstOrDefault<Product_Inventory>();
                            var VendorProductID = db.Vendor_Products.Where(x => x.PI_Id == productInventoryID.PI_Id).FirstOrDefault<Vendor_Product>();
                            var VendorProductQuantityID = db.Vendor_Product_Qties.Where(x => x.VP_Id == VendorProductID.VP_Id).FirstOrDefault<Vendor_Product_Qty>();
                            var VendorProductCostID = db.Vendor_Product_Cost.Where(x => x.VPQ_Id == VendorProductQuantityID.VPQ_Id).FirstOrDefault<Vendor_Product_Cost>();

                            OrderItem oi = new OrderItem
                            {
                                OD_Id = od.OD_Id,
                                VPC_Id = VendorProductCostID.VPC_Id,
                                UnitPrice = VendorProductCostID.Vendor_Price,
                                QuantityOrder = items.OD_ProductQty,
                                TotalItemPrice = (VendorProductCostID.Vendor_Price * items.OD_ProductQty)
                            };
                            oiList.Add(oi);
                        }
                        db.OrderItems.AddRange(oiList);
                        db.SaveChanges();
                        #endregion *** Order Items Table Entry ***

                        transaction.Commit();

                        model.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }

            }
        }

        public List<CartViewModel> GetOrderDetails(string name,int id)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetUserOrders(name).Select(x => new CartViewModel
                {
                    OD_Id = x.OD_Id,
                    DeliveryAddress=x.DeliveryAddress,
                    Phone1=x.Phone1,
                    Phone2=x.Phone2,
                    TotalPrice=Convert.ToInt16(x.TotalPrice),
                    Totalitem=Convert.ToInt16(x.Totalitem),
                    US_Name=x.US_Name,
                    OrderStatus=x.OrderStatus,
                    OrderDate = String.Format("{0:d/M/yyyy}", x.OrderDate),
                    PC3_Name=x.PC3_Name,
                    VP_Desc=x.VP_Desc,
                    Vendor_Price=Convert.ToInt16(x.UnitPrice),
                    QuantityOrder=Convert.ToInt16(x.QuantityOrder),
                    TotalItemPrice = Convert.ToInt32(x.TotalItemPrice),
                    ProImg_Path=x.ProImg_Path
                }).ToList();
                if(id != 0)
                {
                    data = data.Where(x => x.OD_Id == id).ToList();
                }
                return data;
            }
        }

        public List<CartViewModel> GetCustomerOrders(string name)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetCustomerDistinctOrders(name).Select(x => new CartViewModel
                {
                    OD_Id = x.OD_Id,
                    OrderDate = String.Format("{0:d/M/yyyy}", x.OrderDate),
                    OrderStatus = x.OrderStatus
                }).ToList();
                return data;
            }
        }

        public List<CartViewModel> GetVendorOrders(string name,int id)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.VendorOrders(name).Select(x => new CartViewModel
                {
                    OD_Id = x.OD_Id,
                    OrderStatus = x.OrderStatus,
                    OrderDate = String.Format("{0:d/M/yyyy}", x.OrderDate),
                }).ToList();
                if (id != 0)
                {
                    data = data.Where(x => x.OD_Id == id).ToList();
                }
                return data;
            }
        }


        public List<CartViewModel> GetVendorOrderDetails(string name, int id)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.VendorOrderDetails(name,id).Select(x => new CartViewModel
                {
                    OD_Id = x.OD_Id,
                    DeliveryAddress = x.DeliveryAddress,
                    Phone1 = x.Phone1,
                    Phone2 = x.Phone2,
                    TotalPrice = Convert.ToInt16(x.TotalPrice),
                    Totalitem = Convert.ToInt16(x.Totalitem),
                    US_Name = x.US_Name,
                    OrderStatus = x.OrderStatus,
                    OrderDate = String.Format("{0:d/M/yyyy}", x.OrderDate),
                    PC3_Name = x.PC3_Name,
                    VP_Desc = x.VP_Desc,
                    Vendor_Price = Convert.ToInt16(x.UnitPrice),
                    QuantityOrder = Convert.ToInt16(x.QuantityOrder),
                    TotalItemPrice = Convert.ToInt32(x.TotalItemPrice),
                    ProImg_Path = x.ProImg_Path
                }).ToList();
                return data;
            }
        }

    }
}