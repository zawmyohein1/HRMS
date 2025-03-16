using Microsoft.EntityFrameworkCore;
using HRMS.API.Controllers.Helper;
using HRMS.DataAccess.Context;
using HRMS.DataAccess.Repositories.Implementations;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Services.Cores.Implementations;
using HRMS.Services.Cores.Interfaces;
using HRMS.Services.Validators.Implementations;
using HRMS.Services.Validators.Interfaces;
using HRMS.Services.Cores.Implementations.Setup;
using HRMS.Services.Cores.Interfaces.Setup;
using HRMS.Services.Validators.Implementations.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Implementations.Setup;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<HRMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRMSDb")));

builder.Services.AddAutoMapper(typeof(MappingProfile));// Register AutoMapper profiles

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IJobRoleRepository, JobRoleRepository>();
builder.Services.AddScoped<IJobHistoryRepository, JobHistoryRepository>();

// Register repository and service
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeValidator, EmployeeValidator>();

builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationValidator, LocationValidator>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentValidator, DepartmentValidator>();

builder.Services.AddScoped<IJobRoleService, JobRoleService>();
builder.Services.AddScoped<IJobRoleValidator, JobRoleValidator>();

builder.Services.AddScoped<IJobHistoryService, JobHistoryService>();
builder.Services.AddScoped<IJobHistoryValidator, JobHistoryValidator>();



builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure the database is created and updated
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HRMSDbContext>();
    dbContext.Database.Migrate(); // This will apply any pending migrations and create the database if it doesn't exist
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();