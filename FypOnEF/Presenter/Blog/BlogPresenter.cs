using FypOnEF.Models.DB;
using FypOnEF.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypOnEF.Presenter.Blog
{
    public class BlogPresenter
    {
        public void BlogUploading(BlogViewModel bg, string username, string pname)
        {
            DateTime today = DateTime.Today;
            using (var db = new PetsPlanetDBContext())
            {
                db.BlogUploading(username, bg.Blog_Title, today.ToString(), bg.Blogger_PicturePath, bg.Blog_PicturePath, bg.Blog_WholeBlog, bg.Blogger_PictureName, bg.Blog_PictureName);
            }
        }

        public List<BlogViewModel> GetNewBlogsGet()
        {
            using (var db = new PetsPlanetDBContext())
            {
                var data = db.GetNewBlogs().Select(x => new BlogViewModel
                {
                    Blog_Id = x.Blog_Id,
                    Blog_Title = x.Blog_Title,
                    Blog_Date = Convert.ToDateTime(x.Blog_Date),
                    Blog_WholeBlog = x.Blog_WholeBlog,
                    Blogger_PicturePath=x.Blogger_PicturePath,
                    Blog_PicturePath=x.Blog_PicturePath,
                    US_Name=x.US_Name
                }).ToList();
                return data;
            }
            
        }

        public List<BlogViewModel> GetApproveBlogsGet()
        {
            using (var db = new PetsPlanetDBContext())
            {
                var data = db.GetApproveBlogs().Select(x => new BlogViewModel
                {
                    Blog_Id = x.Blog_Id,
                    Blog_Title = x.Blog_Title,
                    Blog_Date = Convert.ToDateTime(x.Blog_Date),
                    Blog_WholeBlog = x.Blog_WholeBlog,
                    Blogger_PicturePath = x.Blogger_PicturePath,
                    Blog_PicturePath = x.Blog_PicturePath,
                    US_Name = x.US_Name
                }).ToList();
                return data;
            }
        }

        public List<BlogViewModel> GetNotApproveBlogsGet()
        {
            using (var db = new PetsPlanetDBContext())
            {
                var data = db.GetNotApproveBlogs().Select(x => new BlogViewModel
                {
                    Blog_Id = x.Blog_Id,
                    Blog_Title = x.Blog_Title,
                    Blog_Date = Convert.ToDateTime(x.Blog_Date),
                    Blog_WholeBlog = x.Blog_WholeBlog,
                    Blogger_PicturePath = x.Blogger_PicturePath,
                    Blog_PicturePath = x.Blog_PicturePath,
                    US_Name = x.US_Name
                }).ToList();
                return data;
            }
        }

        public void DeleteBlog(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.DeleteBlog(id);
            }
        }

        public BlogViewModel GetBlogDetails(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                var data = db.BlogDetailsGet(id);
                BlogViewModel bvm = new BlogViewModel();
                foreach(var x in data)
                {
                    bvm.Blog_Id = x.Blog_Id;
                    bvm.Blog_Title = x.Blog_Title;
                    bvm.Blog_Date = Convert.ToDateTime(x.Blog_Date);
                    bvm.Blog_WholeBlog = x.Blog_WholeBlog;
                    bvm.Blogger_PicturePath = x.Blogger_PicturePath;
                    bvm.Blog_PicturePath = x.Blog_PicturePath;
                    bvm.US_Name = x.US_Name;
                }
                return bvm;
            }
        }

        public void ApproveBlog(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.ApproveBlog(id);
            }
        }

        public void RejectBlog(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.NotApproveBlog(id);
            }
        }

        public List<BlogViewModel> BlogIndexPanel()
        {
            using (var db = new PetsPlanetDBContext())
            {
                var data = db.BlogDisplay().Select(x => new BlogViewModel
                {
                    Blog_Id = x.Blog_Id,
                    Blog_Title = x.Blog_Title,
                    Blog_Date = Convert.ToDateTime(x.Blog_Date),
                    Blog_WholeBlog = x.Blog_WholeBlog.Substring(0,5),
                    Blogger_PicturePath = x.Blogger_PicturePath,
                    Blog_PicturePath = x.Blog_PicturePath,
                    US_Name = x.US_Name
                }).ToList();
                return data;
            }
        }

    }
}