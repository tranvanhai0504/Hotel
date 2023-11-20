using HotelServer;
using Microsoft.EntityFrameworkCore;
using HotelServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        
        var connectionString = builder.Configuration.GetConnectionString("DbConnection");


        Startup startup = new Startup();
        startup.ConfigureServices(builder.Services, connectionString);
        startup.ConfigAuthentication(builder);


        var app = builder.Build();
        startup.Configure(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        //app.Use(async (ctx, next) =>
        //{
        //    ctx.Response.Headers["Access-Control-Allow-Origin"] = "*";
        //    ctx.Response.Headers["Access-Control-Expose-Headers"] = "*";

        //    if (HttpMethods.IsOptions(ctx.Request.Method))
        //    {
        //        ctx.Response.Headers["Access-Control-Allow-Headers"] = "*";
        //        ctx.Response.Headers["Access-Control-Allow-Methods"] = "POST, GET, OPTION, PUT";
        //        await ctx.Response.CompleteAsync();
        //        return;
        //    }

        //    await next();
        //});

        app.MapControllers();

        app.Run();
    }
}