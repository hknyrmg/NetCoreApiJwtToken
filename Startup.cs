using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TokenBasedAuth_NetCore.Context;
using TokenBasedAuth_NetCore.Models;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Services;
using TokenBasedAuth_NetCore.UnitofWork;
using TokenBasedAuth_NetCore.Utilities.Helper;
using TokenBasedAuth_NetCore.Utils;

namespace TokenBasedAuth_NetCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region JWT Token
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion
            services.AddCors();

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<UserDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString(ConstantKeys.DefaultConnection)));
            services.AddScoped<DbContext, UserDbContext>();
            services.AddScoped<ICacheProvider, CacheProvider>();
            // configure DI for application services
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMemoryCache();
            services.AddScoped<IUserService, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
