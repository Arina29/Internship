using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using MED.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MED.Presentation.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DAL.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.Models.ApplicationDbContext context)
        {
            if (!context.Specializations.Any())
            {
                context.Specializations.AddOrUpdate(m => m.Id,
                    new Specializations() { Id = Guid.NewGuid(), Name = "Allergists/Immunologists" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Audiologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Anesthesiologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Cardiologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Dentist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Dermatologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Endocrinologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Epidemiologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Infectious Disease Specialist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Internal Medicine Specialist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Medical Geneticist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Microbiologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Neonatologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Neurologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Neurosurgeon" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Obstetrician" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Oncologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Orthopedic Surgeon" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Pediatrician" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Physiologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Plastic Surgeon" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Psychiatrist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Radiologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Rheumatologist" },
                    new Specializations() { Id = Guid.NewGuid(), Name = "Surgeon" }
                );
                context.SaveChanges();
            }


            if (!context.Roles.Any())
            {
                // Initialize default identity roles
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                // RoleTypes is a class containing constant string values for different roles
                List<IdentityRole> identityRoles = new List<IdentityRole>();
                identityRoles.Add(new IdentityRole() { Name = "Admin" });
                foreach (IdentityRole role in identityRoles)
                {
                    manager.Create(role);
                }
            }

            if (!context.Users.Any())
            {
                // Initialize default user
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser admin = new ApplicationUser();
                admin.Email = "admin@admin.com";
                admin.UserName = "admin@admin.com";
                userManager.Create(admin, "1Admin!");
                userManager.AddToRole(admin.Id, "Admin");
            }
        }
    }
}
