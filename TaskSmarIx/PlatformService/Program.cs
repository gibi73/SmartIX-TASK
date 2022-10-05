using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        //Add sercices myself
        builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
        builder.Services.AddGrpc();

        var env=builder.Environment;
         if (env.IsProduction())
         {
            Console.WriteLine("Mssql Data");
            builder.Services.AddDbContext<AppContextDb>(opt=> opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
        }
        else
        {
            Console.WriteLine("InMemory Data");
            builder.Services.AddDbContext<AppContextDb>(opt => opt.UseInMemoryDatabase("InMem"));
        }



        // Add services to the container.
        builder.Services.AddControllers();



        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();




        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();

   
         app.MapControllers();
         app.MapGrpcService<GrpcPlatformService>();
         app.MapGet("/ProtoFiles/Platforms.proto",async context=>
            {
                await context.Response.WriteAsync(File.ReadAllText("ProtoFiles/Platforms.proto"));
            });
        PrepDb.PrepPopulation(app,env.IsProduction());

        app.Run();
    }
}