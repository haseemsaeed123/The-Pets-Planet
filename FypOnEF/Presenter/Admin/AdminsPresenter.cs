using FypOnEF.Models.DB;
using FypOnEF.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace FypOnEF.Presenter.Admin
{
    public class AdminsPresenter
    {
        public static void SendEmail(String name, String email, String id)
        {
            string body = "http://localhost:64881/login/" + id + "";
            WebMail.Send(email, "ACCOUNT VERIFICATION"
            , "Hello " + name + "<br/><br/>Please Verify your account. Open the link Below.<br/><br/>" + body
            , null, null, null, true, null, null, null, null, null
            , email);
        }

        public void SaveNewAdmin(UserModel u,string code)
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                User_Info ui = new User_Info();
                ui.US_Name = u.US_Name;
                ui.US_Phone = u.US_Phone;
                ui.US_Address = u.US_Address;
                ui.US_Email = u.US_Email;
                ui.US_Password = u.US_Password;
                ui.U_Id = 20;
                ui.US_ConfirmCode = code;
                ui.IsValid = 0;
                db.User_Info.Add(ui);
                db.SaveChanges();
            }
            AdminsPresenter.SendEmail(u.US_Name, u.US_Email, code);
        }
    }


}