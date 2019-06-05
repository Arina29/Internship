using System.Web.Mvc;
using MED.BLL.Implimentation;
using MED.DAL.UnitOfWork;
using MED.Presentation.Controllers;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace MED
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUserBll, UserBll>();
            container.RegisterType<IDoctorBll, DoctorBll>();

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}