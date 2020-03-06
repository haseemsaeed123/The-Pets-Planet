using FypOnEF.Models.DB;
using FypOnEF.Presenter.Order;
using FypOnEF.Presenter.Product;
using FypOnEF.Presenter.ShoppingCart;
using FypOnEF.ViewModels.Order;
using FypOnEF.ViewModels.Product;
using FypOnEF.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypOnEF.Controllers
{
    public class ProductController : Controller
    {
        
        ProductPresenter pp;
        ProductCategoryViewModel model;
        List<ProductCategoryViewModel> prolist;
        List<OrderViewModel> orderlist;
        ShoppingCartPresenter scp;
        OrderPresenter op;
        List<CartViewModel> listcartmodel;
        public ProductController() // Constructor
        {
            pp = new ProductPresenter();
            model = new ProductCategoryViewModel();
            prolist = new List<ProductCategoryViewModel>();
            scp = new ShoppingCartPresenter();
            op = new OrderPresenter();
            orderlist = new List<OrderViewModel>();
            listcartmodel = new List<CartViewModel>();
        }
        public ActionResult Index()
        {
            return View();
        }

        #region **** Vendor Roles ****

        #region *** Product Uploading ***
        [Route("product/newproduct")]
        [HttpGet]
        public ActionResult newProduct()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? vendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                    if (vendorCount == 1)
                    {                        
                        model.DropDownPets = pp.GetPets();
                        model.DropDownCategory = pp.GetProductCategory();
                        return View(model);
                    }
                    else
                    {
                        Session.Abandon();
                        return RedirectToAction("LoginUser", "User");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ProRegMsg"] = "Product Loading Failed - " + ex.Message;
            }
            return View();
        }

        [Route("product/addnewpro")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddNewProduct(ProductCategoryViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string vname = Session["US_Name"] as string;
                    model.DropDownPets = pp.GetPets();
                    model.DropDownCategory = pp.GetProductCategory();

                    int bytecount = data.ImageFileProduct.ContentLength;
                    if (bytecount <= 5000000)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageFileProduct.FileName);
                        string extension = Path.GetExtension(data.ImageFileProduct.FileName);
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG")
                        {
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            data.ProImg_Name = fileName;
                            data.ProImg_Path = "~/Content/Pictures/VendorProductImages/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Content/Pictures/VendorProductImages/"), fileName);
                            data.ImageFileProduct.SaveAs(fileName);

                            ProductPresenter.VendorProductSubmission(data, vname);
                            TempData["ProRegMsg"] = "Product Entered Successfully.";
                            ModelState.Clear();
                            return RedirectToAction("newProduct", "Product");
                        }
                        else
                        {
                            TempData["extension"] = "Only .jpg , .jpeg & .png file supported.";
                            return RedirectToAction("newProduct", "Product");
                        }
                    }
                    else
                    {
                        TempData["extension"] = "File size must be less than 5MB.";
                        return RedirectToAction("newProduct", "Product");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ProRegMsg"] = "Product Insertion Failed - " + ex.Message;
            }
            return RedirectToAction("newProduct", "Product");
        }

        #endregion  *** Product Uploading ***

        #region *** Delete Product ***
        [Route("product/delete/{id}")]
        public JsonResult DeleteProduct(int id)
        {
            var ret = "";
            try
            {
                pp.DeleteVendorProductGet(id);
                ret = "Deleted";
            }
            catch (Exception ex)
            {
                ret = ViewBag.mm = "Product deletion failed !!!" + ex;
                //return Json(ViewBag.mm, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion *** Delete Product ***

        #region *** Edit Product ***
        [Route("product/edit/{id}")]
        [HttpGet]
        public ActionResult EditProductGet(int id)
        {
            if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                try
                {
                    int? vendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                    if (vendorCount == 1)
                    {
                        model = pp.ProDetailsGet(id);
                        return View(model);
                    }
                    else
                        return RedirectToAction("Index", "User");
                }
                catch (Exception ex)
                {
                    ViewBag.mm = ex;
                    return View();
                }
            }
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult EditProduct(ProductCategoryViewModel pcvm, string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (pcvm.ImageFileProduct == null)
                    {
                        try
                        {
                            pp.EditVendorProductPost(pcvm);
                            return RedirectToAction("ViewAllProducts", "Product");
                        }
                        catch (Exception exd)
                        {
                            TempData["ProRegMsg"] = "Product Updation Failed - " + exd.Message;
                            return RedirectToAction("EditProductGet", "Product");
                        }
                    }
                    else
                    {
                        int bytecount = pcvm.ImageFileProduct.ContentLength;
                        if (bytecount > 0 && bytecount <= 5000000)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(pcvm.ImageFileProduct.FileName);
                            string extension = Path.GetExtension(pcvm.ImageFileProduct.FileName);
                            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG" && ModelState.IsValid)
                            {
                                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                                pcvm.ProImg_Name = fileName;
                                pcvm.ProImg_Path = "~/Content/Pictures/VendorProductImages/" + fileName;
                                fileName = Path.Combine(Server.MapPath("~/Content/Pictures/VendorProductImages/"), fileName);
                                pcvm.ImageFileProduct.SaveAs(fileName);

                                //ProductPresenter.EditVendorProductPost(pcvm);
                                TempData["ProRegMsg"] = "Product Entered Successfully.";
                                ModelState.Clear();
                                return RedirectToAction("ViewAllProducts", "Product");
                            }
                            else
                            {
                                TempData["ModelError"] = "Only .jpg , .jpeg & .png file supported.";
                                return RedirectToAction("EditProductGet", "Product");
                            }
                        }
                        else
                        {
                            TempData["ModelError"] = "File size must be less than 5MB.";
                            return RedirectToAction("EditProductGet", "Product");
                        }
                    }
                }
                return RedirectToAction("EditProductGet", "Product");
            }
            catch (Exception ex)
            {
                TempData["ProRegMsg"] = "Product Updation Failed - " + ex.Message;
                return RedirectToAction("EditProductGet", "Product");
            }
        }
        #endregion *** Edit Product ***

        #region *** Vendor Order ***
        [Route("vendororders")]
        [HttpGet]
        public ActionResult VendorOrders()
        {
            if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? VendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                if (VendorCount == 1)
                {
                    int id = 0;
                    listcartmodel = op.GetVendorOrders(Session["US_Name"] as string,id);
                    return View(listcartmodel);
                }
                else
                    return RedirectToAction("Index", "User");
            }
        }

        #endregion *** Vendor Order ***

        #region ***Vendor Order Details ***
        [Route("vendororderdetails/{id}")]
        public ActionResult VendorOrderDetails(int id)
        {
            if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                
                int? VendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                if (VendorCount == 1)
                {
                    listcartmodel = op.GetVendorOrderDetails(Session["US_Name"] as string,id);
                    return View(listcartmodel);
                }
                else
                    return RedirectToAction("Index", "User");
            }
        }
        #endregion *** Order Details ***

        #endregion **** Vendor Roles ****


        #region **** Admin Roles ****


        #region *** Add New Product Category ***
        [Route("product/addnew")]
        [HttpGet]
        public ActionResult AddNewProductCategory()
        {
            if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? adminCount = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (adminCount == 1)
                    return View();
                else
                    return RedirectToAction("Index", "User");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddNewProCat([Bind(Include = "PC1_Name,PC2_Name")] ProductCategoryViewModel pcvm, string answer)
        {
            if (!String.IsNullOrWhiteSpace(answer))
            {
                switch (answer)
                {
                    case "Submit Product Category 1":
                        if (ModelState.IsValidField("PC1_Name"))
                        {
                            pp.AddNewProductCategory1(pcvm);
                            ModelState.Clear();
                        }
                        break;
                    case "Submit Product Category 2":
                        if (ModelState.IsValidField("PC2_Name"))
                        {
                            pp.AddNewProductCategory2(pcvm);
                            ModelState.Clear();
                        }
                        break;
                }
            }
            return RedirectToAction("AddNewProductCategory", "Product");
        }
        #endregion *** Add New Product Category ***

        #region **** Products Approval ****
        [Route("product/approve/{id}")]
        public JsonResult ApproveProduct(int id)
        {
            var ret = "";
            try
            {
                pp.ApproveProduct(id);
                ret = "Approved";               
            }
            catch (Exception ex)
            {
                ret = ViewBag.error = "Product Approval Failed !!!" + ex;
                //return Json(ViewBag.error, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion **** Products Approval ****

        #region **** Products Rejection ****
        [Route("product/reject/{id}")]
        public JsonResult NotApproveProduct(int id)
        {
            var ret = "";
            try
            {
                pp.RejectProduct(id);
                ret = "Rejected";
                //return Json(ret, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ret = ViewBag.error = "Product Rejection Failed !!!" + ex;
                //return Json(ViewBag.error, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion **** Products Rejection ****


        #endregion **** Admin Roles ****


        #region **** Admin/Vendor Roles ****

        #region *** View Products ***
        [Route("product/new")]
        [HttpGet]
        public ActionResult ViewAllProducts()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? vendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                    int? adminCount = UserPresenter.IsAdmin(Session["US_Name"] as string);
                    if (vendorCount == 1 || adminCount == 1)
                    {
                        prolist = pp.GetAllProducts();
                        try
                        {
                            if (adminCount == 1)
                                ViewBag.adminCheck = 1;
                            return View(prolist);
                        }
                        catch (Exception exx)
                        {
                            ViewBag.errormsg = exx;
                            return View();
                        }
                    }
                    else
                        return RedirectToAction("Login", "User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.errormsg = "Failed" + ex.Message;
            }
            return View();
        }
        #endregion *** View Products ***

        #region *** Product Details ***
        [Route("product/details/{id}")]
        [HttpGet]
        public ActionResult ProductDetails(int id)
        {
            if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? vendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                int? adminCount = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (vendorCount == 1 || adminCount == 1)
                {
                    model = pp.ProDetailsGet(id);
                    return View(model);
                }
                else
                    return RedirectToAction("Index", "User");
            }
        }
        #endregion *** Product Details ***

        #region *** Admin Approved Products ***
        [Route("product/approved")]
        [HttpGet]
        public ActionResult GetAdminApprovedProduct()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? vendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                    int? adminCount = UserPresenter.IsAdmin(Session["US_Name"] as string);
                    if (vendorCount == 1 || adminCount == 1)
                    {
                        prolist = pp.GetApproveVendorProductsGet("");
                        try
                        {
                            if (adminCount == 1)
                                ViewBag.adminCheck = 1;
                            return View(prolist);
                        }
                        catch (Exception exx)
                        {
                            ViewBag.errormsg = exx;
                            return View();
                        }
                    }
                    else
                        return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex;
                return View();
            }
        }
        #endregion *** Admin Approved Products ***

        #region *** Admin Rejected Products ***
        [Route("product/rejected")]
        [HttpGet]
        public ActionResult GetAdminNotApprovedProduct()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? vendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                    int? adminCount = UserPresenter.IsAdmin(Session["US_Name"] as string);
                    if (vendorCount == 1 || adminCount == 1)
                    {
                        prolist = pp.GetNotApproveVendorProductsGet();
                        try
                        {
                            if (adminCount == 1)
                                ViewBag.adminCheck = 1;
                            return View(prolist);
                        }
                        catch (Exception exx)
                        {
                            ViewBag.errormsg = exx;
                            return View();
                        }
                    }
                    else
                        return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex;
                return View();
            }
        }
        #endregion *** Admin Rejected Products ***

        #endregion **** Admin/Vendor Roles ****


        #region **** User Panel ****

        #region *** Product panel View ***
        [Route("product/search/{search}")]
        [Route("product")]
        [HttpGet]
        public ActionResult ProductView(string search)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search))
                    ViewBag.SearchKeyword = "Your Search '" + search + "' Result";
                prolist = pp.GetApproveVendorProductsGet(search);
                return View(prolist);
            }
            catch (Exception ex)
            {
                ViewBag.error = "Page Loading Failed." + ex;
                return View();
            }
        }
        #endregion *** Product panel View ***

        #region *** Product Details ***
        [Route("product/{id}")]
        [HttpGet]
        public ActionResult picdetails(int id)
        {
            model = pp.ProDetailsGet(id);
            return View(model);
        }
        #endregion *** Product Details ***

        #endregion **** User Panel ****


        #region **** Cart Actions ****

        #region *** Add To Cart ***
        [Route("product/addcart")]
        public JsonResult AddCart(Basket b)
        {
            if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                return Json(new
                {
                    redirectUrl = Url.Action("LoginUser", "User"),
                    isRedirect = true
                });
            else
            {
                var ses = Session["Basket"];
                Session["Basket"] = scp.CartActions(b, (List<CartViewModel>)ses);
                return Json(Session["Basket"], JsonRequestBehavior.AllowGet);
            }
        }
        #endregion  *** Add To Cart ***

        #region *** CheckOut ***
        [Route("checkout")]
        public ActionResult CheckOut()
        {
            var currentModel = (List<CartViewModel>)Session["Basket"];
            return View(currentModel);
        }
        #endregion *** CheckOut ***

        #region *** Basket ***
        [Route("basket")]
        public JsonResult Basket()
        {
            return Json(Session["Basket"], JsonRequestBehavior.AllowGet);
        }
        #endregion *** Basket ***

        #endregion **** Cart Actions ****


        #region **** Order ****

        #region *** Cart To Order ***
        [Route("neworder")]
        public JsonResult Order(OrderViewModel model)
        {
            var ses = Session["Basket"];
            if (ses == null)
            {
                return Json(new
                {
                    alert = true
                });
            }
            else
            {
                op.Order((List<CartViewModel>)ses, model, Session["US_Name"] as string);
                return Json("Your Order Has been placed.", JsonRequestBehavior.AllowGet);
            }

        }
        #endregion *** Cart To Order ***

        #region *** Multiple Orders ***
        [Route("orders")]
        public ActionResult CustomerOrders()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? userCount = UserPresenter.IsUser(Session["US_Name"] as string);
                    if (userCount == 1)
                    {
                        listcartmodel = op.GetCustomerOrders(Session["US_Name"] as string);
                        return View(listcartmodel);
                    }
                    else
                        return RedirectToAction("Index", "User");
                }
            }
            catch
            {

            }
            return View();
        }
        #endregion *** Multiple Orders ***

        #region *** Order Details ***
        [Route("orderdetails/{id}")]
        public ActionResult OrderDetails(int id)
        {
            if (string.IsNullOrWhiteSpace(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? userCount = UserPresenter.IsUser(Session["US_Name"] as string);
                if (userCount == 1)
                {
                    listcartmodel = op.GetOrderDetails(Session["US_Name"] as string,id);
                    return View(listcartmodel);
                }
                else
                    return RedirectToAction("Index", "User");
            }
        }
        #endregion *** Order Details ***

        #endregion **** Order ****

    }
}