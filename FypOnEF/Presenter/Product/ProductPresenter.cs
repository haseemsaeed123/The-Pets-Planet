using FypOnEF.Models.DB;
using FypOnEF.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static FypOnEF.ViewModels.Product.ProductCategoryViewModel;

namespace FypOnEF.Presenter.Product
{
    public class ProductPresenter
    {
        public List<SelectListItem> GetPets()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                return (db.Product_C1.Select(x => new SelectListItem { Text = x.PC1_Name, Value = x.PC1_Id.ToString() }).ToList());
            }
        }

        public List<SelectListItem> GetProductCategory()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                return (db.Product_C2.Select(x => new SelectListItem { Text = x.PC2_Name, Value = x.PC2_Id.ToString() }).ToList());
            }
        }

        public static void VendorProductSubmission(ProductCategoryViewModel pvm, string vname)
        {
            using (var db = new PetsPlanetDBContext())
            {
                try
                {
                    db.VendorProductEntry(vname, pvm.PC1_Name, pvm.PC2_Name, pvm.PC3_Name, pvm.ProImg_Path, pvm.ProImg_Name, pvm.VP_Desc, pvm.VP_Specs, pvm.VPQ_Qty, pvm.VPQ_Status, pvm.Vendor_Price, pvm.Company_Price);
                }
                catch
                {

                }
            }
        }

        public List<ProductCategoryViewModel> GetAllProducts()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetNewProducts().Select(x => new ProductCategoryViewModel
                {
                    PC3_Id = x.PC3_Id,
                    PC1_Name = x.PC1_Name,
                    PC2_Name = x.PC2_Name,
                    PC3_Name = x.PC3_Name,
                    VPQ_Qty = Convert.ToInt16(x.VPQ_Qty),
                    Vendor_Price = x.Vendor_Price,
                    Company_Price = x.Company_Price

                }).ToList();
                return data;
            }
        }

        public ProductCategoryViewModel ProDetailsGet(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                var details = db.ProductDetailsGet(id);
                ProductPresenter pp = new ProductPresenter();
                ProductCategoryViewModel pvm = new ProductCategoryViewModel();
                pvm.DropDownPets = pp.GetPets();
                pvm.DropDownCategory = pp.GetProductCategory();
                foreach (var item in details)
                {
                    pvm.PC1_Name = item.PC1_Name;
                    pvm.PC2_Name = item.PC2_Name;
                    pvm.PC3_Name = item.PC3_Name;
                    pvm.VP_Desc = item.VP_Desc;
                    pvm.VP_Specs = item.VP_Specs;
                    pvm.VPQ_Qty = Convert.ToInt16(item.VPQ_Qty);
                    pvm.Vendor_Price = item.Vendor_Price;
                    pvm.Company_Price = item.Company_Price;
                    pvm.ProImg_Path = item.ProImg_Path;
                    pvm.ProImg_Name = item.ProImg_Name;
                    pvm.PC1_Id = item.PC1_Id;
                    pvm.PC2_Id = item.PC2_Id;
                    pvm.ProImg_Id = item.ProImg_Id;
                    pvm.PC3_Id = id;
                }
                return pvm;
            }
        }

        public User_Info GetUserDetails(int id)
        {
            User_Info u = new User_Info();

            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                u = db.User_Info.Where(x => x.US_Id == id).FirstOrDefault();
            }

            return u;
        }

        public void DeleteVendorProductGet(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.DeleteVendorProduct(id);
            }
        }

        public void EditVendorProductPost(ProductCategoryViewModel pcvm)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.UpdateProduct(pcvm.PC3_Id, pcvm.PC1_Id, pcvm.PC2_Id, pcvm.PC3_Name, pcvm.VP_Desc, pcvm.VP_Specs, pcvm.VPQ_Qty, pcvm.Vendor_Price, pcvm.Company_Price, pcvm.ProImg_Name, pcvm.ProImg_Path, pcvm.ProImg_Id);
            }
        }

        public List<ProductCategoryViewModel> GetApproveVendorProductsGet(string search)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetApproveVendorProducts().Select(x => new ProductCategoryViewModel
                {
                    PC3_Id = x.PC3_Id,
                    PC1_Name = x.PC1_Name,
                    PC2_Name = x.PC2_Name,
                    PC3_Name = x.PC3_Name,
                    VPQ_Qty = Convert.ToInt16(x.VPQ_Qty),
                    Vendor_Price = x.Vendor_Price,
                    Company_Price = x.Company_Price,
                    ProImg_Path = x.ProImg_Path
                }).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(x => x.PC3_Name.ToLower().Contains(search.Trim().ToLower())).ToList();
                }
                return data;
            }
        }

        public List<ProductCategoryViewModel> GetNotApproveVendorProductsGet()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetNotApproveVendorProducts().Select(x => new ProductCategoryViewModel
                {
                    PC3_Id = x.PC3_Id,
                    PC1_Name = x.PC1_Name,
                    PC2_Name = x.PC2_Name,
                    PC3_Name = x.PC3_Name,
                    VPQ_Qty = Convert.ToInt16(x.VPQ_Qty),
                    Vendor_Price = x.Vendor_Price,
                    Company_Price = x.Company_Price
                }).ToList();
                return data;
            }
        }

        public void AddNewProductCategory1(ProductCategoryViewModel pcvm)
        {
            using (var db = new PetsPlanetDBContext())
            {
                try { db.AddnewProductCat_1(pcvm.PC1_Name); }
                catch { }
            }
        }

        public void AddNewProductCategory2(ProductCategoryViewModel pcvm)
        {
            using (var db = new PetsPlanetDBContext())
            {
                try { db.AddnewProductCat_2(pcvm.PC2_Name); }
                catch { }
            }
        }

        public void ApproveProduct(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.ApproveProduct(id);
            }
        }

        public void RejectProduct(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.NotApproveProduct(id);
            }
        }

        public CartViewModel GetCartProduct(ProductCategoryViewModel pc)
        {
            using (var db = new PetsPlanetDBContext())
            {
                CartViewModel cvm = new CartViewModel();
                var data = db.GetProductCart(pc.PC3_Id);
                foreach (var x in data)
                {
                    cvm.PC3_Name = x.PC3_Name;
                    cvm.VPQ_Qty = Convert.ToInt16(x.VPQ_Qty);
                    cvm.Vendor_Price = Convert.ToInt16(x.Vendor_Price);
                    cvm.ProImg_Path = x.ProImg_Path;
                }
                return cvm;
            }
        }

    }
}