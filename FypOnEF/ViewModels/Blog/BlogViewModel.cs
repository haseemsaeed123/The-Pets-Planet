using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FypOnEF.ViewModels.Blog
{
    public class BlogViewModel
    {
        public int Blog_Id { get; set; }

        [Required(ErrorMessage = "*Title is required.")]
        [Display(Name ="Blog Title")]
        public string Blog_Title { get; set; }

        public DateTime Blog_Date { get; set; }

        public string Blogger_PicturePath { get; set; }

        public string Blog_PicturePath { get; set; }

        [Required(ErrorMessage = "*Blog is required.")]
        [Display(Name = "Your Blog Area")]
        public string Blog_WholeBlog { get; set; }

        public int US_Id { get; set; }

        public string US_Name { get; set; }

        public string Blogger_PictureName { get; set; }

        public string Blog_PictureName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "*Picture is required.")]
        [Display(Name = "Upload Your Picture")]
        public HttpPostedFileBase ImageFileBlogger { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "*Picture is required.")]
        [Display(Name = "Your Blog Picture")]
        public HttpPostedFileBase ImageFileForBlog { get; set; }
    }
}