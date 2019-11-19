using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using prid1920_g01.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace prid1920_g01
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // In production, the Angular files will be served from this directory


            // services.AddDbContext<Prid1920_g01Context>(opt => opt.UseSqlServer(
            //     Configuration.GetConnectionString("Prid1920-g01-mssql"))
            // );

            // services.AddDbContext<Prid1920_g01Context>(opt => opt.UseMySql(
            //     Configuration.GetConnectionString("Prid1920-g01-mysql"))
            // );

            services.AddDbContext<Prid1920_g01Context>(opt => {

                opt.UseLazyLoadingProxies();

                // opt.UseSqlServer(Configuration.GetConnectionString("prid1920-tuto-mssql"));

                opt.UseMySql(Configuration.GetConnectionString("Prid1920-g01-mysql"));

            });

            services.AddMvc()

                .AddJsonOptions(opt => {

                    /*  

                    ReferenceLoopHandling.Ignore: Json.NET will ignore objects in reference loops and not serialize them.

                    The first time an object is encountered it will be serialized as usual but if the object is 

                    encountered as a child object of itself the serializer will skip serializing it.

                    See: https://stackoverflow.com/a/14205542

                    */

                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                })

                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            //------------------------------
            // configure jwt authentication
            //------------------------------
            var key = Encoding.ASCII.GetBytes("my-super-secret-key");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                                {
                                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                    {
                                        context.Response.Headers.Add("Token-Expired", "true");
                                    }
                                    return Task.CompletedTask;
                                }
                    };
                });
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    // Utilisez cette ligne si vous voulez que VS lance le front-end angular quand vous démarrez l'app
                    //spa.UseAngularCliServer(npmScript: "start");
                    // Utilisez cette ligne si le front-end angular est exécuté en dehors de VS (ou dans une autre instance de VS)
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
