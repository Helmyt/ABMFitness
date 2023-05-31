using App.BLL.DataEnums;
using Microsoft.AspNetCore.Identity;
using App.DAL.Models;
using App.DAL.Context;

namespace App.PL.Context
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club()
                        {
                            Title = "Sports Club 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2023/04/woman-elliptical.jpg?quality=82&strip=1&w=800",
                            Description = "This is the description of the first cinema",
                            ClubCategory = ClubCategory.Sports,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                         },
                        new Club()
                        {
                            Title = "Joga Club 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2023/03/woman-bicycle-crunches.jpg?quality=82&strip=1&w=800",
                            Description = "This is the description of the first cinema",
                            ClubCategory = ClubCategory.Joga,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Club()
                        {
                            Title = "Workout Club",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2023/04/fit-man-protein-powder-shake.jpg?quality=82&strip=1&w=800",
                            Description = "This is the description of the first club",
                            ClubCategory = ClubCategory.Workout,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Club()
                        {
                            Title = "Running Club 3",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first club",
                            ClubCategory = ClubCategory.Running,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Michigan",
                                State = "NC"
                            }
                        }
                    });
                    context.SaveChanges();
                }
                //Routines
                if (!context.Routines.Any())
                {
                    context.Routines.AddRange(new List<Routine>()
                    {
                        new Routine()
                        {
                            Title = "Running Race 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            RoutineCategory = RoutineCategory.Marathon,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Routine()
                        {
                            Title = "Running Race 2",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            RoutineCategory = RoutineCategory.Jogging,
                            AddressId = 5,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Trainer))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Trainer));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string trainerUserEmail = "helmytkasiyan@gmail.com";

                var trainerUser = await userManager.FindByEmailAsync(trainerUserEmail);
                if (trainerUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "helmyt",
                        Email = trainerUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Pass123!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Trainer);
                }

                string appUserEmail = "andrii.kasiyan@lnu.edu.ua";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "appuser",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Pass123!");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}