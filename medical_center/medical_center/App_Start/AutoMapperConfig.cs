using System;
using AutoMapper;
using MED.DAL.Models;
using MED.Presentation.Models;

namespace MED.Presentation.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(arg =>
            {
                arg.CreateMap<PatientFormViewModels, Patient>();
                arg.CreateMap<Patient, PatientFormViewModels>()
                    .ForMember(dest => dest.Image, src => src.Ignore());


                arg.CreateMap<DoctorFormViewModels, Doctors>();
                arg.CreateMap<Doctors, DoctorFormViewModels>()
                    .ForMember(dest=> dest.SelectedSpecialization, src => src.MapFrom(s=>s.SpecializationId));

                arg.CreateMap<BookViewModel, Book>();
                arg.CreateMap<Book, BookViewModel>()
                    .ForMember(dest=> dest.DiagnosticDate, src => src.MapFrom(s => s.DiagnosticsDate));

                arg.CreateMap<Schedule, ScheduleViewModel>();
                arg.CreateMap<ScheduleViewModel, Schedule>();

                arg.CreateMap<PatientAnalysis, AnalysisViewModel>();

                arg.CreateMap<Patient, AppointmentViewModel>()
                    .ForMember(dest => dest.Id, opt=> opt.MapFrom(x =>Guid.NewGuid()));
                arg.CreateMap<AppointmentViewModel, Appointment>()
                    .ForMember(dest => dest.AppointmentDate, src => src.MapFrom(s=> s.Date));

                arg.CreateMap<Appointment, AppointmentViewModel>()
                    .ForMember(dest => dest.Date, src => src.MapFrom(s => s.AppointmentDate));

                arg.CreateMap<ReviewViewModel, DoctorReview>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(x => DateTime.Today));
                arg.CreateMap<DoctorReview, ReviewViewModel>();

                arg.CreateMap<DateTimeOffset, DateTime>().ConvertUsing<DateTimeOffsetToDateTimeConverter>();
                arg.CreateMap<DateTimeOffset, DateTime?>().ConvertUsing<DateTimeOffsetToDateTimeNullableConverter>();
                arg.CreateMap<DateTimeOffset?, DateTime?>().ConvertUsing<DateTimeOffsetNullableToDateTimeNullableConverter>();
                arg.CreateMap<DateTime?, DateTime>().ConvertUsing<DateTimeConverter>();
                arg.CreateMap<DateTime?, DateTime?>().ConvertUsing<NullableDateTimeConverter>();
            });
        }

        public class DateTimeConverter : ITypeConverter<DateTime?, DateTime>
        {
            public DateTime Convert(DateTime? source, DateTime destination, ResolutionContext context)
            {
                if (source.HasValue)
                    return source.Value;
                return default(DateTime);
            }
        }

        public class NullableDateTimeConverter : ITypeConverter<DateTime?, DateTime?>
        {
            public DateTime? Convert(DateTime? source, DateTime? destination, ResolutionContext context)
            {
                return source;
            }
        }

        public class DateTimeOffsetToDateTimeConverter : ITypeConverter<DateTimeOffset, DateTime>
        {
            public DateTime Convert(DateTimeOffset source, DateTime destination, ResolutionContext context)
            {
                return source.DateTime;
            }
        }
        public class DateTimeOffsetToDateTimeNullableConverter : ITypeConverter<DateTimeOffset, DateTime?>
        {
            public DateTime? Convert(DateTimeOffset source, DateTime? destination, ResolutionContext context)
            {
                return source.DateTime;
            }
        }
        public class DateTimeOffsetNullableToDateTimeNullableConverter : ITypeConverter<DateTimeOffset?, DateTime?>
        {
            public DateTime? Convert(DateTimeOffset? source, DateTime? destination, ResolutionContext context)
            {
                return source?.DateTime;
            }
        }
    }
}