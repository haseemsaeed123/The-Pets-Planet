using FypOnEF.Models.DB;
using FypOnEF.ViewModels.Pet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypOnEF.Presenter.Pet
{
    public class PetPresenter
    {
        public List<SelectListItem> GetPets()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                return (db.Pets_C1.Select(x => new SelectListItem { Text = x.PetsC1_Name, Value = x.PetsC1_Id.ToString() }).ToList());
            }
        }

        public List<SelectListItem> GetBreed()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                return (db.Pets_C2.Select(x => new SelectListItem { Text = x.PetsC2_Name, Value = x.PetsC2_Id.ToString() }).ToList());
            }
        }
        public void PetUpload(PetViewModel pvm, string username)
        {
            DateTime today = DateTime.Today;
            using (var db = new PetsPlanetDBContext())
            {
                db.UserPetsEntry(username, pvm.PetsC1_Id, pvm.PetsC2_Id, pvm.PetsC3_Name, pvm.Descrption, pvm.Cost, pvm.PetsImage_Path, pvm.PetsImage_Name);
            }
        }

        public List<PetViewModel> PetApproved()
        {
            using (PetsPlanetDBContext db = new PetsPlanetDBContext())
            {
                var data = db.PetsPanelView().Select(x => new PetViewModel
                {
                    PetsC1_Name = x.PetsC1_Name,
                    PetsC3_Id = x.PetsC3_Id,
                    PetsC3_Name = x.PetsC3_Name,
                    PetsImage_Path = x.PetsImage_Path,
                    Cost = x.Cost
                }).ToList();
                return data;
            }
        }

        public PetViewModel PetDetails(int id)
        {
            using (var db = new PetsPlanetDBContext())
            {
                var details = db.SelectedPetDetails(id);
                PetViewModel pvm = new PetViewModel();
                foreach (var item in details)
                {
                    pvm.PetsC1_Name = item.PetsC1_Name;
                    pvm.PetsC3_Id = item.PetsC3_Id;
                    pvm.PetsC3_Name = item.PetsC3_Name;
                    pvm.PetsImage_Path = item.PetsImage_Path;
                    pvm.Cost = item.Cost;
                    pvm.Descrption = item.Descrption;
                    pvm.US_Name = item.US_Name;
                    pvm.US_Phone = item.US_Phone;
                }
                return pvm;
            }
        }
    }
}