using FypOnEF.Models.DB;
using FypOnEF.Presenter.Product;
using FypOnEF.ViewModels.Order;
using FypOnEF.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypOnEF.Presenter.ShoppingCart
{
    public class ShoppingCartPresenter
    {
        public List<CartViewModel> CartActions(Basket b,List<CartViewModel> ses)
        {
            List<CartViewModel> model;
            ProductCategoryViewModel pc = new ProductCategoryViewModel();
            CartViewModel cvm = new CartViewModel();
            ProductPresenter pm = new ProductPresenter();
            pc.PC3_Id = Convert.ToInt16(b.PC3_Id);
            cvm = pm.GetCartProduct(pc);
            if (ses == null)
                model = new List<CartViewModel>();
            else
                model = ses;
            if (b.Action == "Add")
            {
                if (!model.Any(x => x.PC3_Id == Convert.ToInt16(b.PC3_Id)))
                {
                    model.Add(new CartViewModel()
                    {
                        PC3_Id = Convert.ToInt16(b.PC3_Id),
                        PC3_Name = cvm.PC3_Name,
                        OD_ProductQty = b.OD_ProductQty,
                        ProImg_Path = cvm.ProImg_Path,
                        Vendor_Price = cvm.Vendor_Price,
                    });
                }
                else
                    model.FirstOrDefault(x => x.PC3_Id == b.PC3_Id).OD_ProductQty += b.OD_ProductQty;
            }
            else if(b.Action == "Qty")
                model.FirstOrDefault(x => x.PC3_Id == b.PC3_Id).OD_ProductQty = b.OD_ProductQty;

            else if(b.Action == "Remove")
            {
                var pro = model.Find(x => x.PC3_Id == Convert.ToInt16(b.PC3_Id));
                model.Remove(pro);
            }
            return model;
        }

        
    }
}