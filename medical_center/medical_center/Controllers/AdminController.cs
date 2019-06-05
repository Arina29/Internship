using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MED.DAL.Models;
using MED.DAL.UnitOfWork;
using MED.Presentation.App_Start;
using MED.Presentation.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace MED.Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _context;

        public AdminController(IUnitOfWork _unitOfWork)
        {
            _context = _unitOfWork;
        }

        public ActionResult RegisterDoctor()
        {
            return View("RegisterDoctor");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterDoctor(DoctorRegisterViewModel model)
        {
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (ModelState.IsValid)
            {
                var doctor = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(doctor, model.Password);
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    await roleManager.CreateAsync(new IdentityRole(("Doctor")));
                    await UserManager.AddToRoleAsync(doctor.Id, "Doctor");
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult GetAllDoctors()
        {
            var doctors = _context.DoctorsRepository.Find(x => x.IsDeleted == false);
            var doctorsViewModels = Mapper.Map<List<Doctors>, List<DoctorFormViewModels>>(doctors);
            foreach (var item in doctorsViewModels)
            {
                var doc = _context.DoctorsRepository.Get(item.Id);
                var specialty = _context.SpecializationRepository.GetBy(x => x.Id == item.SelectedSpecialization);
                item.Specialty = specialty.Name;
                var img = _context.ImagesRepository.GetBy(x => x.DoctorId == item.Id);
                item.ImagePath = img.FilePath + @"\" + img.FileName;
            }
            return View("DoctorList", doctorsViewModels);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid Id)
        {
            var doctor = _context.DoctorsRepository.GetBy(x => x.Id == Id);
            doctor.IsDeleted = true;
            _context.DoctorsRepository.Edit(doctor);
            return RedirectToAction("GetAllDoctors");
        }
    }
}