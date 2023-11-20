
using HotelServer.Common;
using HotelServer.Data;
using HotelServer.Data.Infrastructure;
using HotelServer.Data.Respositories;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;

namespace HotelServer
{
    public class Startup
    {
        public string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services, string connectString)
        {
//            services.AddAuthorization(options =>
//{
//                options.AddPolicy("role",
//                    policy => policy.RequireClaim("admin"));
//            });

            //services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(connectString));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IDbFactory, DbFactory>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            //hotel
            services.AddSingleton<IHotelRepository, HotelRepository>();
            services.AddSingleton<IHotelService, HotelService>();
            //bill
            services.AddSingleton<IBillRepository, BillRepository>();
            services.AddSingleton<IBillService, BillService>();

            services.AddTransient<IMailService, MailService>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddControllers().AddJsonOptions(x =>
                        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<HotelDbContext>();
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<HotelDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                  });
            });
        }  

        public void ConfigAuthentication(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new
                Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:validaudience"],
                    ValidIssuer = builder.Configuration["jwt:validissuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:IssuerSigningKey"]))
                };
            });


        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseCors(MyAllowSpecificOrigins);
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


        }

    }
}