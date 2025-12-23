using Microsoft.EntityFrameworkCore;

using SchoolManagement.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();



app.UseHttpsRedirection();

