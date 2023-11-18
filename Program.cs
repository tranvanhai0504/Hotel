using HotelServer;
using Microsoft.EntityFrameworkCore;
using HotelServer.Data;

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

        app.MapControllers();

        app.Run();
    }
}