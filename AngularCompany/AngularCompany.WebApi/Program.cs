using AngularCompany.Core.Security;
using AngularCompany.Core.Services.Implementation;
using AngularCompany.Core.Services.Interface;
using AngularCompany.DataLayer.Context;
using AngularCompany.DataLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AngularCompanyDbContext>(option =>
{
    var connection = "Development:Developer";
    option.UseSqlServer(builder.Configuration.GetConnectionString(connection));
});
#region service and ripository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
#endregion

#region jwt bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7029",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularCompanyJwtBearer"))
    };
});
#endregion

#region allow cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCors",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .Build()
                    );
    //options.AddPolicy("EnableCors", builder =>
    //{
    //    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
    //});
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions() // for files in content folder  
{
    FileProvider = new PhysicalFileProvider(
         Path.Combine(Directory.GetCurrentDirectory(), "content")),
    // RequestPath = new PathString("/outside-content")
});

app.UseCors("EnableCors");
app.UseAuthentication();

app.Run();
