using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AutoMapper;
using MED.BLL.Implimentation;
using MED.DAL.Models;
using MED.DAL.UnitOfWork;
using MED.Presentation.Utils;
using MED.Presentation.ViewModels;
using Microsoft.AspNet.Identity;

namespace MED.Presentation.Controllers
{
    public class DoctorProfileController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IDoctorBll _doctorBll;

        public DoctorProfileController(IUnitOfWork unitOfWork, IDoctorBll doctorBll)
        {
            _context = unitOfWork;
            _doctorBll = doctorBll;
        }

        [Authorize(Roles = RoleConstants.DoctorRole)]
        public ActionResult DoctorForm(string UserId)
        {
            var doctor = new DoctorFormViewModels();
            if (UserId != null)
            {
                var model = _context.DoctorsRepository.GetBy(x => x.UserId == UserId);
                doctor = Mapper.Map<Doctors, DoctorFormViewModels>(model);
                var img = _context.ImagesRepository.GetBy(x => x.DoctorId == model.Id);
                doctor.ImagePath = img.FilePath + @"\" + img.FileName;
                doctor.SelectedSpecialization = model.SpecializationId;
            }   
            doctor.Specialization = _context.SpecializationRepository.GetAll();
            return View("DoctorForm", doctor);
        }

        [HttpPost]
        public ActionResult DoctorForm(DoctorFormViewModels model)
        {
            ModelState.Remove("ErrorKey");
            if (ModelState.IsValid)
            {
                if (model.Id == null)
                {
                    var entity = Mapper.Map<DoctorFormViewModels, Doctors>(model);
                    entity.SpecializationId = model.SelectedSpecialization;
                    entity.UserId = User.Identity.GetUserId();
                    _doctorBll.SaveOrUpdate(entity);
                    _doctorBll.InitializeDoctorSchedule(entity.Id);
                    var img = _doctorBll.SaveImage(model.Image, entity);
                    return RedirectToAction("Profile", "DoctorProfile", new { UserId = entity.UserId });

                }
                else
                {
                    var entity = _context.DoctorsRepository.Get(model.Id);
                    entity = Mapper.Map<DoctorFormViewModels, Doctors>(model);
                    entity.SpecializationId = model.SelectedSpecialization;
                    _doctorBll.SaveOrUpdate(entity);

                    var img = _context.ImagesRepository.GetAllBy(x => x.DoctorId == model.Id);

                    if (model.Image != null)
                    {
                        _context.ImagesRepository.RemoveRange(img);
                        _doctorBll.SaveImage(model.Image, entity);
                    }
                    return  RedirectToAction("Profile", "DoctorProfile", new { UserId = entity.UserId });
                }
            }
            var image = _context.ImagesRepository.GetBy(x => x.DoctorId == model.Id);
            model.ImagePath = image.FilePath + @"\" + image.FileName;
            model.Specialization = _context.SpecializationRepository.GetAll();
            return View(model);
        }

        public ActionResult Profile(string UserId)
        {
            if (UserId != null)
            {
                var entity = _context.DoctorsRepository.Find(x => x.UserId == UserId).FirstOrDefault();
                if (entity != null)
                {
                    var doctor = Mapper.Map<Doctors, DoctorFormViewModels>(entity);
                    var img = _context.ImagesRepository.GetBy(x => x.DoctorId == entity.Id);
                    doctor.ImagePath = img.FilePath + @"\" + img.FileName;
                    doctor.SelectedSpecialization = entity.SpecializationId;

                    var reviews = _context.DoctorReviewRepository.Find(x => x.DoctorId == doctor.Id);
                    doctor.Reviews = Mapper.Map<List<DoctorReview>, List<ReviewViewModel>>(reviews);

                    foreach (var item in doctor.Reviews)
                    {
                        var patient = _context.PacientRepository.Get(item.PatientId);
                        item.PatientName = patient.Name + " " + patient.Surname;
                        item.PatientImage = patient.Image;
                    }
                    doctor.Reviews = doctor.Reviews.OrderByDescending(x => x.CreateTime);

                    var specialization = _context.SpecializationRepository.Get(entity.SpecializationId);
                    doctor.Specialty = specialization.Name;

                    var doctorSchedule = _doctorBll.GetDoctorWorkingDays(entity.UserId);
                    doctorSchedule = _doctorBll.SortByDayWeek(doctorSchedule);
                    doctor.Schedule = Mapper.Map<List<Schedule>, List<ScheduleViewModel>>(doctorSchedule);

                    var guestUserId = User.Identity.GetUserId();
                    if (User.IsInRole("User"))
                    {
                        var guest = _context.PacientRepository.GetBy(x => x.UserId == guestUserId);
                        doctor.PatientImage = guest.Image;
                    }
                    return View("DoctorProfile", doctor);
                }
                else
                    return RedirectToAction("DoctorForm", "DoctorProfile", new { id = UserId });
            }
            return View("Error");
        }

        [Authorize(Roles = RoleConstants.DoctorRole)]
        public ActionResult SearchPatient(string SearchString)
        {
            if (!SearchString.IsEmpty())
            {
                var patient = _context.PacientRepository.GetBy(x => x.IDNP == SearchString);
                if (patient == null)
                    return View("UnexistentPatientError");
                return RedirectToAction("PatientProfile", "PatientForm", new { UserId = patient.UserId });
            }
            return RedirectToAction("PatientList", "PatientForm");
        }

