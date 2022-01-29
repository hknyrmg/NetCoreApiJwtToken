using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenBasedAuth_NetCore.Context;
using TokenBasedAuth_NetCore.Models;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Services;
using TokenBasedAuth_NetCore.Services.Trivia;
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
            services.AddMvc(options => options.EnableEndpointRouting = false);
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
            #region CorsSection

            //Cors ayarları
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                       builder => builder.WithOrigins("http://localhost:4455")
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });

            #endregion CorsSection
            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddDbContext<UserDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString(ConstantKeys.DefaultConnection)));
            //services.AddDbContext<TriviaDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString(ConstantKeys.TriviaConnection)));


   
            // .NET 6 Style
            services.AddSqlServer<UserDbContext>(
                                Configuration.GetConnectionString(ConstantKeys.DefaultConnection));
            services.AddSqlServer<TriviaDbContext>(
                               Configuration.GetConnectionString(ConstantKeys.TriviaConnection));

            services.AddScoped<DbContext, UserDbContext>();
            services.AddScoped<DbContext, TriviaDbContext>();

            services.AddScoped<ICacheProvider, CacheProvider>();
     
            services.AddMemoryCache();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITriviaService, TriviaService>();
            services.AddScoped<IDapperService, DapperService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

          

            app.UseHttpsRedirection();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            #region CorsSection

            app.UseCors("CorsPolicy");

            #endregion CorsSection
            app.UseMvc();

        }
    }
}
