using FypOnEF.Presenter.Blog;
using FypOnEF.ViewModels.Blog;
using FypOnEF.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypOnEF.Controllers
{
    public class BlogController : Controller
    {
        List<BlogViewModel> bvm;
        BlogViewModel _bvm;
        BlogPresenter bp;
        public BlogController() //Constructor
        {
            bvm = new List<BlogViewModel>();
            bp = new BlogPresenter();
            _bvm = new BlogViewModel();
        }

        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        #region **** User Roles ****

        #region *** BLog Upload ***
        [Route("Blog/upload")]
        [HttpGet]
        public ActionResult BlogUpload()
        {
            try
            {
                if (string.IsNullOrEmpty(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? vendorCount = UserPresenter.IsVendor(Session["US_Name"] as string);
                    int? adminCount = UserPresenter.IsAdmin(Session["US_Name"] as string);
                    int? userCount = UserPresenter.IsUser(Session["US_Name"] as string);
                    if (vendorCount == 1 || adminCount == 1 || userCount == 1)
                        return View();
                    else
                    {
                        Session.Abandon();
                        return RedirectToAction("LoginUser", "User");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ProRegMsg = "Page Loading Failed - " + ex.Message;
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewBlog(BlogViewModel bvm, FormCollection fm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string username = Session["US_Name"] as string;
                    string pname = fm["PC1_Name"];

                    int bytecountBlog = bvm.ImageFileForBlog.ContentLength;
                    int bytecountBlogger = bvm.ImageFileBlogger.ContentLength;

                    if (bytecountBlog >= 5000000 && bytecountBlogger >= 5000000)
                    {
                        TempData["_extension"] = "File size must be less than 5 MB.";
                        TempData["extension"] = "File size must be less than 5 MB.";
                        return RedirectToAction("BlogUpload", "Blog");
                    }
                    else if (bytecountBlog <= 5000000 && bytecountBlogger >= 5000000)
                    {
                        TempData["_extension"] = "File size must be less than 5 MB.";
                        return RedirectToAction("BlogUpload", "Blog");
                    }
                    else if (bytecountBlog >= 5000000 && bytecountBlogger <= 5000000)
                    {
                        TempData["extension"] = "File size must be less than 5 MB.";
                        return RedirectToAction("BlogUpload", "Blog");
                    }
                    else if (bytecountBlog <= 5000000 && bytecountBlogger <= 5000000)
                    {
                        string fileNameBlogger = Path.GetFileNameWithoutExtension(bvm.ImageFileBlogger.FileName);
                        string extension = Path.GetExtension(bvm.ImageFileBlogger.FileName);

                        string fileNameBlog = Path.GetFileNameWithoutExtension(bvm.ImageFileForBlog.FileName);
                        string extensionBlog = Path.GetExtension(bvm.ImageFileForBlog.FileName);

                        if ((extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG") && (extensionBlog == ".jpg" || extensionBlog == ".jpeg" || extensionBlog == ".png" || extensionBlog == ".JPG" || extensionBlog == ".JPEG" || extensionBlog == ".PNG"))
                        {
                            fileNameBlogger = fileNameBlogger + DateTime.Now.ToString("yymmssfff") + extension;
                            bvm.Blogger_PictureName = fileNameBlogger;
                            bvm.Blogger_PicturePath = "~/Content/Pictures/BloggerPicture/" + fileNameBlogger;
                            fileNameBlogger = Path.Combine(Server.MapPath("~/Content/Pictures/BloggerPicture/"), fileNameBlogger);
                            bvm.ImageFileBlogger.SaveAs(fileNameBlogger);

                            fileNameBlog = fileNameBlog + DateTime.Now.ToString("yymmssfff") + extensionBlog;
                            bvm.Blog_PictureName = fileNameBlog;
                            bvm.Blog_PicturePath = "~/Content/Pictures/BlogPicture/" + fileNameBlog;
                            fileNameBlog = Path.Combine(Server.MapPath("~/Content/Pictures/BlogPicture/"), fileNameBlog);
                            bvm.ImageFileForBlog.SaveAs(fileNameBlog);

                            BlogPresenter bp = new BlogPresenter();
                            bp.BlogUploading(bvm, username, pname);
                            TempData["ProRegMsg"] = "Blog Uploaded Successfully.";
                            ModelState.Clear();
                            return RedirectToAction("BlogUpload", "Blog");
                        }
                        else if ((extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG") && (extensionBlog != ".jpg" || extensionBlog != ".jpeg" || extensionBlog != ".png" || extensionBlog != ".JPG" || extensionBlog != ".JPEG" || extensionBlog != ".PNG"))
                        {

                            TempData["extension"] = "Only .jpg , .jpeg & .png file supported.";
                            return RedirectToAction("BlogUpload", "Blog");
                        }
                        else if ((extension != ".jpg" || extension != ".jpeg" || extension != ".png" || extension != ".JPG" || extension != ".JPEG" || extension != ".PNG") && (extensionBlog != ".jpg" || extensionBlog != ".jpeg" || extensionBlog != ".png" || extensionBlog != ".JPG" || extensionBlog != ".JPEG" || extensionBlog != ".PNG"))
                        {
                            TempData["_extension"] = "Only .jpg , .jpeg & .png file supported.";
                            TempData["extension"] = "Only .jpg , .jpeg & .png file supported.";
                            return RedirectToAction("BlogUpload", "Blog");
                        }
                        else if ((extension != ".jpg" || extension != ".jpeg" || extension != ".png" || extension != ".JPG" || extension != ".JPEG" || extension != ".PNG") && (extensionBlog == ".jpg" || extensionBlog == ".jpeg" || extensionBlog == ".png" || extensionBlog == ".JPG" || extensionBlog == ".JPEG" || extensionBlog == ".PNG"))
                        {
                            TempData["_extension"] = "Only .jpg , .jpeg & .png file supported.";
                            return RedirectToAction("BlogUpload", "Blog");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("BlogUpload", "Blog");
                }
            }
            catch (Exception ex)
            {
                TempData["ProRegMsg"] = "Blog Uploading Failed - " + ex.Message;
            }
            return RedirectToAction("BlogUpload", "Blog");
        }
        #endregion *** BLog Upload ***

        [Route("Blog")]
        [HttpGet]
        public ActionResult ViewBlog()
        {
            bvm = bp.GetApproveBlogsGet();
            return View(bvm);
        }

        #endregion **** User Roles ****

        #region **** Admin Roles ****

        #region *** Get New Blogs ***
        [Route("blogs/new")]
        [HttpGet]
        public ActionResult GetNewBlogs()
        {
            if (string.IsNullOrEmpty(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (count == 1)
                {
                    //string path_and_query = Request.Url.PathAndQuery;
                    List<BlogViewModel> bvm = new List<BlogViewModel>();
                    BlogPresenter bp = new BlogPresenter();
                    bvm = bp.GetNewBlogsGet();
                    return View(bvm);
                }
                else
                    return RedirectToAction("Index", "User");
            }
        }
        #endregion *** Get New Blogs ***

        #region *** Get Approve Blogs ***
        [Route("blogs/approve")]
        [HttpGet]
        public ActionResult GetApproveBlogs()
        {
            try
            {
                if (string.IsNullOrEmpty(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                    if (count == 1)
                    {
                        bvm = bp.GetApproveBlogsGet();
                        return View(bvm);
                    }
                    else
                        return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex;
                return View();
            }
        }
        #endregion *** Get Approve Blogs ***

        #region *** Get Rejected Blogs ***
        [Route("blogs/rejected")]
        [HttpGet]
        public ActionResult GetNotApproveBlogs()
        {
            try
            {
                if (string.IsNullOrEmpty(Session["US_Name"] as string))
                    return RedirectToAction("LoginUser", "User");
                else
                {
                    int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                    if (count == 1)
                    {
                        bvm = bp.GetNotApproveBlogsGet();
                        return View(bvm);
                    }
                    else
                        return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex;
                return View();
            }
        }
        #endregion *** Get Rejected Blogs ***

        #region *** Delete Blog ***
        [Route("blog/delete/{id}")]
        public JsonResult DeleteBlogs(int id)
        {
            var ret = "";
            try
            {
                bp.DeleteBlog(id);
                ret = "Deleted";                
            }
            catch (Exception ex)
            {
                ret = ViewBag.error = "Blog Deletion Failed !!!" + ex;
                //return Json(ViewBag.error, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion *** Delete Blog ***

        #region *** Blog Details ***
        [Route("blogs/details/{id}")]
        [HttpGet]
        public ActionResult BlogDetails(int id)
        {
            if (string.IsNullOrEmpty(Session["US_Name"] as string))
                return RedirectToAction("LoginUser", "User");
            else
            {
                int? count = UserPresenter.IsAdmin(Session["US_Name"] as string);
                if (count == 1)
                {
                    BlogPresenter bp = new BlogPresenter();
                    _bvm = bp.GetBlogDetails(id);
                    return View(_bvm);
                }
                else
                    return RedirectToAction("Index", "User");
            }
        }
        #endregion *** Blog Details ***

        #region *** Blog Approval ***
        [Route("blog/approve/{id}")]
        public JsonResult ApproveBlog(int id)
        {
            var ret = "";
            try
            {
                bp.ApproveBlog(id);
                ret = "Approved";
            }
            catch (Exception ex)
            {
                ret = ViewBag.error = "Blog Approval Failed !!!" + ex;
                //return Json(ViewBag.error, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion *** Blog Approval ***

        #region *** Blog Rejection ***
        [Route("blog/reject/{id}")]
        public JsonResult RejectBlog(int id)
        {
            var ret = "";
            try
            {
                bp.RejectBlog(id);
                ret = "Rejected";                
            }
            catch (Exception ex)
            {
                ret = ViewBag.error = "Blog Rejection Failed !!!" + ex;
                //return Json(ViewBag.error, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion *** Blog Rejection ***

        #endregion **** Admin Roles ****

    }
}