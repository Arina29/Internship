using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MED.DAL.Models;
using MED.DAL.UnitOfWork;

namespace MED.BLL.Implimentation
{
    public class DoctorBll : IDoctorBll
    {
        IUnitOfWork _context;

        public DoctorBll(IUnitOfWork unitOfWork)
        {
            _context = unitOfWork;
        }

        Dictionary<int, string> dict = new Dictionary<int, string>
        {
            {0, "Sunday"},
            {1, "Monday"},
            {2, "Tuesday"},
            {3, "Wednesday"},
            {4, "Thursday"},
            {5, "Friday" }
        };

        public BaseModel InitializeBaseModel(BaseModel model)
        {
            model.CreateTime = DateTime.Now;
            model.IsDeleted = false;
            return model;
        }

        public void SaveOrUpdate(Doctors entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                InitializeBaseModel(entity);
                _context.DoctorsRepository.Add(entity);
            }
            else
            {
                _context.DoctorsRepository.Edit(entity);
            }
        }

        public Images SaveImage(HttpPostedFileBase img, Doctors entity)
        {
            var image = new Images();
            if (img == null || img.ContentLength <= 0)
                return null;

            image.Id = Guid.NewGuid();
            image.DoctorId = entity.Id;
            image.Extension = Path.GetExtension(img.FileName);
            image.FileName = Guid.NewGuid().ToString() + image.Extension;
            image.FilePath = @"\images\" + DateTime.Today.ToString("yyyy-MM-dd");

            entity.Images = new List<Images>()
            {
                image
            };
            _context.ImagesRepository.Add(image);

            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(image.FilePath));
            if (img != null)
                img.SaveAs(HttpContext.Current.Server.MapPath(Path.Combine(image.FilePath, image.FileName)));

            return image;
        }

        public List<Schedule> GetDoctorSchedule(string UserId)
        {
            var doctor = _context.DoctorsRepository.GetBy(m => m.UserId == UserId);
            var scheduleId = _context.DoctorScheduleRepository.GetBy(x => x.DoctorId == doctor.Id);
            if(scheduleId == null)
                return new List<Schedule>();

            var schedule = _context.ScheduleRepository.GetAllBy(m => m.DoctorScheduleId == scheduleId.Id);
            return schedule;
        }

        public List<Schedule> GetDoctorWorkingDays(string UserId)
        {
            var schedule = GetDoctorSchedule(UserId);
            if(schedule == null)
                return new List<Schedule>();

            return schedule.Where(x => x.IsWorking == true).ToList();
        }

        public void InitializeDoctorSchedule(Guid Id)
        {
            var doctorSchedule = new DoctorSchedule()
            {
                Id = Guid.NewGuid(),
                DoctorId = Id
            };
            _context.DoctorScheduleRepository.Add(doctorSchedule);

            string[] days = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday"};
            DateTime time = DateTime.Now;

            foreach (var item in days)
            {
                var schedule = new Schedule()
                {
                    Id = Guid.NewGuid(),
                    Day = item,
                    IsWorking = true,
                    DoctorScheduleId = doctorSchedule.Id,
                    LunchStart = new DateTime(time.Year, time.Month, time.Day, 13, 0, 0),
                    LunchEnd = new DateTime(time.Year, time.Month, time.Day, 14, 0, 0),
                    WorkStart = new DateTime(time.Year, time.Month, time.Day, 8, 0, 0),
                    WorkEnd = new DateTime(time.Year, time.Month, time.Day, 18, 0, 0)
                };
                _context.ScheduleRepository.Add(schedule);
            }
        }

        public List<Schedule> SortByDayWeek(List<Schedule> schedule)
        {
            List<Schedule> result = new List<Schedule>();
            for (int i = 0; i < dict.Count; i++)
            {
                var day = schedule.SingleOrDefault(x => x.Day == dict[i]);
                if (day != null)
                {
                    result.Add(day);
                }
            }

            return result;
        }

        public bool VerifyDate(DateTime Date, Guid DoctorId)
        {
            var appointment = _context.AppointmentRepository.Find(x => x.DoctorId == DoctorId);
            var doctor = _context.DoctorsRepository.Get(DoctorId);
            var schedule = GetDoctorWorkingDays(doctor.UserId);

            var day = Date.DayOfWeek.ToString();
            var hour = Date.Hour;

            foreach (var item in schedule)
            {
                if (item.Day == day)
                {
                    if (item.WorkStart.Hour > hour || item.WorkEnd.Hour <= hour ||
                        item.LunchStart.Hour == hour)
                    {
                        return false;
                    }
                }
            }
            if(appointment.Any(x => x.AppointmentDate == Date))
                return false;

            return true;
        }

        public int[] GetWorkingDays(Guid Id)
        {
            var doctor = _context.DoctorsRepository.Get(Id);
            var schedule = GetDoctorWorkingDays(doctor.UserId);
            var weekdict = new Dictionary<string,  int>
            {
                {"Sunday", 0},
                {"Monday", 1},
                {"Tuesday", 2},
                {"Wednesday", 3},
                {"Thursday", 4},
                {"Friday", 5},
                {"Saturday", 6}
            };
            List<int> days = new List<int>();

            foreach (var item in weekdict)
            {
                if(schedule.All(x => x.Day != item.Key))
                    days.Add(item.Value);
            }

            return days.ToArray();
        }

        public string[] GetWorkingHours(DateTime day, Guid DoctorId)
        {
            var doctor = _context.DoctorsRepository.Get(DoctorId);
            var doctorSchedule = GetDoctorWorkingDays(doctor.UserId);
            List<string> workingHours = new List<string>();

            var daySchedule = doctorSchedule.SingleOrDefault(x => x.Day == day.DayOfWeek.ToString());
            for (var dt = daySchedule.WorkStart; dt < daySchedule.WorkEnd; dt = dt.AddHours(1))
            {
                workingHours.Add(dt.Hour.ToString() + ":00");
            }

            for (var i = daySchedule.LunchStart; i < daySchedule.LunchEnd; i = i.AddHours(1))
                workingHours.Remove(i.Hour.ToString() + ":00");

            var appointment = _context.AppointmentRepository.Find(x => x.DoctorId == DoctorId);
            if (appointment.Any(x => x.AppointmentDate.Date == day.Date))
            {
                foreach(var item in appointment.Where(x => x.AppointmentDate.Date == day.Date))
                    workingHours.Remove(item.AppointmentDate.Hour.ToString() + ":00");
            }
            return workingHours.ToArray();
        }

        public void AddPatientAnalysis(byte[] file, Guid PatientId)
        {
           var analysis = new PatientAnalysis();
            analysis.PatientId = PatientId;
            analysis.File = file;
            analysis.Id = Guid.NewGuid();
            InitializeBaseModel(analysis);
            _context.PatientAnalysisRepository.Add(analysis);
        }

        //public static Bitmap ResizeImage(Image image, int width, int height)
        //{
        //    var destRect = new Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }
        //    return destImage;
        //}
    }
}
