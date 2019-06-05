using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MED.BLL.Implimentation;
using MED.DAL.Models;
using MED.DAL.UnitOfWork;
using MED.Presentation.Models;
using MED.Presentation.Utils;
using Microsoft.AspNet.Identity;

namespace MED.Presentation.Controllers
{
    public class PatientFormController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IUserBll _userBll;


        public PatientFormController(IUnitOfWork unitOfWork, IUserBll userBll)
        {
            _context = unitOfWork;
            _userBll = userBll;
        }

        [Authorize(Roles = RoleConstants.UserRole)]
        public ActionResult RegisterPatientForm(string UserId)
       {
            var patient = new PatientFormViewModels();
            if (UserId != null)
            {
                var model = _context.PacientRepository.GetBy(x => x.UserId == UserId);
                patient = Mapper.Map<Patient, PatientFormViewModels>(model);
                patient.Image1 = model.Image;
                patient.SelectedGender = model.Gender;
            }

           patient.Genders = _userBll.GetGenderTypes();
            return View("RegisterPatientForm", patient);
        }

        // GET: RegisterPatientForm
        [HttpPost]
        public ActionResult RegisterPatientForm(PatientFormViewModels model)
        {
            ModelState.Remove("ErrorKey");            
            if (ModelState.IsValid)
            {
                if (model.Id == null)
                {
                    var entity = Mapper.Map<PatientFormViewModels, Patient>(model);
                    entity.UserId = User.Identity.GetUserId();
                    if(entity.Image.Length > 0)
                        entity.Image = Utils.Utils.ImageToByte(model.Image);
                    _userBll.SaveOrUpdate(entity);
                    return RedirectToAction("PatientProfile", "PatientForm", new{ entity.UserId});
                }
                else
                {
                    var entity = _context.PacientRepository.Get(model.Id);
                    var img = entity.Image; 
                    entity = Mapper.Map<PatientFormViewModels, Patient>(model);
                    entity.Gender = model.SelectedGender;
                    entity.Image = model.Image == null ? img : Utils.Utils.ImageToByte(model.Image);
                    _userBll.SaveOrUpdate(entity);
                    return RedirectToAction("PatientProfile", "PatientForm", new { entity.UserId });
                }
            }

            model.Genders = _userBll.GetGenderTypes();
            return View(model);
        }

        public ActionResult PatientProfile(string UserId)
        {
            var a = System.Web.HttpContext.Current.User;
            if (UserId != null)
            {
                var patient = new PatientFormViewModels();
                var entity = _context.PacientRepository.GetBy(x => x.UserId == UserId);
                if (entity != null)
                {
                    patient = Mapper.Map<Patient, PatientFormViewModels>(entity);
                    if (entity.Image != null)
                        patient.Image1 = entity.Image;
                    patient.Age = _userBll.CalculateAges(entity);


                    var genders = _userBll.GetGenderTypes();
                    var analysis = _context.PatientAnalysisRepository.Find(x => x.PatientId == patient.Id);
                    var book = _userBll.PatientBook(patient.Id).ToList();

                    patient.MedicalBook = new MedicalBookViewModel()
                    {
                        Id = patient.Id,
                        Book = Mapper.Map<List<Book>, List<BookViewModel>>(book),
                        Analysis = Mapper.Map<List<PatientAnalysis>, List<AnalysisViewModel>>(analysis),
                    };
                    foreach (var item in patient.MedicalBook.Book)
                    {
                        foreach (var type in _userBll.GetDiseaseTypes())
                        {
                            if (item.DiseaseType == type.Id)
                                item.DiseaseTypeName = type.Name;
                        }

                        var Doctor = _context.DoctorsRepository.GetBy(x => x.Id == item.DoctorId);
                        item.Doctor = Doctor.Name + " " + Doctor.Surname;
                    }

                    foreach (var item in genders)
                    {
                        if (item.Id == entity.Gender)
                            patient.Gender = item.Name;
                    }

                    patient.Diagnosis = new BookViewModel
                    {
                        DiseaseTypes = _userBll.GetDiseaseTypes()
                    };

                    return View("PatientProfile", patient);
                }
                else
                {
                    patient.Genders = _userBll.GetGenderTypes();
                    return View("RegisterPatientForm", patient);
                }
            }
            return View("Error");
        }

        [Authorize(Roles = RoleConstants.DoctorRole)]
        public ActionResult PatientList()
        {
            var patients = _context.PacientRepository.GetAll();
            return View("PatientList", patients);
        }

        [Authorize(Roles = RoleConstants.DoctorRole)]
        [HttpPost]
        public ActionResult AddDiagnosis(BookViewModel model)
        {
            var book = new Book();
            var doctorUserId = User.Identity.GetUserId();
            var doctor = _context.DoctorsRepository.GetBy(d => d.UserId == doctorUserId);
            model.DoctorId = doctor.Id;
            book = Mapper.Map<BookViewModel, Book>(model);
            book.DiagnosticsDate = model.DiagnosticDate;
            _userBll.InitializeBaseModel(book);

            _userBll.AddNewBook(book, model.PatientId);
            var patientUserId = _context.PacientRepository.Get(model.PatientId);
            return Json(new { newUrl = Url.Action("PatientProfile", "PatientForm", new { patientUserId.UserId }) });
        }

        [Authorize(Roles = RoleConstants.UserRole)]
        public ActionResult PersonalAppointments(Guid Id)
        {
            var appointment = _context.AppointmentRepository.Find(x => x.PatientId == Id && x.AppointmentDate > DateTime.Today);
            appointment = appointment.OrderByDescending(x => x.AppointmentDate).ToList();
            var appointmentViewModel = Mapper.Map<List<Appointment>, List<AppointmentViewModel>>(appointment);
            foreach (var item in appointmentViewModel)
            {
                var doc = _context.DoctorsRepository.Get(item.DoctorId);
                item.Doctor = doc.Name +" " + doc.Surname;
                var specialty = _context.SpecializationRepository.GetBy(x => x.Id == doc.SpecializationId);
                item.DoctorSpecialty = specialty.Name;
            }
            return View("PatientAppointmentList", appointmentViewModel.OrderBy(x => x.Date));
        }

        [Authorize(Roles = RoleConstants.UserRole)]
        public ActionResult DeleteAppointment(Guid Id)
        {
            _userBll.DeleteAppointment(Id);
            return RedirectToAction("PatientProfile", new{UserId = User.Identity.GetUserId()});
        }

        [Authorize(Roles = RoleConstants.DoctorRole)]
        public ActionResult AddAnalysis(Guid Id)
        {
            var patientBook = _context.PatientBookRepository.GetBy(x => x.BookId == Id);

            return PartialView("_PatientBook");
        }

        public FileResult OpenAnalysis(Guid Id)
        {
            var analysis = _context.PatientAnalysisRepository.GetBy(x => x.Id == Id);
            return File(analysis.File, "application/pdf");
        }
    }
}