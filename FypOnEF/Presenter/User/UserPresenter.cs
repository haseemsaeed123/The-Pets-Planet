using FypOnEF.Models.DB;
using FypOnEF.Models.User;
using FypOnEF.Presenter.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypOnEF.ViewModels.User
{
    public class UserPresenter
    {
        public List<User_Info> GetUsers()
        {
            List<User_Info> users = new List<User_Info>();

            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                users = db.User_Info.ToList();

            }

            return users;
        }

        public void SaveNewUser(UserModel u,string code)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                User_Info ui = new User_Info();
                ui.US_Name = u.US_Name;
                ui.US_Phone = u.US_Phone;
                ui.US_Address = u.US_Address;
                ui.US_Email = u.US_Email;
                ui.US_Password = u.US_Password;
                ui.U_Id = 19;
                ui.US_ConfirmCode = code;
                ui.IsValid = 0;
                db.User_Info.Add(ui);

                db.SaveChanges();
            }
            AdminsPresenter.SendEmail(u.US_Name, u.US_Email, code);
        }

        public List<User_Info> FindUser(string searchuser)
        {
            List<User_Info> user = new List<User_Info>();

            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                user = db.User_Info.Where(x => x.US_Name.Contains(searchuser)).ToList();
            }

            return user;
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

        public void EditUser(User_Info u)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
        }

        public void DeleteUser(int id)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                User_Info user = new User_Info { US_Id = id };
                db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

            }
        }

        public void VendorRegPost(RegVendorViewModel rv, String id)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region ***UserTable entry***

                        User_Info ui = new User_Info();
                        ui.US_Name = rv.US_Name;
                        ui.US_Phone = rv.US_Phone;
                        ui.US_Address = rv.US_Address;
                        ui.US_Email = rv.US_Email;
                        ui.U_Id = 12;
                        ui.US_Password = rv.US_Password;
                        ui.IsValid = 0;
                        ui.US_ConfirmCode = id;

                        db.User_Info.Add(ui);
                        #endregion   ***UserTable enrty***


                        #region ***VendorImage entry***
                        Vendor_Image vi = new Vendor_Image();
                        vi.VendorImage_Name = rv.VendorImage_Name;
                        vi.VendorImage_ImagePath = rv.VendorImage_ImagePath;

                        db.Vendor_Image.Add(vi);
                        #endregion ***VendorImage entry***


                        #region ***Vendor Details***

                        Vendor_Info Vinfo = new Vendor_Info();
                        Vinfo.VI_CNIC = rv.VI_CNIC;
                        Vinfo.VI_Province = rv.VI_Province;
                        Vinfo.VI_City = rv.VI_City;
                        Vinfo.US_Id = ui.US_Id;
                        Vinfo.VI_Approved = 0;
                        Vinfo.VI_NotApproved = 0;
                        Vinfo.VendorImage_Id = vi.VendorImage_Id;

                        db.Vendor_Info.Add(Vinfo);
                        db.SaveChanges();
                        #endregion ***Vendor Details***

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void LoginUserGet(String id)
        {
            if (id != null)
            {
                using (var db = new PetsPlanetDBContext())
                {
                    db.EmailVerification(id);
                }
            }
        }

        public static int? LoginUserPost(User_Info ui)
        {
            using (var db = new PetsPlanetDBContext())
            {
                int? count = db.UserLogin(ui.US_Name, ui.US_Password).FirstOrDefault();
                return count;
            }
        }

        public static int? CheckUserMemberShip(User_Info u)
        {
            using (var db = new PetsPlanetDBContext())
            {
                return (db.User_Info.Where(x => x.US_Name == u.US_Name).FirstOrDefault().U_Id);
            }
        }

        public static int? IsVendor(String name)
        {
            using (var db=new PetsPlanetDBContext())
            {
                return (db.User_Info.Where(x => x.US_Name == name).Where(x => x.U_Id == 12).FirstOrDefault()) != null ? 1 : 0 ;
            }
        }

        public static int? IsAdmin(String name)
        {
            using (var db = new PetsPlanetDBContext())
            {
                return (db.User_Info.Where(x => x.US_Name == name).Where(x => x.U_Id == 20).FirstOrDefault()) != null ? 1 : 0;
            }
        }

        public static int? IsUser(string name)
        {
            using (var db = new PetsPlanetDBContext())
            {
                return (db.User_Info.Where(x => x.US_Name == name).Where(x => x.U_Id == 19).FirstOrDefault()) != null ? 1 : 0;
            }
        }

        public List<RegVendorViewModel> GetNewVendor()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetNewVendor().Select(x => new RegVendorViewModel
                {
                    VI_Id = x.VI_Id,
                    US_Name = x.US_Name,
                    US_Email = x.US_Email,
                    VI_CNIC = x.VI_CNIC
                }).ToList();
                return data;
            }
        }

        public List<RegVendorViewModel> GetApproveVendor()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetApproveVendor().Select(x => new RegVendorViewModel
                {
                    VI_Id = x.VI_Id,
                    US_Name = x.US_Name,
                    US_Email = x.US_Email,
                    VI_CNIC = x.VI_CNIC
                }).ToList();
                return data;
            }
        }

        public List<RegVendorViewModel> GetNotApproveVendor()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.GetNotApproveVendor().Select(x => new RegVendorViewModel
                {
                    VI_Id = x.VI_Id,
                    US_Name = x.US_Name,
                    US_Email = x.US_Email,
                    VI_CNIC = x.VI_CNIC
                }).ToList();
                return data;
            }
        }

        public RegVendorViewModel GetVendorDetails(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                var details = db.VendorDetails(id);
                UserPresenter pp = new UserPresenter();
                RegVendorViewModel rvm = new RegVendorViewModel();
                foreach (var item in details)
                {
                    rvm.US_Name = item.US_Name;
                    rvm.US_Phone = item.US_Phone;
                    rvm.US_Address = item.US_Address;
                    rvm.US_Email = item.US_Email;
                    rvm.VI_CNIC = item.VI_CNIC;
                    rvm.VI_Province = item.VI_Province;
                    rvm.VI_City = item.VI_City;
                    rvm.VendorImage_Id = item.VendorImage_Id;
                    rvm.VendorImage_ImagePath = item.VendorImage_ImagePath;
                }
                return rvm;
            }
        }

        public void ApproveVendor(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.ApprovVendor(id);
            }
        }

        public void RejectVendor(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                db.NotApprovVendor(id);
            }
        }
    }
}