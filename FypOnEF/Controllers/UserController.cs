using FypOnEF.Models.DB;
using FypOnEF.Models.User;
using FypOnEF.Presenter.Admin;
using FypOnEF.Presenter.Blog;
using FypOnEF.ViewModels.Blog;
using FypOnEF.ViewModels.Product;
using FypOnEF.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace FypOnEF.Controllers
{
    public class UserController : Controller
    {
        List<RegVendorViewModel> venlist;
        List<BlogViewModel> bvm;
        BlogPresenter bp;
        UserPresenter up;
        AdminsPresenter ap;
        RegVendorViewModel rvm;
        public UserController() //Constructor
        {
            venlist = new List<RegVendorViewModel>();
            bvm = new List<BlogViewModel>();
            bp = new BlogPresenter();
            up = new UserPresenter();
            ap = new AdminsPresenter();
            rvm = new RegVendorViewModel();
        }

        #region **** User Roles ****

        // GET: User
        [Route("~/")]
        [Route("index")]
        [HttpGet]
        public ActionResult Index()
        {                        
            bvm = bp.BlogIndexPanel();
            return View(bvm);
        }

        #region *** User existence ***
        PetsPlanetDBContext db1 = new PetsPlanetDBContext();
        public JsonResult IsUser(string US_Name)
        {
            return Json(!db1.User_Info.Any(user => user.US_Name == US_Name), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsEmailInUse(string US_Email)
        {
            return Json(!db1.User_Info.Any(email => email.US_Email == US_Email), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsPhoneInUse(string US_Phone)
        {
            return Json(!db1.User_Info.Any(phone => phone.US_Phone == US_Phone), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsCNICInUse(string VI_CNIC)
        {
            return Json(!db1.Vendor_Info.Any(cnic => cnic.VI_CNIC == VI_CNIC), JsonRequestBehavior.AllowGet);
        }
        #endregion *** User existence ***

        #region ***User-Registration***

        [Route("user/register")]
        [HttpGet]
        public ActionResult RegisterBuyerSeller()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RegisterBuyer(UserModel u)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    String code = Guid.NewGuid().ToString();                   
                    up.SaveNewUser(u, code);
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Registration Failed " + ex;
            }
            return View(u);
        }

        #endregion ***User-Registration***

        #region *** User Login ***
        [Route("login/{id}")]
        [Route("login")]
        [HttpGet]
        public ActionResult LoginUser(string id)
        {            
            try
            {
                if(string.IsNullOrEmpty(Session["US_Name"] as string))
                {
                    string val = Request.Url.LocalPath.ToString();
                    string _val = "/login/" + id;
                    if (val == _val)
                        up.LoginUserGet(id);
                    else
                        TempData["preurl"] = (Request.UrlReferrer.AbsolutePath);
                }
                else
                    TempData["Login"] = "You're already logged in.";           
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(User_Info ui)
        {
            try
            {
                int? count = UserPresenter.LoginUserPost(ui);
                if (count == 2)
                {
                    FormsAuthentication.SetAuthCookie(ui.US_Name, true);
                    Session["US_Name"] = ui.US_Name.ToString();
                    int? ret = UserPresenter.CheckUserMemberShip(ui);

                    if (ret == 20)
                    {
                        Session["Role"] = "Admin";
                        return RedirectToAction("ViewAllProducts", "Product");
                    }                        
                    else if (ret == 12)
                    {
                        Session["Role"] = "Vendor";
                        return RedirectToAction("ViewAllProducts", "Product");
                    }                        
                    else
                    { 
                        Session["Role"] = "Buyer/Seller";
                        return Redirect(TempData["preurl"].ToString());
                    }
                }
                else if (count == 1)
                    TempData["Error"] = "*Verify your email first";
                else
                    TempData["Error"] = "*UserName or Password is incorrect";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Login Failed -" + ex.Message;
            }
            return RedirectToAction("LoginUser", "User");
        }
        #endregion *** User Login ***

        #region *** Logout ***
        [Route("logout")]
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }
        #endregion *** Logout ***

        #endregion **** User Roles ****

        #region **** Vendor Roles ****

        #region ***Vendor Register***
        [Route("user/venreg")]
        [HttpGet]
        public ActionResult RegVendor()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RegVen(RegVendorViewModel rv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int bytecount = rv.ImageFile.ContentLength;
                    if (bytecount <= 5000000)
                    {
                        String id = Guid.NewGuid().ToString();
                        string fileName = Path.GetFileNameWithoutExtension(rv.ImageFile.FileName);
                        string extension = Path.GetExtension(rv.ImageFile.FileName);
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG")
                        {
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            rv.VendorImage_Name = fileName;
                            rv.VendorImage_ImagePath = "~/Content/Pictures/Vendor_CNIC_Images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Content/Pictures/Vendor_CNIC_Images"), fileName);
                            rv.ImageFile.SaveAs(fileName);
                            up.VendorRegPost(rv, id);
                            AdminsPresenter.SendEmail(rv.US_Name, rv.US_Email, id);
                            return RedirectToAction("LoginUser", "User");
                        }
                        else
                            TempData["extension"] = "Only .jpg , .jpeg & .png file supported.";
                    }
                    else
                        TempData["extension"] = "File size must be less than 5MB.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Register Failed -" + ex.Message;
                
            }
            return RedirectToAction("RegVendor", "User");
        }
        #endregion ***Vendor Register***

        #endregion **** Vendor Roles ****

        #region **** Admin Roles ****

        #region *** Create New Admin ***
        [Route("createadmin")]
        [HttpGet]
        public ActionResult CreateNewAdmin()
        {
            if (string.IsNullOrEmpty(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (count == 1)
                    return View();
                else
                    return RedirectToAction("Index", "User");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(UserModel u)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    String code = Guid.NewGuid().ToString();                    
                    ap.SaveNewAdmin(u, code);
                    return RedirectToAction("Welcome", "User");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Register Failed -" + ex.Message;
            }
            return View();
        }
        #endregion *** Create New Admin ***

        #region *** Get New Vendor ***
        [Route("newvendor")]
        [HttpGet]
        public ActionResult GetNewVendor()
        {
            if (string.IsNullOrEmpty(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (count == 1)
                {
                    venlist = up.GetNewVendor();
                    try
                    {
                        return View(venlist);
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
        #endregion *** Get New Vendor ***

        #region *** Get Approve Vendor ***
        [Route("approvedvendor")]
        [HttpGet]
        public ActionResult GetApproveVendor()
        {
            if (string.IsNullOrEmpty(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (count == 1)
                {
                    venlist = up.GetApproveVendor();
                    try
                    {
                        return View(venlist);
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
        #endregion *** Get Approve Vendor ***

        #region *** Get Rejected Vendor ***
        [Route("rejectedvendor")]
        [HttpGet]
        public ActionResult GetRejectedVendor()
        {
            if (string.IsNullOrEmpty(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (count == 1)
                {
                    venlist = up.GetNotApproveVendor();
                    try
                    {
                        return View(venlist);
                    }
                    catch (Exception exx)
                    {
                        ViewBag.errormsg = exx;
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
        }
        #endregion *** Get Rejected Vendor ***

        #region *** Vendor Details ***
        [Route("vendordetails")]
        [HttpGet]
        public ActionResult VendorDetails(int id)
        {
            if (string.IsNullOrEmpty(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (count == 1)
                {
                    rvm = up.GetVendorDetails(id);
                    return View(rvm);
                }
                else
                    return RedirectToAction("Index", "User");
            }
        }
        #endregion *** Vendor Details ***

        #region *** Vendor Approval ***
        [Route("vendor/approve/{id}")]
        public JsonResult ApproveVendor(int id)
        {
            var ret = "";
            try
            {
                up.ApproveVendor(id);
                ret = "Approved";                
            }
            catch (Exception ex)
            {
                ret = ViewBag.error = "Vendor Approval Failed !!!" + ex;
                //return Json(ViewBag.error, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion *** Vendor Approval ***

        #region *** Vendor Rejection ***
        [Route("vendor/reject/{id}")]
        public JsonResult RejectVendor(int id)
        {
            var ret = "";
            try
            {
                up.RejectVendor(id);
                ret = "Rejected";
            }
            catch (Exception ex)
            {
                ret = ViewBag.error = "Vendor Rejection Failed !!!" + ex;
                //return Json(ViewBag.error, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion *** Vendor Rejection ***

        #endregion **** Admin Roles ****

    }
}