        public ActionResult DoctorList()
        {
            var doctors = _context.DoctorsRepository.Find(x => x.IsDeleted == false);
            var doctorsViewModels = Mapper.Map<List<Doctors>, List<DoctorFormViewModels>>(doctors);
            foreach (var item in doctorsViewModels)
            {
                var img = _context.ImagesRepository.GetBy(x => x.DoctorId == item.Id);
                item.ImagePath = img.FilePath + @"\" + img.FileName;
                var specialty = _context.SpecializationRepository.GetBy(x => x.Id == item.SelectedSpecialization);
                item.Specialty = specialty.Name;
            }
            return View("DoctorList", doctorsViewModels);
        }

        [Authorize(Roles = RoleConstants.DoctorRole)]
        public ActionResult DoctorSchedule(string UserId)
        {
            var schedule = _doctorBll.GetDoctorSchedule(UserId);
            schedule = _doctorBll.SortByDayWeek(schedule);
            var docSchedule = Mapper.Map<List<Schedule>, List<ScheduleViewModel>>(schedule);

            return View("DoctorSchedule", docSchedule);
        }

        [Authorize(Roles = RoleConstants.DoctorRole)]
        [HttpPost]
        public ActionResult UpdateSchedule(ScheduleViewModel model)
        {
            var schedule = _context.ScheduleRepository.Get(model.Id);
            var doctorschedule = _context.DoctorScheduleRepository.GetBy(x => x.Id == schedule.DoctorScheduleId);
            var appointments = _context.AppointmentRepository.Find(x => x.DoctorId == doctorschedule.DoctorId);
            if (appointments.Any(x => x.AppointmentDate.DayOfWeek.ToString() == model.Day) && model.IsWorking == false)
            {
                return Json(new {success = "Invalid", responseText = "You have appointments in this day"}, JsonRequestBehavior.AllowGet);
            }
            var scheduleId = schedule.DoctorScheduleId;
            schedule.Day = model.Day;
            schedule.IsWorking = model.IsWorking;
            schedule.LunchEnd = model.LunchEnd;
            schedule.LunchStart = model.LunchStart;
            schedule.WorkStart = model.WorkStart;
            schedule.WorkEnd = model.WorkEnd;
            schedule.DoctorScheduleId = scheduleId;

            _context.ScheduleRepository.Edit(schedule);
            return Json(new
            {
                newUrl = Url.Action("Profile", "DoctorProfile", new {UserId = User.Identity.GetUserId()})
            });
        }

        public ActionResult Appointment(Guid DoctorId)
        {
            var appointment = new AppointmentViewModel();
            var Id = User.Identity.GetUserId();
            var patient = _context.PacientRepository.GetBy(x => x.UserId == Id);
            if (patient == null)
                return View("Error");
            appointment = Mapper.Map<Patient, AppointmentViewModel>(patient);
            appointment.DoctorId = DoctorId;

            appointment.WorkingDays = _doctorBll.GetWorkingDays(DoctorId);

            return View("Appointment", appointment);
        }

        [HttpPost]
        public ActionResult Appointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = User.Identity.GetUserId();
                var patient = _context.PacientRepository.GetBy(x => x.UserId == id);
                model.PatientId = patient.Id;
                var appointment = Mapper.Map<AppointmentViewModel, Appointment>(model);
                _context.AppointmentRepository.Add(appointment);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult VerifyDate(string date, Guid DoctorId)
        {
            DateTime AppointmentDate = DateTime.Parse(date);
            var isAvailable = _doctorBll.VerifyDate(AppointmentDate, DoctorId);

            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetWorkingHours(string day, Guid DoctorId)
        {
            DateTime Day = DateTime.Parse(day);
            var days = _doctorBll.GetWorkingHours(Day, DoctorId);
            return Json(days, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddReview(ReviewViewModel model)
        {
            string UserId = User.Identity.GetUserId();
            var patient = _context.PacientRepository.GetBy(x => x.UserId == UserId);
            model.PatientId = patient.Id;
            var review = Mapper.Map<ReviewViewModel, DoctorReview>(model);
            _context.DoctorReviewRepository.Add(review);

            var doctor = _context.DoctorsRepository.GetBy(x => x.Id == model.DoctorId);
            return Json(new { newUrl = Url.Action("Profile", "DoctorProfile", new { UserId = doctor.UserId}) });
        }

        [HttpPost]
        public ActionResult AddAnalysis()
        {
            if (Request.Files.Count > 0)
            {
           
                var patientId = new Guid(Request.Form["guid"]);
                HttpPostedFileBase analysisFile = Request.Files["file"];
                var analysis = Utils.Utils.ImageToByte(analysisFile);
                _doctorBll.AddPatientAnalysis(analysis, patientId);

                return Json("File Uploaded Successfully!");
               
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public ActionResult GetAppointments(string UserId)
        {
            var doctor = _context.DoctorsRepository.GetBy(x => x.UserId == UserId);
            var appointments = _context.AppointmentRepository.Find(x => x.DoctorId == doctor.Id);
            appointments = appointments.OrderBy(x => x.AppointmentDate)
                .Where(x => x.Canceled == false && DateTime.Compare(x.AppointmentDate, DateTime.Now)>= 0).ToList();
            var app = Mapper.Map<List<Appointment>, List<AppointmentViewModel>>(appointments);

            foreach (var item in app)
            {
                var patient = _context.PacientRepository.Get(item.PatientId);
                item.Name = patient.Name;
                item.Surname = patient.Surname;
                item.IDNP = patient.IDNP;
                item.Mail = patient.Mail;
                item.Mobile = patient.Mobile;
                item.Day = item.Date.DayOfWeek.ToString();
            }
            return View("DoctorAppointmentList", app.AsEnumerable());
        }
    }
}