using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using prid1920_g01.Models;

namespace prid1920_g01.Models
{

    public static class SeedData
    {
        public static IWebHost Seed(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<Prid1920_g01Context>())
                {
                    try
                    {
                        if (context.Users.Count() == 0)
                        {
 
                            context.Users.AddRange(
                                new User { Pseudo = "Benito", Password = "Ben", Email = "Ben@epfc.eu", LastName = "Penelle", FirstName = "Ben", Reputation = 1, Role = Role.Admin },
                                new User { Pseudo = "Bruno", Password = "Bruno", Email = "Bruno@epfc.eu", LastName = "Lacroix", FirstName = "Bru", Reputation = 1 }
                            );
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return webHost;
        }
    }
}