using FypOnEF.Presenter.Pet;
using FypOnEF.Presenter.Product;
using FypOnEF.ViewModels.Pet;
using FypOnEF.ViewModels.Product;
using FypOnEF.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypOnEF.Controllers
{
    public class PetController : Controller
    {
        PetViewModel pvm;
        PetPresenter pp;
        public PetController() //Constructor
        {
            pvm = new PetViewModel();
            pp = new PetPresenter();
        }
        // GET: Pet
        public ActionResult Index()
        {
            return View();
        }

        #region **** User Roles ****

        #region *** Pets Upload ***
        [Route("Pet/upload")]
        [HttpGet]
        public ActionResult PetUpload()
        {
            try
            {
                if (string.IsNullOrEmpty(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? count = UserPresenter.IsUser(Session["US_Name"] as string);
                    if (count == 1)
                    {
                        pvm.DropDownPets = pp.GetPets();
                        pvm.DropDownCategory = pp.GetBreed();
                        return View(pvm);
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
                TempData["ProRegMsg"] = "Pets Loading Failed - " + ex.Message;
            }
            return View();
        }

        [Route("pet/addnewpet")]
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult PetsUploading(PetViewModel pvm, FormCollection fm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string username = Session["US_Name"] as string;
                    string pname = pvm.PetsC1_Name = fm["PetsC1_Name"];
                    string _pname = pvm.PetsC2_Name = fm["PetsC2_Name"];
                    int bytecount = pvm.ImageFilePets.ContentLength;
                    if (bytecount <= 5000000)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(pvm.ImageFilePets.FileName);
                        string extension = Path.GetExtension(pvm.ImageFilePets.FileName);
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG")
                        {
                            pvm.PetsC1_Id = Convert.ToInt16(pname);
                            pvm.PetsC2_Id = Convert.ToInt16(_pname);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            pvm.PetsImage_Name = fileName;
                            pvm.PetsImage_Path = "~/Content/Pictures/PetPicture/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Content/Pictures/PetPicture/"), fileName);
                            pvm.ImageFilePets.SaveAs(fileName);
                            pp.PetUpload(pvm, username);
                            TempData["ProRegMsg"] = "Pets Entered Successfully.";
                            ModelState.Clear();

                            return RedirectToAction("PetUpload", "Pet");
                        }
                        else
                        {
                            TempData["extension"] = "Only .jpg , .jpeg & .png file supported.";
                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ProRegMsg"] = "Pets Uploading Failed - " + ex.Message;                
            }
            return RedirectToAction("PetUpload", "Pet");
        }
        #endregion *** Pets Upload ***

        #region *** Pet panel View ***
        [Route("pet")]
        [HttpGet]
        public ActionResult PetsView()
        {
            return View(pp.PetApproved());
        }
        #endregion *** Pet panel View ***

        #region *** Pet Details ***
        [Route("pet/{id}")]
        [HttpGet]
        public ActionResult picdetails(int id)
        {
            pvm = pp.PetDetails(id);
            return View(pvm);
        }
        #endregion *** Pet Details ***

        #endregion **** User Roles ****
    }
}