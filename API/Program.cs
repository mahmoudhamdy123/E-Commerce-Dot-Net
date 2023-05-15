using API.Middleware;
using Core.Interfaces;
using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Infastructure.Data.StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefualtConnection"));
});


builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configurationOptions = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("Redis")
    );
    return ConnectionMultiplexer.Connect(configurationOptions);
});


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//Add Auto Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(opt =>
opt.AddPolicy("CoresPolicy", policy =>
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();


app.UseStaticFiles();

app.UseCors("CoresPolicy");

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var Services = scope.ServiceProvider;
var context = Services.GetRequiredService<StoreContext>();
var Logger = Services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Logger.LogError(ex, "Error occurred while migrating process");
}

app.Run();

