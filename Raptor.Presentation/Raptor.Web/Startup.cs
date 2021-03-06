﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raptor.Data;
using Raptor.Data.Core;
using Raptor.Data.Models.Blog;
using Raptor.Data.Models.Configuration;
using Raptor.Data.Models.Content;
using Raptor.Data.Models.Logging;
using Raptor.Data.Models.Security;
using Raptor.Data.Models.Users;
using Raptor.Services.Authentication;
using Raptor.Services.Blog;
using Raptor.Services.Configuration;
using Raptor.Services.Content;
using Raptor.Services.Helpers;
using Raptor.Services.Logging;
using Raptor.Services.Security;
using Raptor.Services.Users;
using Raptor.Web.Areas.Admin.ViewModels;

namespace Raptor.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env) {
            // Build the basic configuration for our web app
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {

            // Register our Database context
            var connectionString = Configuration["ConnectionStrings:RaptorDbConnectionString"];
            services.AddDbContext<RaptorDbContext>(o => o.UseNpgsql(connectionString));

            // Enable lowercases routes for urls
            services.AddRouting(options => options.LowercaseUrls = true);

            // Add MVC to our web app
            services.AddMvc();

            // Add cookie authentication
            services.AddAuthentication(RaptorCookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(RaptorCookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
            });

            services.AddAuthorization();

            // Register the repositories
            services.AddTransient<IRepository<BlogComment>, Repository<BlogComment>>();
            services.AddTransient<IRepository<BlogPost>, Repository<BlogPost>>();
            services.AddTransient<IRepository<BlogPostCategory>, Repository<BlogPostCategory>>();
            services.AddTransient<IRepository<Taxonomy>, Repository<Taxonomy>>();
            services.AddTransient<IRepository<Term>, Repository<Term>>();
            services.AddTransient<IRepository<TermRelationship>, Repository<TermRelationship>>();
            services.AddTransient<IRepository<ActivityLog>, Repository<ActivityLog>>();
            services.AddTransient<IRepository<Log>, Repository<Log>>();
            services.AddTransient<IRepository<PermissionRecord>, Repository<PermissionRecord>>();
            services.AddTransient<IRepository<RolePermission>, Repository<RolePermission>>();
            services.AddTransient<IRepository<Address>, Repository<Address>>();
            services.AddTransient<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
            services.AddTransient<IRepository<BusinessEntityAddress>, Repository<BusinessEntityAddress>>();
            services.AddTransient<IRepository<Password>, Repository<Password>>();
            services.AddTransient<IRepository<Person>, Repository<Person>>();
            services.AddTransient<IRepository<PersonRole>, Repository<PersonRole>>();
            services.AddTransient<IRepository<PhoneNumber>, Repository<PhoneNumber>>();
            services.AddTransient<IRepository<Role>, Repository<Role>>();
            services.AddTransient<IRepository<ActivityLogType>, Repository<ActivityLogType>>();
            services.AddTransient<IRepository<ForgotPasswordRequest>, Repository<ForgotPasswordRequest>>();
            services.AddTransient<IRepository<Setting>, Repository<Setting>>();
            services.AddTransient<IRepository<SocialProfile>, Repository<SocialProfile>>();

            // Register our services
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<IContentService, ContentService>();
            services.AddTransient<ICustomerActivityService, CustomerActivityService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IUserRegisterationService, UserRegistrationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IUserAuthenticationService, CookieAuthenticationService>();
            services.AddSingleton<IWorkContext, WebWorkContext>();
            services.AddTransient<IUserRolesService, UserRolesService>();

            // Configure AutoMapper
            Mapper.Initialize(x => {
                x.CreateMap<Person, UserViewModel>(MemberList.None);
                x.CreateMap<UserViewModel, Person>(MemberList.None)
                .ForMember(entity => entity.Password, password => password.Ignore());
                x.CreateMap<Role, RoleViewModel>(MemberList.None);
                x.CreateMap<RoleViewModel, Role>(MemberList.None);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            
            // Enable Authentication
            app.UseAuthentication();

            // Enable logger factory
            loggerFactory.AddConsole();

            // Register the routes for areas
            app.UseMvc(routes => {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable exception pages
            app.UseDeveloperExceptionPage();

            // Allow to serve static files
            app.UseStaticFiles();
        }
    }
}
